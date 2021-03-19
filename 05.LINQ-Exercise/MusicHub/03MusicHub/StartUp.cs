namespace MusicHub
{
    using System;
    using System.Text;
    using System.Linq;

    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context = 
                new MusicHubDbContext();

            //DbInitializer.ResetDatabase(context);

            //Test your solutions here

            //var producerResult = ExportAlbumsInfo(context, 9);
            //Console.WriteLine(producerResult);

            var songDuration = ExportSongsAboveDuration(context, 4);
            Console.WriteLine(songDuration);
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            StringBuilder sb = new StringBuilder();
            var producerAlbums = context.Albums
                .Include(x => x.Songs).ThenInclude(x => x.Writer)
                .Include(x => x.Producer)
                .Where(x => x.Producer.Id == producerId)
                .OrderByDescending(x => x.Songs.Sum(y => y.Price))
                .ToList();

            foreach (var albumItem in producerAlbums)
            {
                decimal albumPrice = albumItem.Price;
                int counter = 0;

                sb.AppendLine($"-AlbumName: {albumItem.Name}");
                sb.AppendLine($"-ReleaseDate: {albumItem.ReleaseDate.ToString("MM/dd/yyyy")}");
                sb.AppendLine($"-ProducerName: {albumItem.Producer.Name}");
                sb.AppendLine($"-Songs:");
                
                foreach (var songItem in albumItem.Songs
                    .OrderByDescending(x => x.Name).ThenBy(y => y.Writer.Name))
                {
                    counter++;
                    sb.AppendLine($"---#{counter}");
                    sb.AppendLine($"---SongName: {songItem.Name}");
                    sb.AppendLine($"---Price: {songItem.Price:F2}");
                    sb.AppendLine($"---Writer: {songItem.Writer.Name}");
                }
                sb.AppendLine($"-AlbumPrice: {albumPrice:F2}");
            }

            return sb.ToString().Trim();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songItems = context.Songs
                .Select(s => new
                {
                    Writer = s.Writer.Name,
                    Performer = s.SongPerformers
                        .Select(pn => pn.Performer.FirstName + " " + pn.Performer.LastName)
                        .FirstOrDefault(),
                    SongName = s.Name,
                    AlbumProducer = s.Album.Producer.Name,
                    Duration = s.Duration
                })
                .ToList()
                .Where(s => s.Duration.TotalSeconds > duration)
                .OrderBy(s => s.SongName)
                .ThenBy(s => s.Writer)
                .ThenBy(s => s.Performer)
                .ToList();

            StringBuilder sb = new StringBuilder();
            int counter = 0;
            foreach (var song in songItems)
            {
                counter++;
                sb.AppendLine($"-Song #{counter}");
                sb.AppendLine($"---SongName: {song.SongName}");
                sb.AppendLine($"---Writer: {song.Writer}");
                sb.AppendLine($"---Performer: {song.Performer}");
                sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                sb.AppendLine($"---Duration: {song.Duration:c}");
            }
            return sb.ToString().Trim();
        }
    }
}

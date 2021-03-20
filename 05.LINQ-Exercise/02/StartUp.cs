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

            var producerResult = ExportAlbumsInfo(context, 9);
            Console.WriteLine(producerResult);

        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            StringBuilder sb = new StringBuilder();
            var producerAlbums = context.Albums
                .Include(x => x.Songs).ThenInclude(x => x.Writer)
                .Include(x => x.Producer)
                .Where(x => x.Producer.Id == producerId)
                .OrderByDescending(x => x.Songs.Sum(x => x.Price))
                .ToList();

            foreach (var albumItem in producerAlbums)
            {
                decimal albumPrice = albumItem.Songs.Sum(x => x.Price);
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
            throw new NotImplementedException();
        }
    }
}

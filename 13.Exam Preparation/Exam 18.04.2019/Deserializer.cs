namespace MusicHub.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using ImportDtos;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data";

        private const string SuccessfullyImportedWriter
            = "Imported {0}";
        private const string SuccessfullyImportedProducerWithPhone
            = "Imported {0} with phone: {1} produces {2} albums";
        private const string SuccessfullyImportedProducerWithNoPhone
            = "Imported {0} with no phone number produces {1} albums";
        private const string SuccessfullyImportedSong
            = "Imported {0} ({1} genre) with duration {2}";
        private const string SuccessfullyImportedPerformer
            = "Imported {0} ({1} songs)";

        public static string ImportWriters(MusicHubDbContext context, string jsonString)
        {
            var writersDto = JsonConvert.DeserializeObject<List<WriterDto>>(jsonString);

            StringBuilder sb = new StringBuilder();
            var validWriters = new List<Writer>();

            foreach (var writerDto in writersDto)
            {
                if (!IsValid(writerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var writer = new Writer
                {
                    Name = writerDto.Name,
                    Pseudonym = writerDto.Pseudonym
                };

                validWriters.Add(writer);
                sb.AppendLine(String.Format(SuccessfullyImportedWriter, writer.Name));
            }

            context.Writers.AddRange(validWriters);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducersAlbums(MusicHubDbContext context, string jsonString)
        {
            var producersDto = JsonConvert.DeserializeObject<List<ProducerDto>>(jsonString);

            StringBuilder sb = new StringBuilder();
            var validProducers = new List<Producer>();

            foreach (var producerDto in producersDto)
            {
                if (!IsValid(producerDto) || !producerDto.Albums.All(IsValid))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var producer = new Producer
                {
                    Name = producerDto.Name,
                    Pseudonym = producerDto.Pseudonym,
                    PhoneNumber = producerDto.PhoneNumber,
                    Albums = producerDto.Albums
                        .Select(a => new Album
                        {
                            Name = a.Name,
                            ReleaseDate = DateTime.ParseExact(a.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                        })
                        .ToList()
                };

                if (producer.PhoneNumber == null)
                {
                    sb.AppendLine(String.Format(SuccessfullyImportedProducerWithNoPhone, producer.Name,
                        producer.Albums.Count));
                }
                else
                {
                    sb.AppendLine(String.Format(SuccessfullyImportedProducerWithPhone, producer.Name, producer.PhoneNumber,
                        producer.Albums.Count));
                }

                validProducers.Add(producer);
            }

            context.Producers.AddRange(validProducers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSongs(MusicHubDbContext context, string xmlString)
        {
            var attr = new XmlRootAttribute("Songs");
            var serializer = new XmlSerializer(typeof(List<SongDto>), attr);

            StringBuilder sb = new StringBuilder();
            var validSongs = new List<Song>();

            using (StringReader reader = new StringReader(xmlString))
            {
                var songsDto = (List<SongDto>)serializer.Deserialize(reader);

                foreach (var songDto in songsDto)
                {
                    if (!IsValid(songDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var genre = Enum.TryParse(songDto.Genre, out Genre genreResult);
                    var album = context.Albums.Find(songDto.AlbumId);
                    var writer = context.Writers.Find(songDto.WriterId);
                    var songNameExists = validSongs.Any(s => s.Name == songDto.Name);

                    if (genre == false || album == null || writer == null || songNameExists == true)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var song = new Song
                    {
                        Name = songDto.Name,
                        Duration = TimeSpan.ParseExact(songDto.Duration, "c", CultureInfo.InvariantCulture),
                        CreatedOn = DateTime.ParseExact(songDto.CreatedOn, @"dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Genre = genreResult,
                        AlbumId = songDto.AlbumId,
                        WriterId = songDto.WriterId,
                        Price = songDto.Price
                    };

                    sb.AppendLine(string.Format(SuccessfullyImportedSong, song.Name, song.Genre, song.Duration));
                    validSongs.Add(song);
                }
            }

            context.Songs.AddRange(validSongs);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSongPerformers(MusicHubDbContext context, string xmlString)
        {
            var attr = new XmlRootAttribute("Performers");
            var serializer = new XmlSerializer(typeof(List<PerformerDto>), attr);

            var validSongIds = context
                .Songs
                .Select(s => s.Id)
                .ToList();

            StringBuilder sb = new StringBuilder();
            var validPerformers = new List<Performer>();

            using (StringReader reader = new StringReader(xmlString))
            {
                var performersDto = (List<PerformerDto>)serializer.Deserialize(reader);

                foreach (var performerDto in performersDto)
                {
                    if (!IsValid(performerDto) || performerDto.PerformersSongs.Any(s => !validSongIds.Contains(s.Id)))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var performer = new Performer
                    {
                        FirstName = performerDto.FirstName,
                        LastName = performerDto.LastName,
                        Age = performerDto.Age,
                        NetWorth = performerDto.NetWorth
                    };

                    var performerSongs = new List<SongPerformer>();

                    foreach (var song in performerDto.PerformersSongs)
                    {
                        var performerSongToAdd = new SongPerformer
                        {
                            SongId = song.Id,
                            Performer = performer
                        };

                        performerSongs.Add(performerSongToAdd);
                    }

                    performer.PerformerSongs = performerSongs;
                    validPerformers.Add(performer);

                    sb.AppendLine(string.Format(SuccessfullyImportedPerformer, performer.FirstName, performerSongs.Count));
                }
            }

            context.Performers.AddRange(validPerformers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validationResult = new List<ValidationResult>();

            var result = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            return result;
        }
    }
}
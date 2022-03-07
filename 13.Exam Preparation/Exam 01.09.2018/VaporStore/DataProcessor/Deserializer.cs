namespace VaporStore.DataProcessor
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
    using Dto.Import;
    using Newtonsoft.Json;

    public static class Deserializer
    {
        //public static string ImportGames(VaporStoreDbContext context, string jsonString)
        //{
        //          var gamesDto = JsonConvert.DeserializeObject<List<GameImportDto>>(jsonString);

        //          StringBuilder sb = new StringBuilder();
        //          var validGames = new List<Game>();
        //          var validDevelopers = new List<Developer>();
        //          var validGenres = new List<Genre>();
        //          var validTags = new List<Tag>();

        //          foreach (var dto in gamesDto)
        //          {
        //              if (!IsValid(dto) || dto.Tags.Count == 0)
        //              {
        //                  sb.AppendLine("Invalid Data");
        //                  continue;
        //              }

        //              var developer = validDevelopers.FirstOrDefault(d => 
        //                                      d.Name == dto.Developer) ?? 
        //                              new Developer{Name = dto.Developer};
        //              validDevelopers.Add(developer);

        //              var genre = validGenres.FirstOrDefault(g => 
        //                                      g.Name == dto.Genre) ?? 
        //                              new Genre{Name = dto.Genre};
        //              validGenres.Add(genre);

        //              var gameTags = new List<Tag>();
        //              foreach (var dtoTag in dto.Tags)
        //              {
        //                  var tag = validTags.FirstOrDefault(t => 
        //                                    t.Name == dtoTag) ?? 
        //                            new Tag{Name = dtoTag};

        //                  validTags.Add(tag);
        //                  gameTags.Add(tag);
        //              }

        //              var game = new Game
        //              {
        //                  Name = dto.Name,
        //                  Price = dto.Price,
        //                  ReleaseDate = DateTime.ParseExact(dto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
        //                  Developer = developer,
        //                  Genre = genre
        //              };

        //              foreach (var tag in gameTags)
        //              {
        //                  var tagGameToAdd = new GameTag
        //                  {
        //                      Game = game,
        //                      Tag = tag
        //                  };

        //                  game.GameTags.Add(tagGameToAdd);
        //              }

        //              validGames.Add(game);
        //              sb.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count} tags");
        //          }

        //          context.Games.AddRange(validGames);
        //          context.SaveChanges();

        //          return sb.ToString().TrimEnd();
        //}

        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var gamesDto = JsonConvert.DeserializeObject<List<GameImportDto>>(jsonString);

            StringBuilder sb = new StringBuilder();
            var validGames = new List<Game>();

            foreach (var dto in gamesDto)
            {
                if (!IsValid(dto) || dto.Tags.Count == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var game = new Game
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    ReleaseDate = DateTime.ParseExact(dto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                };

                var developer = GetDeveloper(context, dto.Developer);
                var genre = GetGenre(context, dto.Genre);

                game.Developer = developer;
                game.Genre = genre;

                foreach (var dtoTag in dto.Tags)
                {
                    var tag = GetTag(context, dtoTag);

                    game.GameTags.Add(new GameTag
                    {
                        Game = game,
                        Tag = tag
                    });
                }
                
                validGames.Add(game);
                sb.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count} tags");
            }

            context.Games.AddRange(validGames);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static Tag GetTag(VaporStoreDbContext context, string dtoTag)
        {
            var tag = context.Tags.FirstOrDefault(t => t.Name == dtoTag);

            if (tag == null)
            {
                tag = new Tag
                {
                    Name = dtoTag
                };

                context.Tags.Add(tag);
                context.SaveChanges();
            }

            return tag;
        }

        private static Genre GetGenre(VaporStoreDbContext context, string dtoGenre)
        {
            var genre = context.Genres.FirstOrDefault(g => g.Name == dtoGenre);

            if (genre == null)
            {
                genre = new Genre
                {
                    Name = dtoGenre
                };

                context.Genres.Add(genre);
                context.SaveChanges();
            }

            return genre;
        }

        private static Developer GetDeveloper(VaporStoreDbContext context, string dtoDeveloper)
        {
            var developer = context.Developers.FirstOrDefault(d => d.Name == dtoDeveloper);

            if (developer == null)
            {
                developer = new Developer
                {
                    Name = dtoDeveloper,
                };

                context.Developers.Add(developer);
                context.SaveChanges();
            }

            return developer;
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var usersDto = JsonConvert.DeserializeObject<List<UserImportDto>>(jsonString);

            StringBuilder sb = new StringBuilder();
            var validUsers = new List<User>();

            foreach (var dto in usersDto)
            {
                if (!IsValid(dto) || !dto.Cards.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var user = new User
                {
                    Username = dto.Username,
                    FullName = dto.FullName,
                    Email = dto.Email,
                    Age = dto.Age
                };

                foreach (var card in dto.Cards)
                {
                    var type = Enum.TryParse(card.Type, out CardType resultType);

                    if (!type)
                    {
                        continue;
                    }

                    var cardToAdd = new Card
                    {
                        Number = card.Number,
                        Cvc = card.Cvc,
                        Type = resultType,
                        User = user
                    };

                    user.Cards.Add(cardToAdd);
                }

                validUsers.Add(user);
                sb.AppendLine($"Imported {user.Username} with {user.Cards.Count} cards");
            }

            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            var attr = new XmlRootAttribute("Purchases");
            var serializer = new XmlSerializer(typeof(List<PurchaseImportDto>), attr);

            StringBuilder sb = new StringBuilder();
            var validPurchases = new List<Purchase>();

            using (StringReader reader = new StringReader(xmlString))
            {
                var purchasesDto = (List<PurchaseImportDto>)serializer.Deserialize(reader);

                foreach (var dto in purchasesDto)
                {
                    if (!IsValid(dto))
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    }

                    var resultType = Enum.TryParse(dto.Type, out PurchaseType type);

                    var card = context
                        .Cards
                        .FirstOrDefault(c => c.Number == dto.Card);

                    var game = context
                        .Games
                        .FirstOrDefault(g => g.Name == dto.Title);

                    if (resultType == false
                        || card == null
                        || game == null)
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    }

                    var purchase = new Purchase
                    {
                        Type = type,
                        ProductKey = dto.ProductKey,
                        Date = DateTime.ParseExact(dto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                        Card = card,
                        Game = game
                    };

                    validPurchases.Add(purchase);

                    sb.AppendLine($"Imported {purchase.Game.Name} for {purchase.Card.User.Username}");
                }
            }

            context.Purchases.AddRange(validPurchases);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(this object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validationResult = new List<ValidationResult>();

            var result = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            return result;
        }
    }
}
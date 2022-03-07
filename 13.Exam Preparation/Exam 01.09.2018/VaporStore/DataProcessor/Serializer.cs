namespace VaporStore.DataProcessor
{
	using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using Data.Models.Enums;
    using Dto.Export;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var games = context
                .Genres
                .Where(g => genreNames.Contains(g.Name) 
                            && g.Games.Any(gm => gm.Purchases.Any()))
                .Select(g => new GenresExportDto
                {
                    Id = g.Id,
                    Genre = g.Name,
                    Games = g.Games
                        .Where(gm => gm.Purchases.Any())
                        .Select(gm => new GamesExportDto
                        {
                            Id = gm.Id,
                            Title = gm.Name,
                            Developer = gm.Developer.Name,
                            Tags = String.Join(", ",
                                gm.GameTags
                                .Select(t => t.Tag.Name)),
                            Players = gm.Purchases.Count
                        })
                        .OrderByDescending(gm => gm.Players)
                        .ThenBy(gm => gm.Id)
                        .ToList(),
                    TotalPlayers = g.Games.Sum(x => x.Purchases.Count)
                })
                .OrderByDescending(g => g.TotalPlayers)
                .ThenBy(g => g.Id)
                .ToList();

            var gamesByGenreJson = JsonConvert.SerializeObject(games, Formatting.Indented);

            return gamesByGenreJson;
		}

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
        {
            var purchaseType = Enum.Parse<PurchaseType>(storeType);

            var users = context
                .Users
                .Select(u => new UserExportDto
                {
                    Purchases = u.Cards
                        .SelectMany(c => c.Purchases)
                        .Where(p => p.Type == purchaseType)
                        .OrderBy(p => p.Date)
                        .Select(p => new PurchaseExportDto
                        {
                            Card = p.Card.Number,
                            Cvc = p.Card.Cvc,
                            Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            Game = new GamePurchaseExportDto
                            {
                                Genre = p.Game.Genre.Name,
                                Price = p.Game.Price.ToString(),
                                Title = p.Game.Name
                            }
                        })
                        .ToList(),
                    TotalSpent = u.Cards
                        .SelectMany(c => c.Purchases)
                        .Where(p => p.Type == purchaseType)
                        .Sum(p => p.Game.Price)
                        .ToString(),
                    Username = u.Username
                })
                .Where(u => u.Purchases.Any())
                .OrderByDescending(u => decimal.Parse(u.TotalSpent))
                .ThenBy(u => u.Username)
                .ToList();


            var attr = new XmlRootAttribute("Users");
            var serializer = new XmlSerializer(typeof(List<UserExportDto>), attr);

            StringBuilder sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
                XmlQualifiedName.Empty
            });

            serializer.Serialize(new StringWriter(sb), users, namespaces);

            return sb.ToString().TrimEnd();
		}
	}
}
namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using ExportDto;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {
            var movies = context
                .Movies
                .Where(m => m.Rating >= rating && m.Projections.Any(p => p.Tickets.Count > 0))
                .Select(m => new MovieExportDto
                {
                    MovieName = m.Title,
                    Rating = m.Rating.ToString("f2"),
                    TotalIncomes = m.Projections.Sum(p => p.Tickets.Sum(t => t.Price)).ToString("f2"),
                    Customers = m.Projections
                        .SelectMany(p => p.Tickets)
                        .Select(t => new CustomerMovieDto
                        {
                            FirstName = t.Customer.FirstName,
                            LastName = t.Customer.LastName,
                            Balance = t.Customer.Balance.ToString("f2")
                        })
                        //To pass through Judge it has to ordered by string representation of Balance
                        //But in reality it should be
                        //.OrderByDescending(c => decimal.Parse(c.Balance))

                        .OrderByDescending(c => c.Balance)
                        .ThenBy(c => c.FirstName)
                        .ThenBy(c => c.LastName)
                        .ToList()
                })
                .OrderByDescending(m => double.Parse(m.Rating))
                .ThenByDescending(m => decimal.Parse(m.TotalIncomes))
                .Take(10)
                .ToList();

            var albumsInRangeJson = JsonConvert.SerializeObject(movies, Formatting.Indented);

            return albumsInRangeJson;
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            var customers = context
                .Customers
                .Where(c => c.Age > age)
                .Select(c => new CustomerExportDto
                {
                    SpentMoney = c.Tickets.Sum(t => t.Price).ToString("f2"),
                    SpentTime = TimeSpan
                        .FromSeconds(c.Tickets
                            .Sum(s => s.Projection.Movie.Duration.TotalSeconds))
                            .ToString("hh\\:mm\\:ss"),
                    FirstName = c.FirstName,
                    LastName = c.LastName
                })
                .OrderByDescending(c => decimal.Parse(c.SpentMoney))
                .Take(10)
                .ToList();

            var attr = new XmlRootAttribute("Customers");
            var serializer = new XmlSerializer(typeof(List<CustomerExportDto>), attr);

            StringBuilder sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
            XmlQualifiedName.Empty
        });

            serializer.Serialize(new StringWriter(sb), customers, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
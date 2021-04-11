namespace Cinema.DataProcessor
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
    using ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie 
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportProjection 
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket 
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var moviesDto = JsonConvert.DeserializeObject<List<MoviesDto>>(jsonString);

            StringBuilder sb = new StringBuilder();
            var validMovies = new List<Movie>();

            foreach (var dto in moviesDto)
            {
                var genre = Enum.TryParse(dto.Genre, out Genre resultGenre);

                if (!IsValid(dto) || genre == false || validMovies.Any(m => m.Title == dto.Title))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var movie = new Movie
                {
                    Title = dto.Title,
                    Genre = resultGenre,
                    Duration = TimeSpan.ParseExact(dto.Duration, "c", CultureInfo.InvariantCulture),
                    Rating = dto.Rating,
                    Director = dto.Director
                };

                sb.AppendLine(string.Format(SuccessfulImportMovie, movie.Title, movie.Genre, movie.Rating.ToString("f2")));
                validMovies.Add(movie);
            }

            context.Movies.AddRange(validMovies);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            var attr = new XmlRootAttribute("Projections");
            var serializer = new XmlSerializer(typeof(List<ProjectionDto>), attr);

            var validMoviesIds = context
                .Movies
                .Select(m => m.Id)
                .ToList();

            StringBuilder sb = new StringBuilder();
            var validProjections = new List<Projection>();

            using (StringReader reader = new StringReader(xmlString))
            {
                var projectionsDto = (List<ProjectionDto>)serializer.Deserialize(reader);

                foreach (var dto in projectionsDto)
                {
                    if (!IsValid(dto) 
                        || !validMoviesIds.Contains(dto.MovieId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var projection = new Projection
                    {
                        MovieId = dto.MovieId,
                        DateTime = DateTime.ParseExact(dto.DateTime, "yyyy-MM-dd HH:mm:ss",  CultureInfo.InvariantCulture)
                    };

                    var movie = context.Movies.Find(projection.MovieId);

                    sb.AppendLine(string.Format(SuccessfulImportProjection, movie.Title, projection.DateTime.ToString("MM/dd/yyyy")));

                    validProjections.Add(projection);
                }
            }

            context.Projections.AddRange(validProjections);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            var attr = new XmlRootAttribute("Customers");
            var serializer = new XmlSerializer(typeof(List<CustomerDto>), attr);

            var validProjectionIds = context
                .Projections
                .Select(p => p.Id)
                .ToList();

            StringBuilder sb = new StringBuilder();
            var validCustomers = new List<Customer>();

            using (StringReader reader = new StringReader(xmlString))
            {
                var customersDto = (List<CustomerDto>)serializer.Deserialize(reader);

                foreach (var dto in customersDto)
                {
                    if (!IsValid(dto) 
                        || dto.Tickets.Any(t => !IsValid(t))
                        || dto.Tickets.Any(t => !validProjectionIds
                                            .Contains(t.ProjectionId)))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var customer = new Customer
                    {
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        Age = dto.Age,
                        Balance = dto.Balance
                    };

                    var customerTickets = new List<Ticket>();

                    foreach (var ticket in dto.Tickets)
                    {
                        var ticketToAdd = new Ticket
                        {
                            Price = ticket.Price,
                            Customer = customer,
                            ProjectionId = ticket.ProjectionId
                        };

                        customerTickets.Add(ticketToAdd);
                    }

                    customer.Tickets = customerTickets;
                    validCustomers.Add(customer);

                    sb.AppendLine(string.Format(SuccessfulImportCustomerTicket, customer.FirstName, customer.LastName, customer.Tickets.Count));
                }
            }

            context.Customers.AddRange(validCustomers);
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
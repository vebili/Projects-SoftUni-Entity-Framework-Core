namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportBookDto[]), new XmlRootAttribute("Books"));

            var bookDtos = (ImportBookDto[])xmlSerializer.Deserialize(new StringReader(xmlString));

            var books = new List<Book>();

            foreach (var dto in bookDtos)
            {
                if (!IsValid(dto) || dto.Price <= 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var book = new Book()
                {
                    Name = dto.Name,
                    Genre = (Genre)dto.Genre,
                    Price = dto.Price,
                    Pages = dto.Pages,
                    PublishedOn = DateTime.ParseExact(dto.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture)
                };

                books.Add(book);
                sb.AppendLine(String.Format(SuccessfullyImportedBook, book.Name, book.Price));
            }

            context.Books.AddRange(books);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var authorsDtos = JsonConvert.DeserializeObject<ImportAuthorDto[]>(jsonString, new JsonSerializerSettings()
            { 
                NullValueHandling = NullValueHandling.Ignore
            });

            var authors = new List<Author>();

            foreach (var dto in authorsDtos)
            {
                if (!IsValid(dto) || !IsPhoneValid(dto.Phone))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool emailExists = context
                    .Authors
                    .Any(a => a.Email == dto.Email);

                if (emailExists)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var author = new Author()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Phone = dto.Phone
                };

                foreach (var bookDto in dto.Books)
                {
                    var book = context
                        .Books
                        .Find(bookDto.Id);

                    if (book == null)
                    {
                        continue;
                    }

                    author.AuthorsBooks.Add(new AuthorBook()
                    {
                        Author = author,
                        BookId = bookDto.Id
                    });
                }

                if (author.AuthorsBooks.Count == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                authors.Add(author);

                string authorFullname = author.FirstName + " " + author.LastName;
                int authorBooksCount = author.AuthorsBooks.Count;
                sb.AppendLine(String.Format(SuccessfullyImportedAuthor, authorFullname, authorBooksCount));
            }

            context.Authors.AddRange(authors);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

        private static bool IsPhoneValid(string phone)
        {
            return Regex.IsMatch(phone, @"^\d{3}\-\d{3}\-\d{4}$");
        }
    }
}
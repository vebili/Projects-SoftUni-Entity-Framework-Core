namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System;
    using System.Text;
    using System.Linq;
    using System.Collections.Generic;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            Console.Write("Enter the Categories: ");
            string categories = Console.ReadLine();

            string resultGet = GetBooksByCategory(db, categories);
            Console.WriteLine(resultGet);

        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            List<string> categories = input
                .ToLower()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            var booksByCategory = context.BooksCategories
                .Where(bc => categories.Contains(bc.Category.Name.ToLower()))
                .Select(bc => bc.Book.Title)
                .OrderBy(bc => bc)
                .ToList();
            string resultOut = string.Join(Environment.NewLine, booksByCategory).Trim();
            return resultOut;
        }

        //==========================================================================

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var booksNotReleased = context.Books
                .Where(b => !(b.ReleaseDate.HasValue && b.ReleaseDate.Value.Year == year))
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, booksNotReleased).Trim();
        }

        //===========================================================================

        public static string GetBooksByPrice(BookShopContext context)
        {
            var bookPrices = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    BookTitle = b.Title,
                    BookPrice = b.Price
                })
                .OrderByDescending(sb => sb.BookPrice)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var book in bookPrices)
            {
                sb.AppendLine($"{book.BookTitle} - ${book.BookPrice:F2}");
            }
            return sb.ToString().Trim();
        }

        //================================================================================

        public static string GetGoldenBooks(BookShopContext context)
        {
            var goldEdition = EditionType.Gold;

            var goldBooks = context.Books
                .Where(gb => gb.EditionType == goldEdition && gb.Copies < 5000)
                .OrderBy(gb => gb.BookId)
                .Select(gb => gb.Title)
                .ToList();

            return string.Join(Environment.NewLine, goldBooks).Trim();
        }

        //====================================================================================

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var restrictNum = (AgeRestriction)Enum.Parse(typeof(AgeRestriction), command, true);

            var restrictBooks = context.Books
                .Where(b => b.AgeRestriction == restrictNum)
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in restrictBooks)
            {
                sb.AppendLine($"{book}");
            }
            return sb.ToString().Trim();
        }
    }
}

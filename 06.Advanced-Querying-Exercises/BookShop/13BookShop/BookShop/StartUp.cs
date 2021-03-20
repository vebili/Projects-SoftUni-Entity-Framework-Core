namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            Console.Write("Enter the Title lenght: ");
            int bookNum = int.Parse(Console.ReadLine());

            int resultGet = CountBooks(db, bookNum);
            //Console.WriteLine(resultGet.Length);
            Console.WriteLine(resultGet);

        }
        //========== 13 ===================================================
        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var categoryBooks = context.Categories
                .Select(c => new
                {
                    Name = c.Name,
                    Books = c.CategoryBooks
                        .OrderByDescending(cb => cb.Book.ReleaseDate)
                        .Select(cb => new
                        {
                            Title = cb.Book.Title,
                            Date = cb.Book.ReleaseDate.Value.Year
                        })
                        .Take(3)
                        .ToList()
                })
                .ToList();

            foreach (var cb in categoryBooks.OrderBy(cb => cb.Name))
            {
                sb.AppendLine($"--{cb.Name}");

                foreach (var b in cb.Books)
                {
                    sb.AppendLine($"{b.Title} ({b.Date})");
                }
            }

            return sb.ToString().TrimEnd();
        }
		
        //========== 12 ===================================================
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var categoryProfit = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    Profit = c.CategoryBooks.Sum(cb => cb.Book.Copies * cb.Book.Price)
                })
                .OrderByDescending(cp => cp.Profit)
                .ThenBy(cp => cp.CategoryName)
                .ToList();

            foreach (var cp in categoryProfit)
            {
                sb.AppendLine($"{cp.CategoryName} ${cp.Profit:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //========== 11 ===================================================
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var resultAuthors = context.Authors
                .Select(a => new
                {
                    Name = a.FirstName + " " + a.LastName,
                    TotalCopies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(b => b.TotalCopies)
                .ToList();

            foreach (var author in resultAuthors)
            {
                sb.AppendLine($"{author.Name} - {author.TotalCopies}");
            }

            return sb.ToString().TrimEnd();
        }

        //========== T-10 ===================================================

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var numBooks = context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .ToList().Count;

            return numBooks;
        }

        //========== T-09 ===================================================

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            string lowCaseInput = input.ToLower();

            var booksByAutor = context.Books
                .Select(b => new
                {
                    b.BookId,
                    BookTitle = b.Title,
                    AuthorFirstName = b.Author.FirstName,
                    AuthorLastName = b.Author.LastName,
                })
                .Where(s => s.AuthorLastName.ToLower().StartsWith(lowCaseInput))
                .OrderBy(s => s.BookId)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var book in booksByAutor)
            {
                sb.AppendLine($"{book.BookTitle} ({book.AuthorFirstName} {book.AuthorLastName})");
            }
            return sb.ToString().Trim();
        }

        //========== T-08 ===================================================

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var titlesContain = context.Books
                .Where(t => (t.Title.ToLower()).Contains(input.ToLower()))
                .Select(t => t.Title)
                .OrderBy(t => t)
                .ToList();

            return string.Join(Environment.NewLine, titlesContain);
        }

        //========== T-07 ===================================================

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authorNames = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => a.FirstName + " " + a.LastName)
                .OrderBy(fn => fn)
                .ToList();

            return string.Join(Environment.NewLine, authorNames).Trim();
        }

        //=========== T-06 ======================================================

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime inpDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var booksBeforeDate = context.Books
                .Where(b => b.ReleaseDate.HasValue && b.ReleaseDate.Value < inpDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    BookTitle = b.Title,
                    BookEditionType = b.EditionType,
                    BookPrice = b.Price
                }).ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var book in booksBeforeDate)
            {
                sb.AppendLine($"{book.BookTitle} - {book.BookEditionType.ToString()} - ${book.BookPrice:F2}");
            }
            return sb.ToString().Trim();
        }

        //============ T-05 ==========================================================

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

        //============ T-04 ==============================================================

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var booksNotReleased = context.Books
                .Where(b => !(b.ReleaseDate.HasValue && b.ReleaseDate.Value.Year == year))
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, booksNotReleased).Trim();
        }

        //============= T-03 ==========================================================

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

        //============== T-02 ============================================================

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

        //=============== T-01 =============================================================

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

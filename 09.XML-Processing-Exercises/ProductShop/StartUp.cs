namespace ProductShop
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using Dtos.Export;
    using Dtos.Import;
    using Models;

    public class StartUp
    {
        private const string UsersXmlFilePath = @"../../../Datasets/users.xml";
        private const string ProductsXmlFilePath = @"../../../Datasets/products.xml";
        private const string CategoriesXmlFilePath = @"../../../Datasets/categories.xml";
        private const string CategoriesProductsXmlFilePath = @"../../../Datasets/categories-products.xml";
        private const string returnInputMessage = @"Successfully imported {0}";

        public static void Main(string[] args)
        {
            //var usersXml = File.ReadAllText(UsersXmlFilePath);
            //var productsXml = File.ReadAllText(ProductsXmlFilePath);
            //var categoriesXml = File.ReadAllText(CategoriesXmlFilePath);
            //var categoriesProductsXml = File.ReadAllText(CategoriesProductsXmlFilePath);

            using (var context = new ProductShopContext())
            {
                //Console.WriteLine(ImportUsers(context, usersXml));
                //Console.WriteLine(ImportProducts(context, productsXml));
                //Console.WriteLine(ImportCategories(context, categoriesXml));
                //Console.WriteLine(ImportCategoryProducts(context, categoriesProductsXml));

                //Console.WriteLine(GetProductsInRange(context));
                //Console.WriteLine(GetSoldProducts(context));
                //Console.WriteLine(GetCategoriesByProductsCount(context));
                Console.WriteLine(GetUsersWithProducts(context));
            }
        }


        //8
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context
                .Users
                .Where(u => u.ProductsSold.Any())
                .Select(u => new UserWithProductSExportDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new SoldProductsForUserExportDto
                    {
                        Count = u.ProductsSold.Count,
                        Products = u.ProductsSold
                            .Select(p => new ProductSoldDto
                            {
                                Name = p.Name,
                                Price = p.Price
                            })
                            .OrderByDescending(p => p.Price)
                            .ToList()
                    }
                })
                .OrderByDescending(u => u.SoldProducts.Count)
                .ToList();

            var allUsers = new AllUsersDto
            {
                Count = users.Count,
                Users = users.Take(10).ToList()
            };

            var attr = new XmlRootAttribute("Users");
            var serializer = new XmlSerializer(typeof(AllUsersDto), attr);

            StringBuilder sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
                XmlQualifiedName.Empty
            });

            serializer.Serialize(new StringWriter(sb), allUsers, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
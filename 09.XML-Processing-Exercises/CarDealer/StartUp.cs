namespace CarDealer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Dtos.Export;
    using Dtos.Import;
    using Models;

    public class StartUp
    {
        private const string CarsXmlFilePath = @"../../../Datasets/cars.xml";
        private const string CustomersXmlFilePath = @"../../../Datasets/customers.xml";
        private const string PartsXmlFilePath = @"../../../Datasets/parts.xml";
        private const string SalesXmlFilePath = @"../../../Datasets/sales.xml";
        private const string SuppliersXmlFilePath = @"../../../Datasets/suppliers.xml";
        private const string returnInputMessage = @"Successfully imported {0}";
        public static void Main(string[] args)
        {
            //var carsTextXml = File.ReadAllText(CarsXmlFilePath);
            //var customersTextXml = File.ReadAllText(CustomersXmlFilePath);
            //var partsTextXml = File.ReadAllText(PartsXmlFilePath);
            //var salesTextXml = File.ReadAllText(SalesXmlFilePath);
            //var suppliersTextXml = File.ReadAllText(SuppliersXmlFilePath);

            //Mapper.Initialize(cfg => cfg.AddProfile<CarDealerProfile>());

            using (var context = new CarDealerContext())
            {
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();

                //Console.WriteLine(ImportSuppliers(context, suppliersTextXml));
                //Console.WriteLine(ImportParts(context, partsTextXml));
                //Console.WriteLine(ImportCars(context, carsTextXml));
                //Console.WriteLine(ImportCustomers(context, customersTextXml));
                //Console.WriteLine(ImportSales(context, salesTextXml));

                //Console.WriteLine(GetCarsWithDistance(context));
                //Console.WriteLine(GetCarsFromMakeBmw(context));
                //Console.WriteLine(GetLocalSuppliers(context));
                //Console.WriteLine(GetCarsWithTheirListOfParts(context));
                //Console.WriteLine(GetTotalSalesByCustomer(context));
                Console.WriteLine(GetSalesWithAppliedDiscount(context));
            }
        }

        //9
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var attr = new XmlRootAttribute("Suppliers");
            var serializer = new XmlSerializer(typeof(List<SupplierImportDto>), attr);

            var suppliersDto = (List<SupplierImportDto>)serializer
                .Deserialize(new StringReader(inputXml));

            var suppliers = new List<Supplier>();

            foreach (var s in suppliersDto)
            {
                var supplier = new Supplier
                {
                    Name = s.Name,
                    IsImporter = s.IsImporter
                };

                suppliers.Add(supplier);
            }

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return String.Format(returnInputMessage, suppliers.Count);
        }

        //10

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var attr = new XmlRootAttribute("Parts");
            var serializer = new XmlSerializer(typeof(List<PartImportDto>), attr);

            var partsDto = (List<PartImportDto>)serializer
                .Deserialize(new StringReader(inputXml));

            var parts = new List<Part>();
            var validSupplierIds = context
                .Suppliers
                .Select(s => s.Id)
                .ToList();

            foreach (var p in partsDto)
            {
                if (!validSupplierIds.Contains(p.SupplierId))
                {
                    continue;
                }

                var part = new Part
                {
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    SupplierId = p.SupplierId
                };

                parts.Add(part);
            }

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return String.Format(returnInputMessage, parts.Count);
        }

        //11
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var attr = new XmlRootAttribute("Cars");
            var serializer = new XmlSerializer(typeof(List<CarImportDto>), attr);

            var carsDto = (List<CarImportDto>)serializer
                .Deserialize(new StringReader(inputXml));

            var validPartIds = context
                .Parts
                .Select(p => p.Id)
                .ToHashSet();

            var carParts = new HashSet<PartCar>();
            var cars = new HashSet<Car>();

            foreach (var car in carsDto)
            {
                var carToAdd = new Car
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TraveledDistance
                };

                foreach (var pc in car.Parts)
                {
                    if (validPartIds.Contains(pc.Id))
                    {
                        var carPartToAdd = new PartCar
                        {
                            Car = carToAdd,
                            PartId = pc.Id
                        };

                        if (carToAdd.PartCars.FirstOrDefault(p => p.PartId == pc.Id) == null)
                        {
                            carToAdd.PartCars.Add(carPartToAdd);
                            carParts.Add(carPartToAdd);
                        }
                    }
                }

                cars.Add(carToAdd);
            }

            context.Cars.AddRange(cars);
            context.PartCars.AddRange(carParts);
            context.SaveChanges();

            return String.Format(returnInputMessage, cars.Count);
        }

        //12
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var attr = new XmlRootAttribute("Customers");
            var serializer = new XmlSerializer(typeof(List<CustomerImportDto>), attr);

            var customersDto = (List<CustomerImportDto>)serializer
                .Deserialize(new StringReader(inputXml));

            var customers = new List<Customer>();

            foreach (var c in customersDto)
            {
                var customer = new Customer
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate,
                    IsYoungDriver = c.IsYoungDriver
                };

                customers.Add(customer);
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return String.Format(returnInputMessage, customers.Count);
        }

        //13
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var attr = new XmlRootAttribute("Sales");
            var serializer = new XmlSerializer(typeof(List<SaleImportDto>), attr);

            var salesDto = (List<SaleImportDto>)serializer
                .Deserialize(new StringReader(inputXml));

            var sales = new List<Sale>();
            var validCarIds = context
                .Cars
                .Select(c => c.Id)
                .ToList();

            foreach (var s in salesDto)
            {
                if (!validCarIds.Contains(s.CarId))
                {
                    continue;
                }

                var sale = new Sale
                {
                    Discount = s.Discount,
                    CarId = s.CarId,
                    CustomerId = s.CustomerId
                };

                sales.Add(sale);
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return String.Format(returnInputMessage, sales.Count);
        }

        //14
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Where(c => c.TravelledDistance > 2000000)
                .Select(c => new CarDistanceExportDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    Travelleddistance = c.TravelledDistance
                })
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ToList();

            var attr = new XmlRootAttribute("cars");
            var serializer = new XmlSerializer(typeof(List<CarDistanceExportDto>), attr);

            StringBuilder sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
                XmlQualifiedName.Empty
            });


            serializer.Serialize(new StringWriter(sb), cars, namespaces);

            return sb.ToString().TrimEnd();
        }

        //15
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Where(c => c.Make == "BMW")
                .Select(c => new CarMakeExportDto
                {
                    Id = c.Id,
                    Model = c.Model,
                    Travelleddistance = c.TravelledDistance
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.Travelleddistance)
                .ToArray();

            //If it is .ToList() gives 1MB more memory usage in judge and Memory Limit Exception In Judge

            var attr = new XmlRootAttribute("cars");

            //If it is List<CarMakeExportDto> gives 1MB more memory usage in judge and Memory Limit Exception In Judge
            var serializer = new XmlSerializer(typeof(CarMakeExportDto[]), attr);

            StringBuilder sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
                XmlQualifiedName.Empty
            });


            serializer.Serialize(new StringWriter(sb), cars, namespaces);

            return sb.ToString().TrimEnd();
        }
        //16
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context
                .Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new SupplierExportDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Partscount = s.Parts.Count
                })
                .ToArray();

            var attr = new XmlRootAttribute("suppliers");
            var serializer = new XmlSerializer(typeof(SupplierExportDto[]), attr);

            StringBuilder sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
                XmlQualifiedName.Empty
            });


            serializer.Serialize(new StringWriter(sb), suppliers, namespaces);

            return sb.ToString().TrimEnd();
        }

        //17
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Select(c => new CarDetailsExportDto
                {
                    Parts = c.PartCars
                        .Select(p => new PartExportDto
                        {
                            Name = p.Part.Name,
                            Price = p.Part.Price
                        })
                        .OrderByDescending(p => p.Price)
                        .ToArray(),
                    Make = c.Make,
                    Model = c.Model,
                    Travelleddistance = c.TravelledDistance
                })
                .OrderByDescending(c => c.Travelleddistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ToArray();

            var attr = new XmlRootAttribute("cars");
            var serializer = new XmlSerializer(typeof(CarDetailsExportDto[]), attr);

            StringBuilder sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
                XmlQualifiedName.Empty
            });


            serializer.Serialize(new StringWriter(sb), cars, namespaces);

            return sb.ToString().TrimEnd();
        }

        //18
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context
                .Customers
                .Where(c => c.Sales.Count > 0)
                .Select(c => new CustomerExportDto
                {
                    Fullname = c.Name,
                    Boughtcars = c.Sales.Count,
                    Spentmoney = c.Sales
                        .Sum(s => s.Car.PartCars
                            .Sum(pc => pc.Part.Price))
                })
                .OrderByDescending(c => c.Spentmoney)
                .ToArray();

            var attr = new XmlRootAttribute("customers");
            var serializer = new XmlSerializer(typeof(CustomerExportDto[]), attr);

            StringBuilder sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
                XmlQualifiedName.Empty
            });

            serializer.Serialize(new StringWriter(sb), customers, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context
                .Sales
                .Select(s => new SaleExportDto
                {
                    Car = new CarAttributesExportDto
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        Travelleddistance = s.Car.TravelledDistance
                    },
                    Customername = s.Customer.Name,
                    Discount = s.Discount,
                    Price = s.Car.PartCars.Sum(pc => pc.Part.Price),
                    Pricewithdiscount = s.Car.PartCars.Sum(pc => pc.Part.Price) - s.Car.PartCars.Sum(pc => pc.Part.Price) * s.Discount / 100m 
                    /* If it is * (s.Discount / 100m) the result differs with 1 digit and givers 0/100 in judge */
                })
                .ToArray();

            var attr = new XmlRootAttribute("sales");
            var serializer = new XmlSerializer(typeof(SaleExportDto[]), attr);

            StringBuilder sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
                XmlQualifiedName.Empty
            });

            serializer.Serialize(new StringWriter(sb), sales, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}

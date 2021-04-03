using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using CarDealer.DTO;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var db = new CarDealerContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

           // string suppliersJson = File.ReadAllText("../../../Datasets/suppliers.json");
            //var suppliersResult = ImportSuppliers(db, suppliersJson);

            //string partsJson = File.ReadAllText("../../../Datasets/parts.json");
            //var partsResult = ImportParts(db, partsJson);

            //string carsJson = File.ReadAllText("../../../Datasets/cars.json");
            //var carsResult = ImportCars(db, carsJson);
            //
           // string customersJson = File.ReadAllText("../../../Datasets/customers.json");
           // var customersResult = ImportCustomers(db, customersJson);

            //string salesJson = File.ReadAllText("../../../Datasets/sales.json");
           // var salesResult = ImportSales(db, salesJson);

            //File.WriteAllText("../../../../TestFolder/carsandparts.json", carsPartsJson);

            var jsonFile = GetSalesWithAppliedDiscount(db);
            Console.WriteLine(jsonFile);
        }
        //19

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var salesDiscountDto = context.Sales.Take(10)
                .Select(s => new SalesDiscountsDto
                {
                    car = new CarDto
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    customerName = s.Customer.Name,
                    Discount = s.Discount.ToString("F2"),
                    price = s.Car.PartCars.Sum(x => x.Part.Price).ToString("F2"),
                    priceWithDiscount = (s.Car.PartCars
                        .Sum(x => x.Part.Price) * (1.0M - s.Discount / 100)).ToString("F2")
                })
                .ToList();

            return JsonConvert.SerializeObject(salesDiscountDto, Formatting.Indented);
        }

        //18

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var salesDto = context.Customers
                .Where(s => s.Sales.Count > 0)
                .Select(s => new SalesExportDto
                {
                    fullName = s.Name,
                    boughtCars = s.Sales.Count,
                    spentMoney = s.Sales.Select(x => x.Car.PartCars.Sum(y => y.Part.Price)).Sum()
                })
                .OrderByDescending(se => se.spentMoney)
                .ThenByDescending(se => se.boughtCars)
                .ToList();

            return JsonConvert.SerializeObject(salesDto, Formatting.Indented);
        }

        //17

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsAndPartsDto = context.Cars
                .Select(c => new CarPartsExportDto
                {
                    car = new CarDto
                    {
                        Make = c.Make,
                        Model = c.Model,
                        TravelledDistance = c.TravelledDistance
                    },

                    parts = c.PartCars.Select(x => new PartDto
                    {
                        Name = x.Part.Name,
                        Price = x.Part.Price.ToString("F2")
                    }).ToList()
                })
                .ToList();

            return JsonConvert.SerializeObject(carsAndPartsDto, Formatting.Indented);
        }

        //16

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliersDto = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new SuppliersExportDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToList();

            return JsonConvert.SerializeObject(suppliersDto, Formatting.Indented);
        }

        //15

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var carsExport = context.Cars
                .Where(c => c.Make == "Toyota")
                .Select(c => new CarsExportDto
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .OrderBy(s => s.Model)
                .ThenByDescending(s => s.TravelledDistance)
                .ToList();

            return JsonConvert.SerializeObject(carsExport, Formatting.Indented);
        }

        //14

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customersDto = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .Select(c => new CustomersExportDto
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy"),
                    IsYoungDriver = c.IsYoungDriver
                }).ToList();

            return JsonConvert.SerializeObject(customersDto, Formatting.Indented);
        }

        //13

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var salesDto = JsonConvert.DeserializeObject<ICollection<SalesImportDto>>(inputJson);

            var sales = salesDto.Select(s => new Sale
            {
                CarId = s.carId,
                CustomerId = s.customerId,
                Discount = s.discount
            }).ToList();

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}.";
        }

        //12

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customersDto = JsonConvert.DeserializeObject<ICollection<CustomersImportDto>>(inputJson);

            var customers = customersDto.Select(c => new Customer
            {
                Name = c.name,
                BirthDate = c.birthDate,
                IsYoungDriver = c.isYoungDriver
            }).ToList();

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}.";
        }

        //11

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carsDto = JsonConvert.DeserializeObject<ICollection<CarsImportDto>>(inputJson);

            List<Car> cars = new List<Car>();
            foreach (var carDto in carsDto)
            {
                Car currentCar = new Car
                {
                    Make = carDto.make,
                    Model = carDto.model,
                    TravelledDistance = carDto.travelledDistance
                };

                foreach (var dtoPartsId in carDto.partsId.Distinct())
                {
                    currentCar.PartCars.Add(new PartCar
                    {
                        PartId = dtoPartsId
                    });
                }
                cars.Add(currentCar);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}.";
        }

        //10

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var mappConfig = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
            var mapper = mappConfig.CreateMapper();

            var partsDto = JsonConvert.DeserializeObject<ICollection<PartsImportDto>>(inputJson);
            var partsAll = mapper.Map<ICollection<Part>>(partsDto);

            var suppliersId = context.Suppliers.Select(s => s.Id).ToList();
            var parts = partsAll.Where(p => suppliersId.Contains(p.SupplierId)).ToList();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}.";
        }

        //09

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var mappConfig = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
            var mapper = mappConfig.CreateMapper();

            var suppliersDto = JsonConvert.DeserializeObject<ICollection<SuppliersImportDto>>(inputJson);
            var suppliers = mapper.Map<ICollection<Supplier>>(suppliersDto);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}.";
        }
    }
}
namespace PetStore
{
    using System;
    using Data;
    using Services.Implementations;

    public class Program
    {
        public static void Main()
        {
            using var data = new PetStoreDbContext();

            //var brandService = new BrandService(data);
            //var brandWithToys = brandService.FindByIdWithToys(1);
            //Console.WriteLine(brandWithToys.Name);


        }
    }
}

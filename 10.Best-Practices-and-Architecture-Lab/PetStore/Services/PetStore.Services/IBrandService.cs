using PetStore.Services.Models.Brand;
using System.Collections.Generic;

namespace PetStore.Services
{
    public interface IBrandService
    {
        int Create(string name);

        IEnumerable<BrandListingServiceModel> SearchByName(string name);

        BrandWithToysServiceModel FindByIdWithToys(int id);


    }
}

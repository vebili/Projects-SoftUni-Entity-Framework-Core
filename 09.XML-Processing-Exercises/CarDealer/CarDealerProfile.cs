using AutoMapper;

namespace CarDealer
{
    using Dtos.Export;
    using Dtos.Import;
    using Models;

    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<SupplierImportDto, Supplier>();

            CreateMap<Car, CarMakeExportDto>();

        }
    }
}

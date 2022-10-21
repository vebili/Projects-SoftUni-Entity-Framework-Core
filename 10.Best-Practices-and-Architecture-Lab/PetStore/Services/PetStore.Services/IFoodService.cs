using PetStore.Services.Models.Food;
using System;

namespace PetStore.Services
{
    public interface IFoodService
    {
        void BuyFromDistributor(string name, double weigh, decimal distributorPrice, decimal profit, DateTime expirationDate, int brandId, int categoryId);

        void BuyFromDistributor(AddingFoodServiceModel model);

        void SellFoodToUser(int foodId, int userId);

        bool Exists(int foodId);
    }
}

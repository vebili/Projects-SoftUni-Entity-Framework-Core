namespace PetStore.Services
{
    using System.Collections.Generic;
    using Models.Category;

    public interface ICategoryService
    {
        DetailsCategoryServiceModel GetById(int id);

        void Create(CreateCategoryServiceModel model);

        void Edit(EditCategoryServiceModel model);

        bool Remove(int id);

        bool Exists(int categoryId);

        IEnumerable<AllCategoriesServiceModel> All();
    }
}
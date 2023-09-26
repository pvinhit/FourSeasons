using be.Models;

namespace be.Services
{
    public interface ICategoryService
    {
        Category GetCategory(int categoryId);
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(int categoryId);
        IList<Category> GetAllCategories();
        IList<Category> GetEnableCategories();
    }
}

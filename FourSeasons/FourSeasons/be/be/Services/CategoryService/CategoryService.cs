using be.Models;
using be.Repositories.CategoryRepository;

namespace be.Services
{
    public class CategoryService : ICategoryService
    {
        public ICategoryRepository _categoryRepo;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepo = categoryRepository;
        }

        public void AddCategory(Category category)
        {
            _categoryRepo.AddCategory(category);
        }

        public void DeleteCategory(int categoryId)
        {
            _categoryRepo.DeleteCategory(categoryId);
        }

        public IList<Category> GetAllCategories()
        {
            return _categoryRepo.GetAllCategories();
        }

        public Category GetCategory(int categoryId)
        {
            return _categoryRepo.GetCategory(categoryId);
        }

        public void UpdateCategory(Category category)
        {
            _categoryRepo.UpdateCategory(category);
        }

        public IList<Category> GetEnableCategories()
        {
            return _categoryRepo.GetEnableCategories();
        }
    }
}

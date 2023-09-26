using be.Models;

namespace be.Repositories.CategoryRepository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DbFourSeasonHotelContext _context;

        public CategoryRepository()
        {
            _context = new DbFourSeasonHotelContext();
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void DeleteCategory(int categoryId)
        {
            var item = _context.Categories.Find(categoryId);
            _context.Categories.Remove(item);
            _context.SaveChanges();
        }

        public IList<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }
        public IList<Category> GetEnableCategories()
        {
            return _context.Categories.Where(c => c.Status == true).ToList();
        }

        public Category GetCategory(int categoryId)
        {
            return _context.Categories.Find(categoryId);
        }

        public void UpdateCategory(Category category)
        {
            Category updateCategory = _context.Categories.Find(category.CategoryId);
            updateCategory = category;
            _context.SaveChanges();
        }
    }
}

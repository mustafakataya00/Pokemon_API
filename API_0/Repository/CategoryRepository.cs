using API_0.Data;
using API_0.Interfaces;
using API_0.models;

namespace API_0.Repository
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly DataContext _context;
        public CategoryRepository(DataContext context)
        {
            _context = context;

        }

        public bool CategoryExists(int categoryid)
        {
            return _context.Categories.Any(c => c.Id == categoryid);    
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.OrderBy(c => c.Id).ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(c=>c.Id == id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCategory(int categoryid)
        {
            return _context.PokemonCategories.OrderBy(c => c.CategoryId ==categoryid).Select(c=>c.Pokemon).ToList();
        }
        public bool CreateCategory(Category category)
        {

            _context.Add(category);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }
    }
}

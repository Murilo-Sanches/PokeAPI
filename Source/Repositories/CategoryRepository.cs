using PokeAPI.DAOs;
using PokeAPI.Data;
using PokeAPI.Models;

namespace PokeAPI.Repositories
{
    public class CategoryRepository : ICategoryDAO
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public bool CategoryExists(int categoryId)
        {
            return _context.Categories.Any((category) => category.Id == categoryId);
        }

        public bool CreateCategory(Category category)
        {
            _context.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int categoryId)
        {
            return _context.Categories.Where((category) => category.Id == categoryId).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
        {
            return _context.PokemonCategories.Where((el) => el.CategoryId == categoryId)
                                             .Select((el) => el.Pokemon)
                                             .ToList();
        }

        public bool Save()
        {
            // query OK - 1 line
            int saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }
    }
}
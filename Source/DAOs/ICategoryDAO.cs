using PokeAPI.DAOs;
using PokeAPI.Models;

namespace PokeAPI.DAOs
{
    public interface ICategoryDAO
    {
        ICollection<Category> GetCategories();

        Category GetCategory(int categoryId);

        ICollection<Pokemon> GetPokemonByCategory(int categoryId);

        bool CategoryExists(int categoryId);

        bool CreateCategory(Category category);

        bool Save();

        bool UpdateCategory(Category category);

        bool DeleteCategory(Category category);
    }
}
using PokeAPI.DAOs;
using PokeAPI.Data;
using PokeAPI.DTOs;
using PokeAPI.Models;
using PokeAPI.Models.Joins;

namespace PokeAPI.Repositories
{
    public class PokemonRepository : IPokemonDAO
    {
        private readonly DataContext _context;

        public PokemonRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var pokemonOwnerEntity = _context.Owners.Where((owner) => owner.Id == ownerId).FirstOrDefault();

            var category = _context.Categories.Where((category) => category.Id == categoryId).FirstOrDefault();

            _context.Add(new PokemonOwner() { Owner = pokemonOwnerEntity, Pokemon = pokemon });

            _context.Add(new PokemonCategory() { Category = category, Pokemon = pokemon });

            _context.Add(pokemon);

            return Save();
        }

        public bool DeletePokemon(Pokemon pokemon)
        {
            _context.Remove(pokemon);
            return Save();
        }

        public Pokemon GetPokemon(int id)
        {
            return _context.Pokemon.Where((pokemon) => pokemon.Id == id).FirstOrDefault();
        }

        public Pokemon GetPokemon(string name)
        {
            return _context.Pokemon.Where((pokemon) => pokemon.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokeId)
        {
            var review = _context.Reviews.Where((review) => review.Pokemon.Id == pokeId);

            var reviewCount = review.Count();

            if (reviewCount <= 0) return 0;

            return ((decimal)review.Sum((rating) => rating.Rating / reviewCount));
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemon.OrderBy((pokemon) => pokemon.Id).ToList();
        }

        public Pokemon GetPokemonTrimToUpper(PokemonDTO pokemonCreate)
        {
            return GetPokemons().Where(c => c.Name.Trim().ToUpper() == pokemonCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
        }

        public bool PokemonExists(int pokeId)
        {
            return _context.Pokemon.Any((pokemon) => pokemon.Id == pokeId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdatePokemon(Pokemon pokemon)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            _context.Update(pokemon);
            return Save();
        }
    }
}
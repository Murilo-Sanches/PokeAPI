using PokeAPI.DTOs;
using PokeAPI.Models;

namespace PokeAPI.DAOs
{
    public interface IPokemonDAO
    {
        ICollection<Pokemon> GetPokemons();

        Pokemon GetPokemon(int id);

        Pokemon GetPokemon(string name);

        Pokemon GetPokemonTrimToUpper(PokemonDTO pokemonDTO);

        decimal GetPokemonRating(int pokeId);

        bool PokemonExists(int pokeId);

        bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);

        bool Save();

        bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon);

        bool DeletePokemon(Pokemon pokemon);
    }
}
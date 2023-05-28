using PokeAPI.Models;

namespace PokeAPI.DAOs
{
    public interface IOwnerDAO
    {
        ICollection<Owner> GetOwners();

        Owner GetOwner(int ownerId);

        ICollection<Owner> GetOwnerOfAPokemon(int pokeId);

        ICollection<Pokemon> GetPokemonByOnwer(int ownerId);

        bool OwnerExists(int ownerId);

        bool CreateOwner(Owner owner);

        bool Save();

        bool UpdateOwner(Owner owner);

        bool DeleteOwner(Owner owner);
    }
}
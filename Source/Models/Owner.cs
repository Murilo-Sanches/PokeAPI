using PokeAPI.Models.Joins;

namespace PokeAPI.Models
{
    public class Owner
    {
        public int Id { get; set; }
        // public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gym { get; set; }
        public Region Region { get; set; }
        public ICollection<PokemonOwner> PokemonOwners { get; set; }
    }
}
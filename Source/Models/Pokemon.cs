using PokeAPI.Models.Joins;

namespace PokeAPI.Models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CatchDate { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<PokemonOwner> PokemonOwners { get; set; }
        public ICollection<PokemonCategory> PokemonCategories { get; set; }

        public override string ToString()
        {
            return $"Pokemon - Name: {Name}, CatchDate: {CatchDate}, Reviews: {Reviews}";
        }
    }
}
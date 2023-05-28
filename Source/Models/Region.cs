namespace PokeAPI.Models
{
    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Owner> Owners { get; set; }

    }
}
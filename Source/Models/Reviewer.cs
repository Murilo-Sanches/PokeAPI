namespace PokeAPI.Models
{
    public class Reviewer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Review> Reviews { get; set; }

        public override string ToString()
        {
            return $"id = {Id}, FirstName = {FirstName}, LastName = {LastName}, Reviews = {Reviews.ToString()}";
        }
    }
}
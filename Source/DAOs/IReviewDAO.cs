using PokeAPI.Models;

namespace PokeAPI.DAOs
{
    public interface IReviewDAO
    {
        ICollection<Review> GetReviews();

        Review GetReview(int reviewId);

        ICollection<Review> GetReviewsOfAPokemon(int pokeId);

        bool ReviewExists(int reviewId);

        bool CreateReview(Review review);

        bool Save();

        bool UpdateReview(Review review);

        bool DeleteReview(Review review);

        bool DeleteReviews(List<Review> reviews);
    }
}
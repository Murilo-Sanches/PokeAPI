using PokeAPI.Models;

namespace PokeAPI.DAOs
{
    public interface IReviewerDAO
    {
        ICollection<Reviewer> GetReviewers();

        Reviewer GetReviewer(int reviewerId);

        ICollection<Review> GetReviewsByReviewer(int reviewerId);

        bool ReviewerExists(int reviewerId);

        bool CreateReviewer(Reviewer reviewer);

        bool Save();

        bool UpdateReviewer(Reviewer reviewer);

        bool DeleteReviewer(Reviewer reviewer);
    }
}
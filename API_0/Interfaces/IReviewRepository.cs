using API_0.models;

namespace API_0.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int reviewid);
        ICollection<Review> GetReviewOfAPokemon(int pokeid);
        bool ReviewExists(int reviewid);
        bool CreateReview( Review review );
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);
        bool DeleteReviews(List<Review> reviews);
        bool Save();

    }
}

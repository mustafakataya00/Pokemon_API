using API_0.Interfaces;
using API_0.models;
using API_0.Data;

namespace API_0.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private DataContext _context;

        public ReviewRepository(DataContext context)
        {
            _context = context;   
        }

        public bool CreateReview(Review review)
        {

            _context.Add(review);
            return Save();
          
        }

        public bool DeleteReview(Review review)
        {
            _context.Remove(review);
            return Save();
        }
        public bool DeleteReviews(List<Review> reviews)
        {
            _context.RemoveRange(reviews);
            return Save();
        }

        public Review GetReview(int reviewid)
        {
            return _context.Reviews.Where(r => r.Id == reviewid).FirstOrDefault();
        }

        public ICollection<Review> GetReviewOfAPokemon(int pokeid)
        {
            return _context.Reviews.Where(p => p.Pokemon.ID == pokeid).ToList();
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.OrderBy(r => r.Id).ToList();
        }

        public bool ReviewExists(int reviewid)
        {
           return _context.Reviews.Any(r => r.Id == reviewid);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReview(Review review)
        {
           _context.Update(review);
            return Save();
        }
    }
}

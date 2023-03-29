using API_0.Interfaces;
using API_0.models;
using API_0.Data;
using Microsoft.EntityFrameworkCore;

namespace API_0.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private DataContext _context;

        public ReviewerRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _context.Add(reviewer);
            return Save();
        }

        public Reviewer GetReviewer(int reviewerid)
        {
            return _context.Reviewers.Where(r => r.ID == reviewerid).Include(e => e.Reviews).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _context.Reviewers.OrderBy(r => r.ID).ToList();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerid)
        {
            return _context.Reviews.Where(r=>r.Reviewer.ID == reviewerid).ToList();
        }

        public bool ReviewerExists(int reviewerid)
        {
           return _context.Reviewers.Any(r => r.ID == reviewerid);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _context.Update(reviewer);
            return Save();
        }
        public bool DeleteReviewer(Reviewer reviewer)
        {
            _context.Remove(reviewer);
            return Save();
        }

    }
}

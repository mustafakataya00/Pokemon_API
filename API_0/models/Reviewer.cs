namespace API_0.models
{
    public class Reviewer
    {
        public int ID { get; set; }
        public string FirstName  { get; set; }
        public string LastName { get; set; }
        public ICollection<Review> Reviews { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace API_0.models
{
    public class Pokemon
    {
        public int ID { get; set; }
        
        public string Name { get; set; }
        
        public DateTime BirthDate { get; set; }

        public ICollection<Review> Reviews { get; set; }
        public ICollection<PokemonCategory> PokemonCategories { get; set; }
        public ICollection<PokemonOwner> PokemonOwners { get; set; }

    }

}


using API_0.Interfaces;
using API_0.models;
using API_0.Data;
using System.Transactions;

namespace API_0.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private DataContext _context;

        public OwnerRepository(DataContext context )
        {
            _context = context;
        }

        public bool CreateOwner(Owner owner)
        {
            _context.Add(owner);
            return Save();
        }

        public Owner GetOwner(int ownerId)
        {
            return _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerOfAPokemon(int pokeid)
        {
            return _context.PokemonOwners.Where(p => p.Pokemon.ID == pokeid).Select(o => o.Owner).ToList();
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.OrderBy(o => o.Id).ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerid)
        {
            return _context.PokemonOwners.Where(o => o.Owner.Id == ownerid).Select(p => p.Pokemon).ToList();    
        }

        public bool OwnerExists(int ownerid)
        {
            return _context.Owners.Any(o => o.Id == ownerid);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateOwner(Owner owner)
        {
            _context.Update(owner); 
            return Save();
        }
        public bool DeleteOwner(Owner owner) 
        {
            _context.Remove(owner);
            return Save();
        }
    }
}

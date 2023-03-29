using API_0.Interfaces;
using API_0.Data;
using API_0.models;
using AutoMapper;

namespace API_0.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private DataContext _context;
        private IMapper _mapper;

        public CountryRepository(DataContext context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            
        }

        public bool CountryExists(int countryid)
        {
           return _context.Countries.Any(c => c.Id == countryid);
        }

        public bool CreateCountry(Country country)
        {
            _context.Add(country);
            return Save();
        }

        public bool DeleteCountry(Country country)
        {
           _context.Remove(country); 
            return Save();
        }

        public ICollection<Country> GetCountries()
        {
            return _context.Countries.OrderBy(c => c.Id).ToList();
        }

        public Country GetCountry(int countryid)
        {
            return _context.Countries.Where(c => c.Id == countryid).FirstOrDefault();
        }

        public Country GetCountryByOwner(int ownerid)
        {
            return _context.Owners.Where(o => o.Id == ownerid).Select(c => c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersFromACountry(int countryid)
        {
           return _context.Owners.Where(c => c.Country.Id == countryid).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
            _context.Update(country);
            return Save();
        }
    }
}

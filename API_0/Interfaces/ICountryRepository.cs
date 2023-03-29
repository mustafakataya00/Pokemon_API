using API_0.models;
namespace API_0.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country GetCountry(int countryid);
        Country GetCountryByOwner(int ownerid);
        ICollection<Owner> GetOwnersFromACountry(int countryid);
        bool CountryExists(int countryid);
        bool CreateCountry(Country country);
        bool UpdateCountry(Country country);
        bool DeleteCountry(Country country);
        bool Save();



    }
}

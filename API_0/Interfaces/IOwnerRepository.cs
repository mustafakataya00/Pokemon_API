using API_0.models;
namespace API_0.Interfaces
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetOwners();
        Owner GetOwner(int ownerId);

        ICollection<Owner> GetOwnerOfAPokemon(int pokeid );
        ICollection<Pokemon> GetPokemonByOwner(int ownerid);
        bool OwnerExists(int ownerid);
        bool CreateOwner(Owner owner);
        bool UpdateOwner(Owner owner);
        bool DeleteOwner(Owner owner);
        bool Save();

    }
}

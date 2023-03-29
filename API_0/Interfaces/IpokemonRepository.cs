using API_0.models;
namespace API_0.Interfaces
{
    public interface IpokemonRepository
    {
        ICollection<Pokemon> GetPokemons();
        Pokemon GetPokemon(int id);
        Pokemon GetPokemon(string name);
        decimal GetPokemonRating(int pokeId);
        bool PokemonExists(int pokeid);
        bool CreatePokemon(Pokemon pokemon , int ownerid , int categoryid );
        bool UpdatePokemon(Pokemon pokemon, int ownerid, int categoryid);
        bool DeletePokemon(Pokemon pokemon);
        bool Save();

    }
}

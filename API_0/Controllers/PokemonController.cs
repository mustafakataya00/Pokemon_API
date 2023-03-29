using API_0.Interfaces;
using API_0.models;
using API_0.Repository;
using API_0.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API_0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private IpokemonRepository _pokemonRepository;
        private ICategoryRepository _categoryRepository;
        private IOwnerRepository _ownerRepository;
        private IReviewRepository _reviewRepository;

        private IMapper _mapper;

        public PokemonController(IpokemonRepository pokemonRepository, IOwnerRepository ownerRepository, ICategoryRepository categoryRepository,IReviewRepository reviewRepository ,IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _categoryRepository = categoryRepository;
            _ownerRepository = ownerRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            
        }
        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());

            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }

        [HttpGet("{pokeid}")]
        [ProducesResponseType(200 , Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeid) 
        {
            if (!_pokemonRepository.PokemonExists(pokeid))
            {
                return NotFound();
            }
            var pokemon  = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeid));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemon);
        }

        [HttpGet("{pokeid}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeid)
        {
            if(!_pokemonRepository.PokemonExists(pokeid))
            {
                return NotFound();
            }
            var rating = _pokemonRepository.GetPokemonRating(pokeid);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }
            return Ok(rating);
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromBody] PokemonDto PokemonCreate, [FromQuery] int ownerid , [FromQuery] int categoryid)
        {
            if (PokemonCreate == null)
            {
                return BadRequest(ModelState);
            }

            var pname = PokemonCreate.Name.TrimEnd().ToUpper();
            var pokemons = _pokemonRepository.GetPokemons().Where(o => o.Name.Trim().ToUpper() == pname).FirstOrDefault();

            if (pokemons != null)
            {
                ModelState.AddModelError("", "Pokemon Already Exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonmap = _mapper.Map<Pokemon>(PokemonCreate);


            if (!_pokemonRepository.CreatePokemon(pokemonmap,ownerid,categoryid))
            {
                ModelState.AddModelError("", "Error while Saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Created");


        }

        [HttpPut("{pokeid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokemon(int pokeid, [FromQuery]int ownerid , [FromQuery]int categoryid,[FromBody] PokemonDto UpdatedPokemon)
        {
            if (UpdatedPokemon == null)
            {
                return BadRequest(ModelState);
            }
            if (pokeid != UpdatedPokemon.ID)
            {
                return BadRequest(ModelState);
            }
            if (!_pokemonRepository.PokemonExists(pokeid))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pokemonMap = _mapper.Map<Pokemon>(UpdatedPokemon);
            if (!_pokemonRepository.UpdatePokemon(pokemonMap,ownerid,categoryid))
            {
                ModelState.AddModelError("", "Error Occured While Updating");
                return BadRequest(ModelState);
            }

            return Ok("Pokemon Updated");
        }
        
        [HttpDelete("{pokeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
            {
                return NotFound();
            }

            var reviewsToDelete = _reviewRepository.GetReviewOfAPokemon(pokeId);
            var pokemonToDelete = _pokemonRepository.GetPokemon(pokeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong when deleting reviews");
            }

            if (!_pokemonRepository.DeletePokemon(pokemonToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
            }

            return NoContent();
        }




    }

}

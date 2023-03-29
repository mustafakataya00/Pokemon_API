using API_0.Dto;
using API_0.Interfaces;
using API_0.models;
using API_0.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;


namespace API_0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private ICountryRepository _countryRepository;
        private IOwnerRepository _ownerRepository;
        private IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository, ICountryRepository countryRepository ,IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
            
        }

        [HttpGet("{ownerid}")]
        [ProducesResponseType(200 , Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int ownerid)
        {
            if(!_ownerRepository.OwnerExists(ownerid))
            {
                return NotFound();
            }
            var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(ownerid));

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(owner);

        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwners()
        {

            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(owners);

        }

        [HttpGet("{ownerid}/pokemon")]
        [ProducesResponseType(200, Type = typeof(Owner) )]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByOwner(int ownerid)
        {
            if (!_ownerRepository.OwnerExists(ownerid))
            { return NotFound(); }

            var owner = _mapper.Map<List<PokemonDto>>(
                 _ownerRepository.GetPokemonByOwner(ownerid));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owner);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromBody]OwnerDto OwnerCreate , [FromQuery] int countryid)
        {
            if(OwnerCreate == null)
            {
                return BadRequest(ModelState);
            }

            var lname = OwnerCreate.LastName.TrimEnd().ToUpper();
            var owner = _ownerRepository.GetOwners().Where(o => o.LastName.Trim().ToUpper() == lname).FirstOrDefault();

            if(owner != null)
            {
                ModelState.AddModelError("", "Owner Already Exists");
                return StatusCode(422 , ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ownermap = _mapper.Map<Owner>(OwnerCreate);
            ownermap.Country = _countryRepository.GetCountry(countryid);

            if(!_ownerRepository.CreateOwner(ownermap))
            {
                ModelState.AddModelError("", "Error while Saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Created");


        }
        [HttpPut("{ownerid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOwner(int ownerid, [FromBody] OwnerDto UpdatedOwner)
        {
            if (UpdatedOwner == null)
            {
                return BadRequest(ModelState);
            }
            if (ownerid != UpdatedOwner.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_countryRepository.CountryExists(ownerid))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ownerMap = _mapper.Map<Owner>(UpdatedOwner);
            if (!_ownerRepository.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Error Occured While Updating");
                return BadRequest(ModelState);
            }

            return Ok("Owner Updated");
        }

        [HttpDelete("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }

            var ownerToDelete = _ownerRepository.GetOwner(ownerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_ownerRepository.DeleteOwner(ownerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
            }

            return NoContent();
        }



    }
}

using API_0.Dto;
using API_0.Interfaces;
using API_0.models;
using API_0.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API_0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private ICountryRepository _countryRepository;
        private IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;

        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(countries);
        }

        [HttpGet("{countryid}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int countryid)
        {
            if (!_countryRepository.CountryExists(countryid))
            {
                return NotFound();
            }
            var country = _mapper.Map<CountryDto>
                (_countryRepository.GetCountry(countryid));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(country);
        }

        [HttpGet("owners/{ownerid}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryByOwner(int ownerid)
        {
            var country = _mapper.Map<CountryDto>
                (_countryRepository.GetCountryByOwner(ownerid));


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(country);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CountryDto CountryCreate)
        {
            if(CountryCreate == null)
            {
                return BadRequest();
            }
            var country = _countryRepository.GetCountries().Where(c => c.Name.Trim().ToUpper() == CountryCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();
            if(country!=null)
            {
                ModelState.AddModelError("", "Country already Exists");
                return StatusCode(422, ModelState);
            }

            var countryMap = _mapper.Map<Country>(CountryCreate);
            if(!_countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went Wrong While Saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Created!");
        }

        [HttpPut("{countryid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCountry(int countryid , [FromBody] CountryDto UpdatedCountry) 
        {
              if(UpdatedCountry == null)
            {
                return BadRequest(ModelState);
            }
              if(countryid != UpdatedCountry.Id)
            {
                return BadRequest(ModelState);
            }
              if(!_countryRepository.CountryExists(countryid))
            {
                return NotFound();
            }
              if(!ModelState.IsValid)
            {
               return BadRequest(ModelState); 
            }

            var countryMap = _mapper.Map<Country>(UpdatedCountry);
            if(!_countryRepository.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "Error Occured While Updating");
                return BadRequest(ModelState);
            }
            
            return Ok("Country Updated");
        }

        [HttpDelete("{countryid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult DeleteCountry(int countryid)
        {
            if (!_countryRepository.CountryExists(countryid))
            {
                return NotFound();
            }
            var countryToDelete = _countryRepository.GetCountry(countryid);
            
            if (!_countryRepository.DeleteCountry(countryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);

            }
            return NoContent();
        }


    }
}

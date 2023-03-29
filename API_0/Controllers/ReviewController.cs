using API_0.Interfaces;
using API_0.models;
using AutoMapper;
using API_0.Dto;
using Microsoft.AspNetCore.Mvc;
using API_0.Repository;

namespace API_0.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController :Controller
    {
        private IReviewRepository _reviewRepository;
        public IpokemonRepository _pokemonRepository;
        public IReviewerRepository _reviewerRepository;
        private IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository, IpokemonRepository pokemonRepository, IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _pokemonRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
            
        }

        [HttpGet("{reviewid}")]
        [ProducesResponseType(200 , Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int reviewid)
        {
            if(!_reviewRepository.ReviewExists(reviewid))
            {
                return NotFound();
            }
            var review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewid));

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(review);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviews()
        { 
            
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }

        [HttpGet("pokemon/{pokeid}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewOfAPokemon(int pokeid)
        {
            var review = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewOfAPokemon(pokeid));
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(review);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromBody] ReviewDto ReviewCreate, [FromQuery] int pokemonid , int reviewerid)
        {
            if (ReviewCreate == null)
            {
                return BadRequest(ModelState);
            }

            var title = ReviewCreate.Title.TrimEnd().ToUpper();
            var review = _reviewRepository.GetReviews().Where(r => r.Title.Trim().ToUpper() == title).FirstOrDefault();

            if (review != null)
            {
                ModelState.AddModelError("", "Review Already Exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewmap = _mapper.Map<Review>(ReviewCreate);
            reviewmap.Pokemon = _pokemonRepository.GetPokemons().Where(p => p.ID == pokemonid).FirstOrDefault();
            reviewmap.Reviewer = _reviewerRepository.GetReviewers().Where(r => r.ID == reviewerid).FirstOrDefault();

            if (!_reviewRepository.CreateReview(reviewmap))
            {
                ModelState.AddModelError("", "Error while Saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Created");


        }
        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto updatedReview)
        {
            if (updatedReview == null)
                return BadRequest(ModelState);

            if (reviewId != updatedReview.Id)
                return BadRequest(ModelState);

            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var reviewMap = _mapper.Map<Review>(updatedReview);

            if (!_reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong updating review");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{reviewid}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReview(int reviewid)
        {
            if (!_reviewRepository.ReviewExists(reviewid))
            {
                return NotFound();
            }

            var ReviewToDelete = _reviewRepository.GetReview
                (reviewid);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReview(ReviewToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting review");
            }

            return NoContent();
        }

        [HttpDelete("/DeleteReviewsByReviewer/{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReviewsByReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            var reviewsToDelete = _reviewerRepository.GetReviewsByReviewer(reviewerId).ToList();
            if (!ModelState.IsValid)
                return BadRequest();

            if (!_reviewRepository.DeleteReviews(reviewsToDelete))
            {
                ModelState.AddModelError("", "error deleting reviews");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


    }
}

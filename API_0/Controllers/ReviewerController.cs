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
    public class ReviewerController :Controller
    {
        private IReviewerRepository _reviewerRepository;
        private IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;

        }
        [HttpGet("{reviewerid}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int reviewerid)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerid))
                return NotFound();

            var reviewer = _mapper.Map<ReviewerDto>(_reviewerRepository.GetReviewer(reviewerid));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewer);
        }



        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewers()
        {

            var reviewers = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewers());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviewers);

        }
        [HttpGet("{reviewerid}/reviews")]
        public IActionResult GetReviewsByReviewer(int reviewerid)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerid))
            { return NotFound(); }

            var reviewer = _mapper.Map<List<ReviewDto>>(_reviewerRepository.GetReviewsByReviewer(reviewerid));

            if(!ModelState.IsValid)
            { return BadRequest(ModelState); }

            return Ok(reviewer);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReviewer([FromBody] ReviewerDto ReviewerCreate)
        {
            if (ReviewerCreate == null)
            {
                return BadRequest(ModelState);
            }

            var lname = ReviewerCreate.LastName.TrimEnd().ToUpper();
            var reviewer = _reviewerRepository.GetReviewers().Where(r => r.LastName.Trim().ToUpper() == lname).FirstOrDefault();

            if (reviewer != null)
            {
                ModelState.AddModelError("", "Reviewer Already Exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewermap = _mapper.Map<Reviewer>(ReviewerCreate);
           

            if (!_reviewerRepository.CreateReviewer(reviewermap))
            {
                ModelState.AddModelError("", "Error while Saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Created");


        }

        [HttpPut("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDto updatedReviewer)
        {
            if (updatedReviewer == null)
                return BadRequest(ModelState);

            if (reviewerId != updatedReviewer.ID)
                return BadRequest(ModelState);

            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var reviewerMap = _mapper.Map<Reviewer>(updatedReviewer);

            if (!_reviewerRepository.UpdateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }

            var reviewerToDelete = _reviewerRepository.GetReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewerRepository.DeleteReviewer(reviewerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting reviewer");
            }

            return NoContent();
        }
    }
}

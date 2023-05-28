using PokeAPI.DAOs;
using PokeAPI.DTOs;
using PokeAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace PokeAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviewerDAO _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerDAO reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Review>), 200)]
        public IActionResult GetReviewers()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var reviewers = _mapper.Map<List<ReviewerDTO>>(_reviewerRepository.GetReviewers());

            return Ok(reviewers);
        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(typeof(Reviewer), 200)]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int reviewerId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_reviewerRepository.ReviewerExists(reviewerId)) return NotFound();

            var reviewer = _mapper.Map<ReviewerDTO>(_reviewerRepository.GetReviewer(reviewerId));

            return Ok(reviewer);
        }

        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(typeof(Review), 200)]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsByAReviewer(int reviewerId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_reviewerRepository.ReviewerExists(reviewerId)) return NotFound();

            var reviews = _mapper.Map<List<ReviewDTO>>(_reviewerRepository.GetReviewsByReviewer(reviewerId));

            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult CreateReviewer([FromBody] ReviewerDTO reviewerDTO)
        {
            if (reviewerDTO == null) return BadRequest(ModelState);

            var reviewer = _reviewerRepository.GetReviewers().Where((reviewer) =>
            {
                return reviewer.LastName.Trim().ToUpper() == reviewerDTO.LastName.TrimEnd().ToUpper();
            }).FirstOrDefault();

            if (reviewer != null)
            {
                ModelState.AddModelError("", "Region already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var reviewerMap = _mapper.Map<Reviewer>(reviewerDTO);

            if (!_reviewerRepository.CreateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wront while creating");
            }

            return Ok("Successfully created");
        }

        [HttpPut("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDTO reviewerDTO)
        {
            if (reviewerDTO is null) return BadRequest(ModelState);

            if (reviewerId != reviewerDTO.Id) return BadRequest(ModelState);

            if (!_reviewerRepository.ReviewerExists(reviewerId)) return NotFound();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var reviewerMap = _mapper.Map<Reviewer>(reviewerDTO);

            if (!_reviewerRepository.UpdateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wront while updating");
            }

            return NoContent();
        }

        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public IActionResult DeleteCategory(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId)) return NotFound();

            var reviewerToDelete = _reviewerRepository.GetReviewer(reviewerId);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_reviewerRepository.DeleteReviewer(reviewerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
            }

            return NoContent();
        }
    }
}
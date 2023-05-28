using PokeAPI.DAOs;
using PokeAPI.DTOs;
using PokeAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace PokeAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewDAO _reviewRepository;
        private readonly IPokemonDAO _pokemonRepository;
        private readonly IReviewerDAO _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewDAO reviewRepository, IPokemonDAO pokemonRepository, IReviewerDAO reviewerRepository, IMapper mapper)
        {

            _reviewRepository = reviewRepository;
            _pokemonRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Review>), 200)]
        public IActionResult GetReviews()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var reviews = _mapper.Map<List<ReviewDTO>>(_reviewRepository.GetReviews());

            return Ok(reviews);
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(typeof(Review), 200)]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int reviewId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_reviewRepository.ReviewExists(reviewId)) return NotFound();

            var review = _mapper.Map<ReviewDTO>(_reviewRepository.GetReview(reviewId));

            return Ok(review);
        }

        [HttpGet("/pokemon/{pokeId}")]
        [ProducesResponseType(typeof(Review), 200)]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsForAPokemon(int pokeId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var reviews = _mapper.Map<List<ReviewDTO>>(_reviewRepository.GetReviewsOfAPokemon(pokeId));

            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult CreateReview([FromQuery] int reviewerId, [FromQuery] int pokemonId, [FromBody] ReviewDTO reviewDTO)
        {
            if (reviewDTO == null) return BadRequest(ModelState);

            var review = _reviewRepository.GetReviews().Where((review) =>
            {
                return review.Title.Trim().ToUpper() == reviewDTO.Title.TrimEnd().ToUpper();
            }).FirstOrDefault();

            if (review != null)
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(reviewDTO);

            reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokemonId);
            reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);

            if (!_reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wront while creating");
            }

            return Ok("Successfully created");
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDTO reviewDTO)
        {
            if (reviewDTO is null) return BadRequest(ModelState);

            if (reviewId != reviewDTO.Id) return BadRequest(ModelState);

            if (!_reviewRepository.ReviewExists(reviewId)) return NotFound();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(reviewDTO);

            if (!_reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wront while updating");
            }

            return NoContent();
        }

        [HttpDelete("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public IActionResult DeleteReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId)) return NotFound();

            var reviewToDelete = _reviewRepository.GetReview(reviewId);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
            }

            return NoContent();
        }
    }
}
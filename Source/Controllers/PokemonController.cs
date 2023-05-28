using PokeAPI.DAOs;
using PokeAPI.DTOs;
using PokeAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace PokeAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonDAO _pokemonRepository;
        private readonly IOwnerDAO _ownerRepository;
        private readonly IReviewDAO _reviewRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonDAO pokemonRepository, IOwnerDAO ownerRepository, IReviewDAO reviewRepository, IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _ownerRepository = ownerRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Pokemon>), 200)]
        public IActionResult GetPokemons()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var pokemons = _mapper.Map<List<PokemonDTO>>(_pokemonRepository.GetPokemons());

            return Ok(pokemons);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(typeof(Pokemon), 200)]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_pokemonRepository.PokemonExists(pokeId)) return NotFound();

            var pokemon = _mapper.Map<PokemonDTO>(_pokemonRepository.GetPokemon(pokeId));

            return Ok(pokemon);
        }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(typeof(decimal), 200)]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_pokemonRepository.PokemonExists(pokeId)) return NotFound();

            return Ok(_pokemonRepository.GetPokemonRating(pokeId));
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDTO pokemonCreate)
        {
            if (pokemonCreate == null)
                return BadRequest(ModelState);

            var pokemons = _pokemonRepository.GetPokemonTrimToUpper(pokemonCreate);

            if (pokemons != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);


            if (!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{pokeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public IActionResult UpdatePokemon(
            int pokeId,
            [FromQuery] int ownerId,
            [FromQuery] int categoryId,
            [FromBody] PokemonDTO pokemonDTO)
        {
            if (pokemonDTO is null) return BadRequest(ModelState);

            if (pokeId != pokemonDTO.Id) return BadRequest(ModelState);

            if (!_pokemonRepository.PokemonExists(pokeId)) return NotFound();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var pokemonMap = _mapper.Map<Pokemon>(pokemonDTO);

            if (!_pokemonRepository.UpdatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wront while updating");
            }

            return NoContent();
        }

        [HttpDelete("{pokemonId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public IActionResult DeleteCategory(int pokemonId)
        {
            if (!_pokemonRepository.PokemonExists(pokemonId)) return NotFound();

            var reviewsToDelete = _reviewRepository.GetReviewsOfAPokemon(pokemonId);
            var pokemonToDelete = _pokemonRepository.GetPokemon(pokemonId);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong when deleting reviews");
            };

            if (!_pokemonRepository.DeletePokemon(pokemonToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
            }

            return NoContent();
        }
    }
}
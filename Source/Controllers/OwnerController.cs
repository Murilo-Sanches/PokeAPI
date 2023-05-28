using PokeAPI.DAOs;
using PokeAPI.DTOs;
using PokeAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace PokeAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerDAO _ownerRepository;
        private readonly IRegionDAO _regionRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerDAO ownerRepository, IRegionDAO regionRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Owner>), 200)]
        public IActionResult GetOwners()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var owners = _mapper.Map<List<OwnerDTO>>(_ownerRepository.GetOwners());

            return Ok(owners);
        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(typeof(Owner), 200)]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int ownerId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_ownerRepository.OwnerExists(ownerId)) return NotFound();

            var owner = _mapper.Map<OwnerDTO>(_ownerRepository.GetOwner(ownerId));

            return Ok(owner);
        }

        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(typeof(Owner), 200)]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByOwner(int ownerId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_ownerRepository.OwnerExists(ownerId)) return NotFound();

            var pokemon = _mapper.Map<List<PokemonDTO>>(_ownerRepository.GetPokemonByOnwer(ownerId));

            return Ok(pokemon);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult CreateOwner([FromQuery] int regionId, [FromBody] OwnerDTO ownerDTO)
        {
            if (ownerDTO == null) return BadRequest(ModelState);

            var owner = _ownerRepository.GetOwners().Where((owner) =>
            {
                return owner.LastName.Trim().ToUpper() == ownerDTO.LastName.TrimEnd().ToUpper();
            }).FirstOrDefault();

            if (owner != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ownerMap = _mapper.Map<Owner>(ownerDTO);

            ownerMap.Region = _regionRepository.GetRegion(regionId);

            if (!_ownerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wront while creating");
            }

            return Ok("Successfully created");
        }

        [HttpPut("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDTO ownerDTO)
        {
            if (ownerDTO is null) return BadRequest(ModelState);

            if (ownerId != ownerDTO.Id) return BadRequest(ModelState);

            if (!_ownerRepository.OwnerExists(ownerId)) return NotFound();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ownerMap = _mapper.Map<Owner>(ownerDTO);

            if (!_ownerRepository.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wront while updating");
            }

            return NoContent();
        }

        [HttpDelete("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public IActionResult DeleteCategory(int ownerId)
        {
            if (!_ownerRepository.OwnerExists(ownerId)) return NotFound();

            var ownerToDelete = _ownerRepository.GetOwner(ownerId);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_ownerRepository.DeleteOwner(ownerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
            }

            return NoContent();
        }
    }
}
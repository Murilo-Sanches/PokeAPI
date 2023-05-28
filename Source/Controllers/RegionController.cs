using PokeAPI.DAOs;
using PokeAPI.DTOs;
using PokeAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace PokeAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class RegionController : Controller
    {
        private readonly IRegionDAO _regionRepository;
        private readonly IMapper _mapper;

        public RegionController(IRegionDAO regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Region>), 200)]
        public IActionResult GetRegions()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var regions = _mapper.Map<List<RegionDTO>>(_regionRepository.GetRegions());

            return Ok(regions);
        }

        [HttpGet("{regionId}")]
        [ProducesResponseType(typeof(Region), 200)]
        [ProducesResponseType(400)]
        public IActionResult GetRegion(int regionId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_regionRepository.RegionExists(regionId)) return NotFound();

            var region = _mapper.Map<RegionDTO>(_regionRepository.GetRegion(regionId));

            return Ok(region);
        }

        [HttpGet("/owners/{ownerId}")]
        [ProducesResponseType(typeof(Region), 200)]
        [ProducesResponseType(400)]
        public IActionResult GetRegionOfAnOwner(int ownerId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var region = _mapper.Map<RegionDTO>(_regionRepository.GetRegionByOwner(ownerId));

            return Ok(region);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult CreateRegion([FromBody] RegionDTO RegionDTO)
        {
            if (RegionDTO == null) return BadRequest(ModelState);

            var region = _regionRepository.GetRegions().Where((region) =>
            {
                return region.Name.Trim().ToUpper() == RegionDTO.Name.TrimEnd().ToUpper();
            }).FirstOrDefault();

            if (region != null)
            {
                ModelState.AddModelError("", "Region already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var regionMap = _mapper.Map<Region>(RegionDTO);

            if (!_regionRepository.CreateRegion(regionMap))
            {
                ModelState.AddModelError("", "Something went wront while creating");
            }

            return Ok("Successfully created");
        }

        [HttpPut("{regionId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public IActionResult UpdateRegion(int regionId, [FromBody] RegionDTO RegionDTO)
        {
            if (RegionDTO is null) return BadRequest(ModelState);

            if (regionId != RegionDTO.Id) return BadRequest(ModelState);

            if (!_regionRepository.RegionExists(regionId)) return NotFound();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var regionMap = _mapper.Map<Region>(RegionDTO);

            if (!_regionRepository.UpdateRegion(regionMap))
            {
                ModelState.AddModelError("", "Something went wront while updating");
            }

            return NoContent();
        }

        [HttpDelete("{regionId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public IActionResult DeleteRegion(int regionId)
        {
            if (!_regionRepository.RegionExists(regionId)) return NotFound();

            var regionToDelete = _regionRepository.GetRegion(regionId);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_regionRepository.DeleteRegion(regionToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
            }

            return NoContent();
        }
    }
}
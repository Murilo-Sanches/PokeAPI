using PokeAPI.DAOs;
using PokeAPI.DTOs;
using PokeAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace PokeAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryDAO _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryDAO categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Category>), 200)]
        public IActionResult GetCategories()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var categories = _mapper.Map<List<CategoryDTO>>(_categoryRepository.GetCategories());

            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(typeof(Category), 200)]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int categoryId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_categoryRepository.CategoryExists(categoryId)) return NotFound();

            var category = _mapper.Map<CategoryDTO>(_categoryRepository.GetCategory(categoryId));

            return Ok(category);
        }

        [HttpGet("/pokemon/categoryId")]
        [ProducesResponseType(typeof(IEnumerable<Pokemon>), 200)]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByCategoryId(int categoryId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var pokemons = _mapper.Map<List<PokemonDTO>>(_categoryRepository.GetPokemonByCategory(categoryId));

            return Ok(pokemons);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDTO categoryDTO)
        {
            if (categoryDTO == null) return BadRequest();

            var category = _categoryRepository.GetCategories().Where((category) =>
            {
                return category.Name.Trim().ToUpper() == categoryDTO.Name.TrimEnd().ToUpper();
            }).FirstOrDefault();

            if (category != null)
            {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(categoryDTO);

            if (!_categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while creating");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDTO categoryDTO)
        {
            if (categoryDTO is null) return BadRequest(ModelState);

            if (categoryId != categoryDTO.Id) return BadRequest(ModelState);

            if (!_categoryRepository.CategoryExists(categoryId)) return NotFound();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(categoryDTO);

            if (!_categoryRepository.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
            }

            return NoContent();
        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId)) return NotFound();

            var categoryToDelete = _categoryRepository.GetCategory(categoryId);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_categoryRepository.DeleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
            }

            return NoContent();
        }
    }
}
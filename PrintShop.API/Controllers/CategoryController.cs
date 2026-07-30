using Microsoft.AspNetCore.Mvc;
using PrintShop.Application.Dtos;
using PrintShop.Application.Interfaces;
using PrintShop.Domain.Models;

namespace PrintShop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryResponse>>> GetAll()
        {
            var result = await _categoryService.GetAllAsync();

            if (result.Error != null)
                return StatusCode(500, result.Error);

            return Ok(result.Categories);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CategoryRequest request)
        {
            var result = await _categoryService.CreateAsync(request.Name);

            if (result.Error != null)
                return BadRequest(result.Error);

            // Возвращаем 201 Created с ссылкой на созданный ресурс (хорошая практика REST)
            return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result.Id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] CategoryRequest request)
        {
            var result = await _categoryService.UpdateAsync(id, request.Name);

            if (result.Error != null)
                return BadRequest(result.Error);

            return Ok(result.Id);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _categoryService.DeleteAsync(id);

            if (result.Error != null)
                return BadRequest(result.Error);

            return NoContent(); // 204 No Content - стандартный ответ для успешного удаления
        }

    }
}

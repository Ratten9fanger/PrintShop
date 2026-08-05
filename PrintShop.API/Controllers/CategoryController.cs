using Microsoft.AspNetCore.Mvc;
using PrintShop.Application.Dtos;
using PrintShop.Application.Interfaces.Services;
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
            var result = await _categoryService.GetAll();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CategoryRequest request)
        {
            var result = await _categoryService.Create(request.Name);

            if (result.Error != null)
                return BadRequest(result.Error);

            return Ok(result.Id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] CategoryRequest request)
        {
            var result = await _categoryService.Update(id, request.Name);

            if (result.Error != null)
                return BadRequest(result.Error);

            return Ok(result.Id);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _categoryService.Delete(id);

            if (result.Error != null)
                return BadRequest(result.Error);

            return Ok(result.Id);
        }

    }
}

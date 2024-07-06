using AutoMapper;
using BusinessObject.Entities;
using DataTransfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Service.Interfaces;
using System.Threading.Tasks;

namespace MomMomMilks.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class CategoryController : ODataController
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [EnableQuery]
        [HttpGet("{key}")]
        public async Task<IActionResult> Get([FromODataUri] int key)
        {
            var category = await _categoryService.GetCategoryByIdAsync(key);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Post([FromBody] CategoryDTO create)
        {
            var category = _mapper.Map<Category>(create);
            await _categoryService.AddCategoryAsync(category);
            return Ok(category);
        }

        [HttpPut("{key}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] CategoryDTO update)
        {
            var category = _mapper.Map<Category>(update);
            await _categoryService.UpdateCategoryAsync(category);

            return Updated(update);
        }


        [HttpDelete("{key}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var existingCategory = await _categoryService.GetCategoryByIdAsync(key);
            if (existingCategory == null)
            {
                return NotFound();
            }

            await _categoryService.DeleteCategoryAsync(key);
            return NoContent();
        }
    }
}

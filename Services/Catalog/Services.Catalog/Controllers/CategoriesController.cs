using Education.Shared.ControllerBases;
using Microsoft.AspNetCore.Mvc;
using Services.Catalog.Dtos;
using Services.Catalog.Services;
using System.Threading.Tasks;

namespace Services.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : CustomBaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        //[HttpGet("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();

            return CreateActionResultInstance(categories);
        }

        //categories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            return CreateActionResultInstance(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto categoryDto)
        {
            var response = await _categoryService.CreateAsync(categoryDto);

            return CreateActionResultInstance(response);
        }
    }
}

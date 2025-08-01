using Blog_API.Data;
using Blog_API.Models.Domain;
using Blog_API.Models.DTO;
using Blog_API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        [HttpPost("Add",Name ="AddCategory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> AddCategory([FromBody]CreateCategoryRequestDto request)
        {
            Category category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle,
            };
           await categoryRepository.CreateAsync(category);
            var response = new CategoryDto
            {
                Id=category.Id,
                Name = category.Name,
                UrlHandle = request.UrlHandle,
            };
            return CreatedAtRoute(response, new {Id=response.Id});
        }
        [HttpGet("All",Name = "GetAllCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCategories()
        {
            var Categories=await categoryRepository.GetAllAsync();
            List<CategoryDto> response = new List<CategoryDto>();
            foreach (var category in Categories)
            {
                response.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle,
                });
            }
            return Ok(response);
        }

        [HttpGet("GetByID/{ID}",Name = "GetCategoryByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryByID([FromRoute]Guid ID)
        {
            Category category=await categoryRepository.GetByID(ID);
            if (category == null)
                return NotFound("Category Not Found!!");
            CategoryDto categoryDto = new CategoryDto
            {
                Id=category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };
            return Ok(categoryDto);
        }
        [HttpPut("{ID:Guid}", Name = "EditCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditCategory([FromRoute]Guid ID,UpdateCategoryRequestDto updateCategoryRequestDto)
        {
            Category category = new Category
            {
                Id=ID,
                Name = updateCategoryRequestDto.Name,
                UrlHandle = updateCategoryRequestDto.UrlHandle,
            };
            category=await categoryRepository.UpdateAsync(category);
            if (category == null)
                return NotFound();
            CategoryDto response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);
        }
        [HttpDelete]
        [Route("{ID:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory([FromRoute]Guid ID)
        {
            var category=await categoryRepository.DeleteAsync(ID);
            if (category is null)
                return NotFound();
            CategoryDto response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);

        }
    }
}

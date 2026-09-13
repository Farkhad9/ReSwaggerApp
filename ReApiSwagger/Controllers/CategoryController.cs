using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReApiSwagger.Data;
using ReApiSwagger.Dtos.Categories;
using ReApiSwagger.Extentions;
using ReApiSwagger.Models;

namespace ReApiSwagger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(AppDbContext context, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = mapper.Map<List<CategoryReturnDto>>(context.Categories.ToList());
            return Ok(categories);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryCreateDto categorydto) 
        { 
            if (categorydto.Photo == null)
            {
                return BadRequest("Photo is required");
            }
            string filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CategoryImages");
            string savefilename = await FileManager.SaveFileAsync(categorydto.Photo, filepath);

            var newCategory = mapper.Map<Category>(categorydto);
            newCategory.ImageUrl = savefilename;
            await context.Categories.AddAsync(newCategory);
            await context.SaveChangesAsync();
            var returndto = mapper.Map<CategoryReturnDto>(newCategory);
            return Ok(returndto);
        } 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            var categoryDto = mapper.Map<CategoryReturnDto>(category);
            return Ok(categoryDto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory( int id, [FromForm] CategoryUpdateDto categoryUpdateDto)
        {
            var category = context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            mapper.Map(categoryUpdateDto, category);
            if (categoryUpdateDto.Photo != null)
            {
                string filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CategoryImages");
                string savefilename = await FileManager.SaveFileAsync(categoryUpdateDto.Photo, filepath);
                category.ImageUrl = savefilename;
            }
           
            await context.SaveChangesAsync();
            var returnDto = mapper.Map<CategoryReturnDto>(category);
            return Ok(returnDto);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            context.Categories.Remove(category);
            context.SaveChanges();
            return Ok("category deleted successfully");
        }
    }
}

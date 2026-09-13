using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ReApiSwagger.Data;
using ReApiSwagger.Dtos.Products;
using ReApiSwagger.Models;

namespace ReApiSwagger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(AppDbContext context, IMapper mapper) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductCreateDto productCreateDto)
        {
            var iscategoryExists = await context.Categories.AnyAsync(c => c.Id == productCreateDto.CategoryId);
            if (!iscategoryExists) 
            {
                return BadRequest("Category does not exist");
            }
            var newProduct = mapper.Map<Product>(productCreateDto);
            newProduct.CategoryId = productCreateDto.CategoryId;
            await context.Products.AddAsync(newProduct);
            await context.SaveChangesAsync();
            await context.Entry(newProduct).Reference(p => p.Category).LoadAsync();
            var productReturnDto = mapper.Map<ProductReturnDto>(newProduct);
            return Ok(productReturnDto);
        }
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await context.Products
                .Include(p => p.Category)
                .ToListAsync();
            var returnproduct = mapper.Map<List<ProductReturnDto>>(products);
            return Ok(returnproduct);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            var returnproduct = mapper.Map<ProductReturnDto>(product);
            return Ok(returnproduct);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductUpdateDto productUpdateDto)
        {
            var product = await context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            var isCategoryExist = await context.Categories.AnyAsync(c => c.Id == productUpdateDto.category.Id);
            if (!isCategoryExist)
            {
                return BadRequest("Category does not exist.");
            }
            mapper.Map(productUpdateDto, product);
            product.CategoryId = productUpdateDto.category.Id;
            await context.SaveChangesAsync();
            await context.Entry(product).Reference(p => p.Category).LoadAsync();
            var returnproduct = mapper.Map<ProductReturnDto>(product);
            return Ok(returnproduct);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            context.Products.Remove(product);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}

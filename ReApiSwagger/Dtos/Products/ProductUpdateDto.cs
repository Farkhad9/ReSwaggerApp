using ReApiSwagger.Models;

namespace ReApiSwagger.Dtos.Products
{
    public class ProductUpdateDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public CategoryInProductUpdateDto category { get; set; } = null!;
    }
    public class CategoryInProductUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}

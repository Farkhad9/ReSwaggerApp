using ReApiSwagger.Models;

namespace ReApiSwagger.Dtos.Products
{
    public class ProductReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public CategoryInProductReturnDto Category { get; set; } = null!;
    }
    public class CategoryInProductReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}

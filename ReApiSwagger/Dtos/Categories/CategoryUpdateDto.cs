namespace ReApiSwagger.Dtos.Categories
{
    public class CategoryUpdateDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public IFormFile Photo { get; set; } = null!;
    }
}

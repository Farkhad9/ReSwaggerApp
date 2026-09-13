using System.ComponentModel.DataAnnotations;

namespace ReApiSwagger.Attributes
{
    public class FileTypeAttribute: ValidationAttribute
    {
        public string[] AllowedTypes { get; set; }
        public FileTypeAttribute(string[] allowedTypes)
        {
            AllowedTypes = allowedTypes;
        }
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (!AllowedTypes.Contains(fileExtension))
                {
                    return new ValidationResult($"File type should be one of the following: {string.Join(", ", AllowedTypes)}.");
                }
            }
            return ValidationResult.Success;
        }
    }
}

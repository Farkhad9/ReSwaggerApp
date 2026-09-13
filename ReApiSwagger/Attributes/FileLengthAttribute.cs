using Microsoft.Extensions.Validation;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace ReApiSwagger.Attributes
{
    public class FileLengthAttribute : ValidationAttribute
    {
        public int Length { get; set; }
        public FileLengthAttribute(int length)
        {
            Length = length;
        }
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        { 
        if (value is IFormFile Photo)
                if (Photo.Length > Length * 1024 * 1024)
                {
                    return new ValidationResult($"File size should not exceed {Length} mb.");
                }
            return ValidationResult.Success;
        }
    }
}

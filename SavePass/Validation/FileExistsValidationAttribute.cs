using System.ComponentModel.DataAnnotations;
using System.IO;

namespace SavePass.Validation
{
    public class FileExistsValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            => File.Exists((string) value) ? ValidationResult.Success : new ValidationResult(ErrorMessage);
    }
}

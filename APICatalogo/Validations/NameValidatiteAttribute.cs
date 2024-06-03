using System.ComponentModel.DataAnnotations;

namespace APICatalogo.Validations
{
    public class NameValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value,
            ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var name = value.ToString()[0].ToString();
            if (name != name.ToUpper())
            {
                return new ValidationResult("First letter must be an upper.");
            }

            return ValidationResult.Success;
        }
    }
}
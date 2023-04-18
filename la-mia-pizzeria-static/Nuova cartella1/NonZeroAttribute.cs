using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Nuova_cartella1
{
    public class NonZeroAttribute : ValidationAttribute
    {
       protected override ValidationResult IsValid(object? value, ValidationContext _)
        {
            var input = value as int?;

            if (input is null or 0)
            {
                return new ValidationResult(ErrorMessage ?? $"Value cannot be zero.");
            }

            return ValidationResult.Success!;
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Validations
{
    public class CapitalizeFirstLetterAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return  ValidationResult.Success;
            }

            var firstLetter = value.ToString()?[0].ToString();
            return firstLetter != firstLetter?.ToUpper() ? new ValidationResult("La Primera Letra Debe Ser Mayúscula") : ValidationResult.Success;
        }
    }    
}


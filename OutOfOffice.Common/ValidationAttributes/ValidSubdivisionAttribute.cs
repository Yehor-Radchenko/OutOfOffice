using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.Common.ValidationAttributes
{
    public class ValidSubdivisionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var subdivision = value as string;

            if (string.IsNullOrEmpty(subdivision) || !StaticData.Subdivisions.Contains(subdivision))
            {
                return new ValidationResult($"The Subdivision '{subdivision}' is not valid.");
            }

            return ValidationResult.Success;
        }
    }
}

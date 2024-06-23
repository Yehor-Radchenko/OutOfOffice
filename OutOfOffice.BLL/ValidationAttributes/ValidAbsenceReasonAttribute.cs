using OutOfOffice.Common.StaticData;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.BLL.ValidationAttributes
{
    internal class ValidAbsenceReasonAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var absenceReason = value as string;

            if (string.IsNullOrEmpty(absenceReason) || !StaticData.AbsenceReasons.Contains(absenceReason))
            {
                return new ValidationResult($"The Subdivision '{absenceReason}' is not valid.");
            }

            return ValidationResult.Success;
        }
    }
}

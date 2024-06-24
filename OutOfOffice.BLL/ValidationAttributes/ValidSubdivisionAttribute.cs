using OutOfOffice.Common.StaticData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOffice.BLL.ValidationAttributes
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

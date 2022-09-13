using LMS.Core.Entities;
using LMS.Core.Services;
using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Validations
{
    public class ValidateModuleStartDate : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime input)
            {
                var validationService = (IDateValidationService)validationContext
                         .GetService(typeof(IDateValidationService))!;

                var module = validationContext.ObjectInstance as Module;

                if (module is not null)
                {
                    var result = validationService.ValidateModuleStartDate(input, module.CourseId).Result;

                    if (result == "true")
                        return ValidationResult.Success;
                    else
                        return new ValidationResult(result);
                }
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}

using LMS.Core.Entities;
using LMS.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace LMS.Web.Validations
{
    public class ValidateModuleEndDate : ValidationAttribute
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
                    var result = validationService.ValidateModuleEndDate(input, module.StartDate, module.CourseId).Result;

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

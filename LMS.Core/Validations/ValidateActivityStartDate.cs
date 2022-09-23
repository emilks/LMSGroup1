using LMS.Core.Entities;
using LMS.Core.Services;
using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Validations
{
    public class ValidateActivityStartDate : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime input)
            {
                var validationService = (IDateValidationService)validationContext
                         .GetService(typeof(IDateValidationService))!;

                var activity = validationContext.ObjectInstance as Activities;

                if (activity is not null)
                {
                    var result = validationService.ValidateActivityStartDate(input, activity.ModuleId).Result;

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

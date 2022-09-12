using LMS.Core.Entities;
using LMS.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace LMS.Web.Validations
{
    public class ValidateActivityEndDate : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime input)
            {
                var validationService = (IDateValidationService)validationContext
                         .GetService(typeof(IDateValidationService))!;

                var activity = validationContext.ObjectInstance as Activity;

                if (activity is not null)
                {
                    var result = validationService.ValidateActivityEndDate(input, activity.StartDate, activity.ModuleId).Result;

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

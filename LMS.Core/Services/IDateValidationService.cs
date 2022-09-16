namespace LMS.Core.Services
{
    public interface IDateValidationService
    {
        Task<string> ValidateModuleStartDate(DateTime startDate, int courseId);
        Task<string> ValidateModuleEndDate(DateTime endDate, DateTime startDate, int courseId);
        Task<string> ValidateModuleDuration(int duration, DateTime startDate, int courseId);
        Task<string> ValidateActivityStartDate(DateTime startDate, int moduleId);
        Task<string> ValidateActivityEndDate(DateTime endDate, DateTime startDate, int moduleId);
    }
}

using LMS.Core.Repositories;
using LMS.Core.Services;
using LMS.Data.Data;

#nullable disable

namespace LMS.Web.Services
{
    public class DateSuggestionService : IDateSuggestionService
    {
        private readonly ApplicationDbContext _context;

        public DateSuggestionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DateTime> GetSuggestedModuleStartDate(int courseId)
        {
            var course = await _context.Course.FindAsync(courseId);
            var lastModule = course.Modules.OrderBy(m => m.EndDate).LastOrDefault();

            if (lastModule is null)
                return DateTime.Now;

            return lastModule.EndDate.AddDays(1);
        }
    }
}

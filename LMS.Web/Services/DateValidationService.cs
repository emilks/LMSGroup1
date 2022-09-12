using LMS.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Web.Services
{
    public class DateValidationService : IDateValidationService
    {

        private readonly ApplicationDbContext _context;

        public DateValidationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> ValidateModuleStartDate(DateTime startDate, int courseId)
        {
            if (_context.Course == null)
                return "Entity set 'ApplicationDbContext.Course'  is null.";

            var course = await _context.Course.Include(c => c.Modules).FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
                return "Ogiltigt kurs-ID.";

            if (DateTime.Compare(startDate, course.StartDate) < 0)
                return $"Modulens startdatum måste ligga efter kursens startdatum: {course.StartDate.ToShortDateString()}";
            else if (DateTime.Compare(startDate, course.EndDate) > 0)
                return $"Modulens startdatum måste ligga innan kursens slutdatum: {course.EndDate.ToShortDateString()}";

            var modules = course.Modules;

            foreach (var module in modules)
                if (DateTime.Compare(startDate, module.StartDate) > 0 && DateTime.Compare(startDate, module.EndDate) < 0)
                    return $"Startdatum ogiltigt, överlappar annan modul med tidsspann: {module.StartDate.ToShortDateString()} - {module.EndDate.ToShortDateString()}";

            return "true";
        }

        public async Task<string> ValidateModuleEndDate(DateTime endDate, DateTime startDate, int courseId)
        {
            if (_context.Course == null)
                return "Entity set 'ApplicationDbContext.Course'  is null.";

            var course = await _context.Course.Include(c => c.Modules).FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
                return "Ogiltigt kurs-ID.";

            if (DateTime.Compare(endDate, course.StartDate) < 0)
                return $"Modulens slutdatum måste ligga efter kursens startdatum: {course.StartDate.ToShortDateString()}";
            else if (DateTime.Compare(endDate, course.EndDate) > 0)
                return $"Modulens slutdatum måste ligga innan kursens slutdatum: {course.EndDate.ToShortDateString()}";

            if (DateTime.Compare(endDate, startDate) < 0)
                return $"Modulens slutdatum måste ligga efter modulens startdatum: {startDate.ToShortDateString()}";

            var modules = course.Modules;

            foreach (var module in modules)
                if (DateTime.Compare(endDate, module.StartDate) > 0 && DateTime.Compare(endDate, module.EndDate) < 0)
                    return $"Slutdatum ogiltigt, överlappar annan modul med tidsspann: {module.StartDate.ToShortDateString()} - {module.EndDate.ToShortDateString()}";


            return "true";
        }

        public async Task<string> ValidateActivityStartDate(DateTime startDate, int moduleId)
        {
            if (_context.Module == null)
                return "Entity set 'ApplicationDbContext.Module'  is null.";

            var module = await _context.Module.Include(c => c.Activities).FirstOrDefaultAsync(c => c.Id == moduleId);

            if (module == null)
                return "Ogiltigt modul-ID.";

            if (DateTime.Compare(startDate, module.StartDate) < 0)
                return $"Modulens startdatum måste ligga efter modulens startdatum: {module.StartDate.ToShortDateString()}";
            else if (DateTime.Compare(startDate, module.EndDate) > 0)
                return $"Modulens startdatum måste ligga innan modulens slutdatum: {module.EndDate.ToShortDateString()}";

            var activities = module.Activities;

            foreach (var activity in activities)
                if (DateTime.Compare(startDate, activity.StartDate) > 0 && DateTime.Compare(startDate, activity.EndDate) < 0)
                    return $"Startdatum ogiltigt, överlappar en annnan aktivitet med tidsspann {activity.StartDate.ToShortDateString()} - {activity.EndDate.ToShortDateString()}";

            return "true";
        }

        public async Task<string> ValidateActivityEndDate(DateTime endDate, DateTime startDate, int moduleId)
        {
            if (_context.Module == null)
                return "Entity set 'ApplicationDbContext.Module'  is null.";

            var module = await _context.Module.Include(c => c.Activities).FirstOrDefaultAsync(c => c.Id == moduleId);

            if (module == null)
                return "Ogiltigt modul-ID.";

            if (DateTime.Compare(endDate, module.StartDate) < 0)
                return $"Aktivitetens slutdatum måste ligga efter modulens startdatum: {module.StartDate.ToShortDateString()}";
            else if (DateTime.Compare(endDate, module.EndDate) > 0)
                return $"Aktivitetens slutdatum måste ligga innan modulens slutdatum: {module.EndDate.ToShortDateString()}";

            if (DateTime.Compare(endDate, startDate) < 0)
                return $"Aktivitetens slutdatum måste ligga efter aktivitetens startdatum: {startDate.ToShortDateString()}";

            var activities = module.Activities;

            foreach (var activity in activities)
                if (DateTime.Compare(endDate, activity.StartDate) > 0 && DateTime.Compare(startDate, activity.EndDate) < 0)
                    return $"Slutdatum ogiltigt, överlappar en annnan aktivitet med tidsspann {activity.StartDate.ToShortDateString()} - {activity.EndDate.ToShortDateString()}";

            return "true";
        }
    }
}

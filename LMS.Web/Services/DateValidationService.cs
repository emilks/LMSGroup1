using LMS.Core.Entities;
using LMS.Core.Repositories;
using LMS.Core.Services;
using LMS.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Web.Services
{
    public class DateValidationService : IDateValidationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _uow;

        public DateValidationService(ApplicationDbContext context, IUnitOfWork uow)
        {
            _context = context; // Remove context and use only uow
            _uow = uow; // Currently unused
        }

        public async Task<string> ValidateModuleStartDate(DateTime startDate, int courseId)
        {
            if (_context.Course == null)
                return "Entity set 'ApplicationDbContext.Course'  is null.";

            var course = await _context.Course.Include(c => c.Modules).FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
                return "Ogiltigt kurs-ID.";

            if (startDate < course.StartDate)
                return $"Modulens startdatum måste ligga efter kursens startdatum: {course.StartDate.ToShortDateString()}";
            else if (startDate > course.EndDate)
                return $"Modulens startdatum måste ligga innan kursens slutdatum: {course.EndDate.ToShortDateString()}";

            var overlap = course.Modules.FirstOrDefault(m => startDate > m.StartDate && startDate < m.EndDate);

            if (overlap != null)
                return $"Startdatum ogiltigt, överlappar annan modul med tidsspann: {overlap.StartDate.ToShortDateString()} - {overlap.EndDate.ToShortDateString()}";

            return "true";
        }

        public async Task<string> ValidateModuleEndDate(DateTime endDate, DateTime startDate, int courseId)
        {
            if (_context.Course == null)
                return "Entity set 'ApplicationDbContext.Course'  is null.";

            var course = await _context.Course.Include(c => c.Modules).FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
                return "Ogiltigt kurs-ID.";

            if (endDate < course.StartDate)
                return $"Modulens slutdatum måste ligga efter kursens startdatum: {course.StartDate.ToShortDateString()}";
            else if (endDate > course.EndDate)
                return $"Modulens slutdatum måste ligga innan kursens slutdatum: {course.EndDate.ToShortDateString()}";

            if (endDate < startDate)
                return $"Modulens slutdatum måste ligga efter modulens startdatum: {startDate.ToShortDateString()}";

            var overlap = course.Modules.FirstOrDefault(m => endDate > m.StartDate && endDate < m.EndDate);
            if (overlap != null)
                return $"Startdatum ogiltigt, överlappar annan modul med tidsspann: {overlap.StartDate.ToShortDateString()} - {overlap.EndDate.ToShortDateString()}";

            return "true";
        }

        public async Task<string> ValidateActivityStartDate(DateTime startDate, int moduleId)
        {
            if (_context.Module == null)
                return "Entity set 'ApplicationDbContext.Module'  is null.";

            var module = await _context.Module.Include(c => c.Activities).FirstOrDefaultAsync(c => c.Id == moduleId);

            if (module == null)
                return "Ogiltigt modul-ID.";

            if (startDate < module.StartDate)
                return $"Aktivitetens startdatum måste ligga efter modulens startdatum: {module.StartDate.ToShortDateString()}";
            else if (startDate > module.EndDate)
                return $"Aktivitetens startdatum måste ligga innan modulens slutdatum: {module.EndDate.ToShortDateString()}";

            var overlap = module.Activities.FirstOrDefault(a => startDate > a.StartDate && startDate < a.EndDate);
            if (overlap != null)
                return $"Startdatum ogiltigt, överlappar annan aktivitet med tidsspann: {overlap.StartDate.ToShortDateString()} - {overlap.EndDate.ToShortDateString()}";

            return "true";
        }

        public async Task<string> ValidateActivityEndDate(DateTime endDate, DateTime startDate, int moduleId)
        {
            if (_context.Module == null)
                return "Entity set 'ApplicationDbContext.Module'  is null.";

            var module = await _context.Module.Include(c => c.Activities).FirstOrDefaultAsync(c => c.Id == moduleId);

            if (module == null)
                return "Ogiltigt modul-ID.";

            if (endDate < module.StartDate)
                return $"Aktivitetens slutdatum måste ligga efter modulens startdatum: {module.StartDate.ToShortDateString()}";
            else if (endDate > module.EndDate)
                return $"Aktivitetens slutdatum måste ligga innan modulens slutdatum: {module.EndDate.ToShortDateString()}";

            if (endDate < startDate)
                return $"Aktivitetens slutdatum måste ligga efter aktivitetens startdatum: {startDate.ToShortDateString()}";

            var overlap = module.Activities.FirstOrDefault(a => endDate > a.StartDate && endDate < a.EndDate);
            if (overlap != null)
                return $"Slutdatum ogiltigt, överlappar annan aktivitet med tidsspann: {overlap.StartDate.ToShortDateString()} - {overlap.EndDate.ToShortDateString()}";

            return "true";
        }
    }
}

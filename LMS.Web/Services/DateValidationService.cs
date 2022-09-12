using LMS.Core.Entities;
using LMS.Core.Repositories;
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
            _context = context;
            _uow = uow;
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

            var modules = course.Modules;

            foreach (var module in modules)
                if (startDate > module.StartDate && startDate < module.EndDate)
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

            if (endDate < course.StartDate)
                return $"Modulens slutdatum måste ligga efter kursens startdatum: {course.StartDate.ToShortDateString()}";
            else if (endDate > course.EndDate)
                return $"Modulens slutdatum måste ligga innan kursens slutdatum: {course.EndDate.ToShortDateString()}";

            if (endDate < startDate)
                return $"Modulens slutdatum måste ligga efter modulens startdatum: {startDate.ToShortDateString()}";

            var modules = course.Modules;

            foreach (var module in modules)
                if (endDate > module.StartDate && endDate < module.EndDate)
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

            if (startDate < module.StartDate)
                return $"Modulens startdatum måste ligga efter modulens startdatum: {module.StartDate.ToShortDateString()}";
            else if (startDate > module.EndDate)
                return $"Modulens startdatum måste ligga innan modulens slutdatum: {module.EndDate.ToShortDateString()}";

            var activities = module.Activities;

            foreach (var activity in activities)
                if (startDate > activity.StartDate && startDate < activity.EndDate)
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

            if (endDate < module.StartDate)
                return $"Aktivitetens slutdatum måste ligga efter modulens startdatum: {module.StartDate.ToShortDateString()}";
            else if (endDate > module.EndDate)
                return $"Aktivitetens slutdatum måste ligga innan modulens slutdatum: {module.EndDate.ToShortDateString()}";

            if (endDate < startDate)
                return $"Aktivitetens slutdatum måste ligga efter aktivitetens startdatum: {startDate.ToShortDateString()}";

            var activities = module.Activities;

            foreach (var activity in activities)
                if (endDate > activity.StartDate && endDate < activity.EndDate)
                    return $"Slutdatum ogiltigt, överlappar en annnan aktivitet med tidsspann {activity.StartDate.ToShortDateString()} - {activity.EndDate.ToShortDateString()}";

            return "true";
        }
    }
}

using LMS.Core.Services;
using LMS.Data.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LMS.Web.Services
{
    public class ActivityTypeService : IActivityTypeService
    {
        private readonly ApplicationDbContext _context;

        public ActivityTypeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SelectListItem>> GetActivityType()
        {
            return await _context.ActivityType.Select(a => new SelectListItem
            {
                Text = a.ActivityName,
                Value = a.Id.ToString()
            }).ToListAsync();
        }
    }
}

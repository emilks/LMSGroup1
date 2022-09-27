using AutoMapper;
using LMS.Core.ViewModels;
using LMS.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS.Web.ViewComponents
{
    public class AssignmentsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public AssignmentsViewComponent(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        public IViewComponentResult Invoke(int id)
        {
            var course = _context.Course.FirstOrDefault(c => c.Id == id);

            var activities = _context.Activity.Where(a => a.ActivityType.ActivityName.Equals("Inlämning"))
                .Where(a => a.Module.CourseId == course.Id)
                .OrderBy(a => a.EndDate)
                .Include(a => a.Documents).ThenInclude(d => d.Owner);

            var viewModels = mapper.ProjectTo<ActivitiesViewModel>(activities);

            return View(viewModels);
        }
    }
}

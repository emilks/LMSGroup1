using AutoMapper;
using LMS.Core.Repositories;
using LMS.Core.ViewModels;
using LMS.Data.Data;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly IUnitOfWork uow;

        public ContactController(ApplicationDbContext context, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _context = context;
            this.mapper = mapper;
            uow = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var course = await uow.CourseRepository.GetTeacherContacts();
            if (course == null)
            {
                return Problem($"´No contacts found.");
            }

            var vm = mapper.ProjectTo<ContactsViewModel>(course.AsQueryable());

            return View(vm);
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
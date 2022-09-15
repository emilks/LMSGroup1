using AutoMapper;
using LMS.Core.Repositories;
using LMS.Core.ViewModels;
using LMS.Data.Data;
using Microsoft.AspNetCore.Mvc;
using LMS.Core.Entities;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Email")] TeacherUser @teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@teacher);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Activity == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            return View(activity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("FirstName,LastName,Email")] TeacherUser @teacheruser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@teacheruser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(@teacheruser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(teacheruser);
        }

        private bool TeacherExists(string id)
        {
            return (_context.TeacherUser?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
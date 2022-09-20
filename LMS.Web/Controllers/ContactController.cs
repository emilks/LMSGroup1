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

            var vm2 = mapper.Map<IEnumerable<ContactsViewModel>>(course);

            return View(vm2);
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

        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || _context.TeacherUser == null)
            {
                return NotFound();
            }

            var teacherUser = await _context.TeacherUser.FindAsync(id);
            if (teacherUser == null)
            {
                return NotFound();
            }
            return View(teacherUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("FirstName,LastName,Email")] TeacherUser teacheruser)
        {
            if (id != teacheruser.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacheruser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacheruser.Id))
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

        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.TeacherUser == null)
            {
                return NotFound();
            }

            var teacher = await _context.TeacherUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.TeacherUser == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TeacherUser'  is null.");
            }
            var teacher = await _context.TeacherUser.FindAsync(id);
            if (teacher != null)
            {
                _context.TeacherUser.Remove(teacher);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
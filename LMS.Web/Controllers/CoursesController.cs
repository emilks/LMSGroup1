﻿using AutoMapper;
using LMS.Core.Entities;
using LMS.Core.Repositories;
using LMS.Core.ViewModels;
using LMS.Data.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace LMS.Web.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly IUnitOfWork uow;
        private readonly UserManager<IdentityUser> userManager;

        public CoursesController(ApplicationDbContext context, IMapper mapper, IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.mapper = mapper;
            uow = unitOfWork;
            this.userManager = userManager;
        }

        // GET: Courses/Contacts/5
        public async Task<IActionResult> Contacts(int? id) {
            var course = await uow.CourseRepository.GetCourseWithContacts(id);
            if (course == null) {
                return Problem($"The course with id: {id} could not be found.");
            }

            var vm = mapper.Map<CourseContactsViewModel>(course);

            return View(vm);
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Student"))
            {
                return RedirectToAction("MyCourse");
            }

            var courses = await uow.CourseRepository.GetCourses(includeModules: true);
            if(courses == null) {
                return View();
            }

            var viewModel = mapper.ProjectTo<MainCourseIndexViewModel>(courses.AsQueryable());

            return View(viewModel);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            //var course = await _context.Course
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var course = await uow.CourseRepository.GetCourseFull(id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        [Authorize(Roles = "Teacher")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
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
            return View(course);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Course == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Course'  is null.");
            }

            var course = await uow.CourseRepository.GetCourseFull(id);

            if (course != null)
            {
                _context.RemoveRange(course.Documents);
                _context.RemoveRange(course.Modules.SelectMany(m => m.Documents));
                _context.RemoveRange(course.Modules.SelectMany(m => m.Activities).SelectMany(m => m.Documents));

                uow.CourseRepository.RemoveCourse(course);
            }

            await uow.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
          return (_context.Course?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult CreatePartial()
        {
            return PartialView();
        }

        public async Task<IActionResult> ContactsPartial(int? id)
        {
            var course = await uow.CourseRepository.GetCourseWithContacts(id);
            if (course == null)
            {
                return Problem($"The course with id: {id} could not be found.");
            }

            var vm = mapper.Map<CourseContactsViewModel>(course);

            return PartialView(vm);
        }
        public async Task<IActionResult> DetailedView(int? id, bool check = false)
        {
            if (User.IsInRole("Student") && check == false)
            {
                return RedirectToAction("MyCourse");

                /*var userId = userManager.GetUserId(User);

                var courseId = _context.Course.Include(e => e.Students)
                    .Where(e => e.Students.Any(f => f.Id.Equals(userId)))
                    .FirstOrDefault();

                id = courseId?.Id;
                if (courseId == null)
                {
                    id = 1;
                }*/
            }

            var course = await uow.CourseRepository.GetCourseFull(id);

            var viewModel = mapper.Map<MainCourseIndexViewModel>(course);

            return View(viewModel);
        }

        public async Task<IActionResult> MyCourse()
        {
            var userId = userManager.GetUserId(User);

            var courseId = await _context.Course.Include(e => e.Students)
                .Where(e => e.Students.Any(f => f.Id.Equals(userId)))
                .FirstOrDefaultAsync();
            //.Select(e => e.Id);
            var test = courseId.Id;

            return RedirectToAction("DetailedView", new { id = test, check = true });
        }

    }
}

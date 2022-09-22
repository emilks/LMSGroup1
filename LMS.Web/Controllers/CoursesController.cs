﻿using AutoMapper;
using LMS.Core.Entities;
using LMS.Core.Repositories;
using LMS.Core.ViewModels;
using LMS.Data.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;

namespace LMS.Web.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly IUnitOfWork uow;
        private readonly UserManager<IdentityUser> userManager;

        public CoursesController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context, IMapper mapper, IUnitOfWork unitOfWork, UserManager<IdentityUser> um) {
            this.webHostEnvironment = webHostEnvironment;
        //public CoursesController(ApplicationDbContext context, IMapper mapper, IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        //{
            _context = context;
            this.mapper = mapper;
            uow = unitOfWork;
            userManager = um;
            //this.userManager = userManager;
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
            if (courses == null) {
                return View();
            }

            var viewModel = mapper.ProjectTo<MainCourseIndexViewModel>(courses.AsQueryable());

            return View(viewModel);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null || _context.Course == null) {
                return NotFound();
            }

            //var course = await _context.Course
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var course = await uow.CourseRepository.GetCourseFull(id);

            if (course == null) {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/UploadDocument
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadDocument(CourseViewModel model) {
            if (ModelState.IsValid == false) {
                return Problem("Could not upload file, model state not valid");
            }

            // create file object
            var documentName = model.FileBuffer!.FileName;

            var document = new Document() {
                Name = documentName,
                Description = model.DocumentDescription,
                FilePath = $"/files/courses/{model.Name}/{documentName}",
                Owner = await userManager.GetUserAsync(User),
                Course = await uow.CourseRepository.GetCourseWithContacts(model.Id), // make 'WithContacts' optional!
                Module = null, // ??
                Activity = null // ??
            };

            // save file
            var documentPath = $"files\\courses\\{model.Name}";
            var path = Path.Combine(webHostEnvironment.WebRootPath, documentPath);

            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(documentPath);
            }

            using (Stream fileStream = new FileStream(Path.Combine(path, documentName), FileMode.Create)) {
                await model.FileBuffer.CopyToAsync(fileStream);
            }

            // update data base
            uow.CourseRepository.AddDocument(document.Course, document);
            await uow.CompleteAsync();

            // expects an object as id, that's why an anonymous object is used
            return RedirectToAction("DetailedView", new { id = model.Id });
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
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate")] Course course) {
            if (ModelState.IsValid) {
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
            if (course == null) {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate")] Course course) {
            if (id != course.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!CourseExists(course.Id)) {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(DetailedView), new { Id = id});
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null || _context.Course == null) {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null) {
                return NotFound();
            }

            return View(course);
        }

        public async Task<IActionResult> DeletePartial(int? id)
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

            return PartialView(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            if (_context.Course == null) {
                return Problem("Entity set 'ApplicationDbContext.Course'  is null.");
            }

            var course = await uow.CourseRepository.GetCourseFull(id);

            if (course != null) {
                _context.RemoveRange(course.Documents);
                _context.RemoveRange(course.Modules.SelectMany(m => m.Documents));
                _context.RemoveRange(course.Modules.SelectMany(m => m.Activities).SelectMany(m => m.Documents));

                uow.CourseRepository.RemoveCourse(course);
            }

            await uow.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id) {
            return (_context.Course?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult CreatePartial()
        {
            return PartialView();
        }

        public async Task<IActionResult> ContactsPartial(int? id) {
            var course = await uow.CourseRepository.GetCourseWithContacts(id);
            if (course == null) {
                return Problem($"The course with id: {id} could not be found.");
            }

            var vm = mapper.Map<CourseContactsViewModel>(course);

            return PartialView(vm);
        }
        public async Task<IActionResult> DetailedView(int? id)
        {
            if (User.IsInRole("Student"))
            {

                var userId = userManager.GetUserId(User);

                var courseId = _context.Course.Include(e => e.Students)
                    .Where(e => e.Students.Any(f => f.Id.Equals(userId)))
                    .FirstOrDefault();

                id = courseId?.Id;
                if (courseId == null)
                {
                    id = 1;
                }
            }

            var course = await uow.CourseRepository.GetCourseFull(id);
            if (course == null) {
                return Problem($"The course with id: {id} could not be found.");
            }

            //var viewModel = mapper.Map<MainCourseIndexViewModel>(course);
            var viewModel = mapper.Map<CourseViewModel>(course);

            TempData["CourseId"] = id;

            return View(viewModel);
        }

        public IActionResult AddStudentsPartial()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudentsPartial(StudentUserViewModel @student)
        {
            var mapped = mapper.Map<StudentUser>(student);

            mapped.CourseId = int.Parse(TempData["CourseId"].ToString());

            TempData.Keep("CourseId");

            if (ModelState.IsValid)
            {
                _context.Add(mapped);
                await _context.SaveChangesAsync();

                return RedirectToAction("DetailedView", "Courses", new { id = int.Parse(TempData["CourseId"].ToString()) });
            }
            return View(@student);
        }

        public async Task<IActionResult> MyCourse()
        {
            var userId = userManager.GetUserId(User);

            var courseId = await _context.Course.Include(e => e.Students)
                .Where(e => e.Students.Any(f => f.Id.Equals(userId)))
                .FirstOrDefaultAsync();
            //.Select(e => e.Id);
            var test = courseId.Id;

            return RedirectToAction("DetailedView", new { id = test });
        }

    }
}

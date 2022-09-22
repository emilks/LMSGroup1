using LMS.Core.Entities;
using LMS.Core.Repositories;
using LMS.Core.Services;
using LMS.Core.ViewModels;
using LMS.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS.Web.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDateValidationService _dateValidationService;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IUnitOfWork uow;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ActivitiesController(IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork, ApplicationDbContext context, IDateValidationService dateValidationService, UserManager<IdentityUser> um)
        {
            _context = context;
            _dateValidationService = dateValidationService;
            userManager = um;
            uow = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
        }

        // POST: Activities/UploadDocument
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadDocument(CourseViewModel model, int parentId) {
            if (ModelState.IsValid == false) {
                return Problem("Could not upload file, model state not valid");
            }

            // create file object
            var fileName = model.FileBuffer!.FileName;
            var relativePath = $"/files/courses/{model.Name}";
            var createPath = Path.Combine(webHostEnvironment.WebRootPath, relativePath);
            string filePath = Path.Combine(createPath, fileName);

            var activity = _context.Activity.FirstOrDefault(a => a.Id == model.documentParentId);

            relativePath += "/" + activity.Module.Name + "/" + activity.Name;
            // temp
            //
            // public files
            // files/courses/{courseName}/{moduleName}/{activityName}/{documentName}
            //
            // personal files
            // files/courses/{courseName}/{moduleName}/{activityName}/{studentUserName}/{documentName}
            //
            // $"files\\courses\\{Model.Name}\\{item.Name}\\{activity.Name}\\{document.Name}"

            if (Directory.Exists(createPath) == false) {
                Directory.CreateDirectory(createPath);
            }

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create)) {
                model.FileBuffer.CopyTo(fileStream);
            }

            var course = await uow.CourseRepository.GetCourseWithContacts(model.Id);
            ArgumentNullException.ThrowIfNull(nameof(course));

            var document = new Document() {
                Name = fileName,
                Description = model.DocumentDescription,
                FilePath = relativePath + "/" + fileName,
                IdentityUserId = userManager.GetUserId(User),
                // Owner is needed!
                Owner = await userManager.GetUserAsync(User),
                Course = course,
                Module = null,
                Activity = null
            };

            // update data base
            uow.CourseRepository.AddDocument(document.Course!, document);
            await uow.CompleteAsync();

            // expects an object as id, that's why an anonymous object is used
            return RedirectToAction("DetailedView", new { id = model.Id });
        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
              return _context.Activity != null ? 
                          View(await _context.Activity.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Activity'  is null.");
        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Activity == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // GET: Activities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate,ModuleId")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(activity);
        }

        // GET: Activities/Edit/5
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

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate")] Activity activity)
        {
            if (id != activity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.Id))
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
            return View(activity);
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Activity == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Activity == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Activity'  is null.");
            }
            var activity = await _context.Activity.Include(a => a.Documents).FirstOrDefaultAsync(a => a.Id == id);
            if (activity != null)
            {
                _context.RemoveRange(activity.Documents);
                _context.Activity.Remove(activity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
          return (_context.Activity?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> VerifyStartDate(DateTime startDate, int moduleId)
        {
            return Json(await _dateValidationService.ValidateActivityStartDate(startDate, moduleId));
        }

        public async Task<IActionResult> VerifyEndDate(DateTime endDate, DateTime startDate, int moduleId)
        {
            return Json(await _dateValidationService.ValidateActivityEndDate(endDate, startDate, moduleId));
        }
    }
}

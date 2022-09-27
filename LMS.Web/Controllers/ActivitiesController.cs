using AutoMapper;
using LMS.Core.Entities;
using LMS.Core.Repositories;
using LMS.Core.Services;
using LMS.Core.ViewModels;
using LMS.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Activity = LMS.Core.Entities.Activity;

namespace LMS.Web.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDateValidationService _dateValidationService;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IUnitOfWork uow;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ActivitiesController(IDateValidationService dateValidationService,
                                    IMapper mapper, 
                                    IWebHostEnvironment webHostEnvironment, 
                                    IUnitOfWork unitOfWork, 
                                    ApplicationDbContext context, 
                                    UserManager<IdentityUser> um)
        {
            _context = context;
            _dateValidationService = dateValidationService;
            userManager = um;
            uow = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
            this.mapper = mapper;
        }


        // POST: Activities/UploadDocument
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadDocument(CourseViewModel model) {
            //if (ModelState.IsValid == false) {
            //    return Problem("Could not upload file, model state not valid");
            //}

            // name convention
            //
            // public files
            // files/courses/{courseName}/{moduleName}/{activityName}/{fileName}
            //
            // personal files
            // files/courses/{courseName}/{moduleName}/{activityName}/{studentUserName}/{fileName}
            //

            // sort out names for file path
            var activity = await uow.ActivityRepository.GetActivity(model.documentParentId, includeModuleAndDocuments: true);
            if (activity == null) throw new ArgumentNullException(nameof(activity));

            var courseName = model.Name;
            var moduleName = activity.Module.Name;
            var activityName = activity.Name;
            var fileName = model.FileBuffer!.FileName;
            var userName = userManager.GetUserName(User);

            // create file path
            var relativePath = $"files/courses/{courseName}/{moduleName}/{activityName}"; 

            if(User.IsInRole("Student")) {
                relativePath += $"/{userName}";
            }

            var absolutePath = Path.Combine(webHostEnvironment.WebRootPath, relativePath);
            string filePath = Path.Combine(absolutePath, fileName);

            if (Directory.Exists(absolutePath) == false) {
                Directory.CreateDirectory(absolutePath);
            }

            // write file to disk
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create)) {
                model.FileBuffer.CopyTo(fileStream);
            }

            // create document
            var document = new Document() {
                Name = fileName,
                Description = model.DocumentDescription,
                FilePath = relativePath + "/" + fileName,
                IdentityUserId = userManager.GetUserId(User),
                // Owner is needed!
                Owner = await userManager.GetUserAsync(User),
                Course = null,
                Module = null,
                Activity = activity
            };

            // update data base
            await uow.ActivityRepository.AddDocument(activity, document);
            await uow.CompleteAsync();

            // expects an object as id, that's why an anonymous object is used
            return RedirectToAction("DetailedView", "Courses", new { id = model.Id });
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
        /*[HttpPost]
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
        }*/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActivitiesViewModel viewModel)
        {
            var activityType = _context.ActivityType.FirstOrDefault(a => a.Id == viewModel.ActivityTypeId);

            var activity  = mapper.Map<Activity>(viewModel);
            activity.ActivityType = activityType;
            var courseId = int.Parse(TempData["CourseId"].ToString());

            if (ModelState.IsValid)
            {
                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction("DetailedView", "Courses", new { id = courseId });
            }
            return View(activity);
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> EditPartial(int? id)
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
            return PartialView(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPartial(int id, Activity activity)
        {
            if (id != activity.Id)
            {
                return NotFound();
            }
            var moduleId =_context.Module.FirstOrDefault(m => m.Id == activity.ModuleId);
            var courseId = moduleId.CourseId;

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
                return RedirectToAction("DetailedView", "Courses", new { id = courseId });
            }
            return View(activity);
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> DeletePartial(int? id)
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

            return PartialView(activity);
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
            var moduleId = new Module();
            var courseId = moduleId.CourseId;

            if (activity != null)
            {
                _context.RemoveRange(activity.Documents);
                _context.Activity.Remove(activity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("DetailedView", "Courses", new { id = courseId });
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMS.Core.Entities;
using LMS.Data.Data;
using LMS.Web.Services;
using LMS.Core.Services;
using LMS.Core.ViewModels;
using AutoMapper;
using System.Diagnostics;
using Activities = LMS.Core.Entities.Activities;

namespace LMS.Web.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDateValidationService _dateValidationService;
        private readonly IMapper mapper;

        public ActivitiesController(ApplicationDbContext context, IDateValidationService dateValidationService, IMapper mapper)
        {
            _context = context;
            _dateValidationService = dateValidationService;
            this.mapper = mapper;
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

            var activity  = mapper.Map<Activities>(viewModel);
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
        public async Task<IActionResult> EditPartial(int id, [Bind("Id,Name,Description,StartDate,EndDate")] Activities activity)
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

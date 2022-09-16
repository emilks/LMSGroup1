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
using Microsoft.CodeAnalysis;
using System.Runtime.Intrinsics.X86;
using LMS.Core.ViewModels;
using Bogus;
using AutoMapper;

namespace LMS.Web.Controllers
{
    public class ModulesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDateValidationService _dateValidationService;
        private readonly IMapper mapper;

        public ModulesController(ApplicationDbContext context, IDateValidationService dateValidationService, IMapper mapper)
        {
            _context = context;
            _dateValidationService = dateValidationService;
            this.mapper = mapper;
        }

        // GET: Modules
        public async Task<IActionResult> Index()
        {
            return _context.Module != null ?
                        View(await _context.Module.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Module'  is null.");
        }

        // GET: Modules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Module == null)
            {
                return NotFound();
            }

            var @module = await _context.Module
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@module == null)
            {
                return NotFound();
            }

            return View(@module);
        }

        // GET: Modules/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate,CourseId")] Module @module)
        public async Task<IActionResult> Create(ModuleViewModel @module)
        {
            var mapped = mapper.Map<Module>(module);
            mapped.CourseId = int.Parse(TempData["CourseId"].ToString());

            TempData.Keep("CourseId");

            if (ModelState.IsValid)
            {
                _context.Add(mapped);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("DetailedView", "Courses", new {id = int.Parse(TempData["CourseId"].ToString()) });
            }
            return View(@module);
        }

        // GET: Modules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Module == null)
            {
                return NotFound();
            }

            var @module = await _context.Module.FindAsync(id);
            if (@module == null)
            {
                return NotFound();
            }
            return View(@module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate")] Module @module)
        {
            if (id != @module.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@module);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleExists(@module.Id))
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
            return View(@module);
        }

        // GET: Modules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Module == null)
            {
                return NotFound();
            }

            var @module = await _context.Module
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@module == null)
            {
                return NotFound();
            }

            return View(@module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Module == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Module'  is null.");
            }
            var @module = await _context.Module.FindAsync(id);
            if (@module != null)
            {
                _context.Module.Remove(@module);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModuleExists(int id)
        {
            return (_context.Module?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> VerifyStartDate(DateTime startDate)
        {
            string courseIdStr = TempData["CourseId"].ToString();
            TempData.Keep("CourseId");
            int courseId = int.Parse(courseIdStr);
            return Json(await _dateValidationService.ValidateModuleStartDate(startDate, courseId));
        }

        public async Task<IActionResult> VerifyEndDate(DateTime endDate,
            [Bind(Prefix = "StartDate")] DateTime startDate)
        {
            string courseIdStr = TempData["CourseId"].ToString();
            TempData.Keep("CourseId");
            int courseId = int.Parse(courseIdStr);
            return Json(await _dateValidationService.ValidateModuleEndDate(endDate, startDate, courseId));
        }

        public async Task<IActionResult> VerifyDuration(int duration,
            [Bind(Prefix = "StartDate")] DateTime startDate)
        {
            string courseIdStr = TempData["CourseId"].ToString();
            TempData.Keep("CourseId");
            int courseId = int.Parse(courseIdStr);
            return Json(await _dateValidationService.ValidateModuleDuration(duration, startDate, courseId));
        }
    }
}

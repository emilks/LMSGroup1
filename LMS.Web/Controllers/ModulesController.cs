using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMS.Core.Entities;
using LMS.Data.Data;

namespace LMS.Web.Controllers
{
    public class ModulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ModulesController(ApplicationDbContext context)
        {
            _context = context;
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
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate,CourseId")] Module @module)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@module);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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

        public async Task<IActionResult> VerifyStartDate(DateTime startDate, int courseId)
        {
            if (_context.Course == null)
                return Json("Entity set 'ApplicationDbContext.Course'  is null.");

            var course = await _context.Course.Include(c => c.Modules).FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
                return Json("Ogiltigt kurs-ID.");

            if (DateTime.Compare(startDate, course.StartDate) < 0)
                return Json($"Modulens startdatum måste ligga efter kursens startdatum: {course.StartDate.ToShortDateString()}");
            else if (DateTime.Compare(startDate, course.EndDate) > 0)
                return Json($"Modulens startdatum måste ligga innan kursens slutdatum: {course.EndDate.ToShortDateString()}");

            var modules = course.Modules;

            foreach (var module in modules)
                if (DateTime.Compare(startDate, module.StartDate) > 0 && DateTime.Compare(startDate, module.EndDate) < 0)
                    return Json($"Startdatum ogiltigt, överlappar en annnan modul med tidsspann: {module.StartDate.ToShortDateString()} - {module.EndDate.ToShortDateString()}");

            return Json(true);
        }

        public async Task<IActionResult> VerifyEndDate(DateTime endDate, int courseId, DateTime startDate)
        {
            if (_context.Course == null)
                return Json("Entity set 'ApplicationDbContext.Course'  is null.");

            var course = await _context.Course.Include(c => c.Modules).FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
                return Json("Ogiltigt kurs-ID.");

            if (DateTime.Compare(endDate, course.StartDate) < 0)
                return Json($"Modulens slutdatum måste ligga efter kursens startdatum: {course.StartDate.ToShortDateString()}");
            else if (DateTime.Compare(endDate, course.EndDate) > 0)
                return Json($"Modulens slutdatum måste ligga innan kursens slutdatum: {course.EndDate.ToShortDateString()}");

            if (DateTime.Compare(endDate, startDate) < 0)
                return Json($"Modulens slutdatum måste ligga efter modulens startdatum: {startDate.ToShortDateString()}");

            var modules = course.Modules;

            foreach (var module in modules)
                if (DateTime.Compare(endDate, module.StartDate) > 0 && DateTime.Compare(endDate, module.EndDate) < 0)
                    return Json($"Slutdatum ogiltigt, överlappar en annnan modul med tidsspann: {module.StartDate.ToShortDateString()} - {module.EndDate.ToShortDateString()}");


            return Json(true);
        }
    }
}

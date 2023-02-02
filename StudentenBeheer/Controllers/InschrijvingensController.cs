using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using StudentenBeheer.Data;
using StudentenBeheer.Models;

namespace StudentenBeheer.Controllers
{

    [Authorize]
    public class InschrijvingensController : ApplicationController
    {
        //private readonly ApplicationContext _context;
        private readonly IStringLocalizer<InschrijvingensController> _localizer;


        public InschrijvingensController(ApplicationContext context,
                                        IHttpContextAccessor httpContextAccessor,
                                        ILogger<ApplicationController> logger, IStringLocalizer<InschrijvingensController> localizer) : base(context, httpContextAccessor, logger)
        {
            _localizer = localizer;
        }

        // GET: Inschrijvingens
        public async Task<IActionResult> Index()
        {
            var studentenBeheerContext = _context.Inschrijvingen.Include(i => i.Module).Include(i => i.Student).Where(i => i.Student.UserId == _user.Id);

            if (!User.IsInRole("Student"))
            {
                studentenBeheerContext = _context.Inschrijvingen.Include(i => i.Module).Include(i => i.Student);
            }

            if (User.IsInRole("Docent"))
            {
                var Modules = _context.Docenten_modules.Include(m => m.module).Include(m => m.docent)
                                                       .Where(m => m.docent.UserId == _user.Id)
                                                       .Select(m => m.module.Id)
                                                       .ToList();

                studentenBeheerContext = _context.Inschrijvingen.Include(i => i.Module)
                                                                .Include(i => i.Student)
                                                                .Where(i => Modules.Contains(i.Module.Id));
            }



            return View(await studentenBeheerContext.ToListAsync());
        }

        // GET: Inschrijvingens/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inschrijvingen = await _context.Inschrijvingen
                .Include(i => i.Module)
                .Include(i => i.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inschrijvingen == null)
            {
                return NotFound();
            }

            return View(inschrijvingen);
        }

        // GET: Inschrijvingens/Create
        [Authorize(Roles = "Beheerder")]
        public IActionResult Create(int? id, int? welke)
        {
            var student = _context.Student.Where(s => s.Deleted > DateTime.Now);
            var module = _context.Module.Where(m => m.Deleted > DateTime.Now);
            ViewData["ModuleId"] = new SelectList(module, "Id", "Name");
            ViewData["StudentId"] = new SelectList(student, "Id", "Lastname");

            if (welke == 0)
            {
                // module

                module = _context.Module.Where(m => m.Id == id);
                ViewData["ModuleId"] = new SelectList(module, "Id", "Name");
                ViewData["StudentId"] = new SelectList(student, "Id", "Lastname");

            }

            if (welke == 1)
            {

                //student

                student = _context.Student.Where(m => m.Id == id);
                ViewData["ModuleId"] = new SelectList(module, "Id", "Name");
                ViewData["StudentId"] = new SelectList(student, "Id", "Lastname");

            }


            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Beheerder")]
        public async Task<IActionResult> Create(int? id, int? welke, [Bind("Id,ModuleId,StudentId,InschrijvingsDatum,AfgelegdOp,Resultaat")] Inschrijvingen inschrijvingen)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inschrijvingen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Name", inschrijvingen.ModuleId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Lastname", inschrijvingen.StudentId);
            return View(inschrijvingen);
        }

        [Authorize(Roles = "Beheerder")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inschrijvingen = await _context.Inschrijvingen.FindAsync(id);
            if (inschrijvingen == null)
            {
                return NotFound();
            }
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Name", inschrijvingen.ModuleId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Lastname", inschrijvingen.StudentId);
            return View(inschrijvingen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Beheerder")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ModuleId,StudentId,InschrijvingsDatum,AfgelegdOp,Resultaat")] Inschrijvingen inschrijvingen)
        {
            if (id != inschrijvingen.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inschrijvingen);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InschrijvingenExists(inschrijvingen.Id))
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
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Name", inschrijvingen.ModuleId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Lastname", inschrijvingen.StudentId);
            return View(inschrijvingen);
        }

        [Authorize(Roles = "Beheerder")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inschrijvingen = await _context.Inschrijvingen
                .Include(i => i.Module)
                .Include(i => i.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inschrijvingen == null)
            {
                return NotFound();
            }

            return View(inschrijvingen);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Beheerder")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inschrijvingen = await _context.Inschrijvingen.FindAsync(id);
            _context.Inschrijvingen.Remove(inschrijvingen);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InschrijvingenExists(int id)
        {
            return _context.Inschrijvingen.Any(e => e.Id == id);
        }
    }
}

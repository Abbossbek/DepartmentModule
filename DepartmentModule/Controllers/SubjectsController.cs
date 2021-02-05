using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DepartmentModule.Data;
using DepartmentModule.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DepartmentModule.Controllers
{
    [Authorize]

    public class SubjectsController : Controller
    {
        private readonly DepartmentModuleContext _context;
        IWebHostEnvironment _appEnvironment;

        public static Subject Current { get; private set; }
        public static bool AddingNew { get; private set; }


        public SubjectsController(DepartmentModuleContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // GET: Subjects
        public async Task<IActionResult> Index()
        {
            return View(await _context.Subject
                .Include("Program")
                .Include("AdditionalLiteratures")
                .Include("Themes")
                .Include("Literatures")
                .Include("User")
                .Where(x => x.User.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                .ToListAsync());
        }

        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subject
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subjects/Create
        public IActionResult Create()
        {
            AddingNew = true;
            Current = new Subject();
            Current.AdditionalLiteratures = new List<Book>();
            Current.Literatures = new List<Book>();

            return View(Current);
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                Current.Name = subject.Name;
                Current.User = _context.Users.Find(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                _context.Add(Current);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Index", 
                await _context.Subject
                .Include("Program")
                .Include("AdditionalLiteratures")
                .Include("Themes")
                .Include("Literatures")
                .Include("User")
                .Where(x => x.User.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                .ToListAsync());
        }
        // GET: Subjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            AddingNew = false;
            if (id == null)
            {
                return NotFound();
            }
            var subject = _context.Subject
                .Include("Program")
                .Include("AdditionalLiteratures")
                .Include("Themes")
                .Include("Literatures")
                .Include("User")
                .Where(x => x.User.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                .ToList().Find(x=>x.Id==id);
            if (subject == null)
            {
                return NotFound();
            }
            Current = subject;
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Subject subject)
        {
            if (id != subject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Current.Name = subject.Name;
                    _context.Update(Current);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.Id))
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
            return View(Current);
        }

        // GET: Subjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subject
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await _context.Subject.FindAsync(id);
            _context.Subject.Remove(subject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(int id)
        {
            return _context.Subject.Any(e => e.Id == id);
        }
        public async Task<IActionResult> Select(int? id)
        {
            
            if (id == null || Current == null)
            {
                return NotFound();
            }
            var book = await _context.Book.FindAsync(id);
            switch (BooksController.BookType)
            {
                case BookType.Program:
                    Current.Program = book;
                    break;
                case BookType.Themes:
                    Current.Themes = book;
                    break;
                case BookType.Literature:
                    Current.Literatures.Add(book);
                    break;
                case BookType.AdditionalLiterature:
                    Current.AdditionalLiteratures.Add(book);
                    break;
                default:
                    break;
            }
            if (AddingNew)
                return View("Create", Current);
            else
                return View("Edit", Current);
        }
        public async Task<IActionResult> RemoveLiterature(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Current.Literatures.Remove(Current.Literatures.First(x => x.Id == id));
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> RemoveAdditionalLiterature(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Current.AdditionalLiteratures.Remove(Current.AdditionalLiteratures.First(x => x.Id == id));
            return RedirectToAction(nameof(Index));
        }
    }
}

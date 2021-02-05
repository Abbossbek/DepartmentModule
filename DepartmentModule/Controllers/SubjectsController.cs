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

namespace DepartmentModule.Controllers
{

    public class SubjectsController : Controller
    {
        private readonly DepartmentModuleContext _context;
        IWebHostEnvironment _appEnvironment;

        public static Subject Current { get; private set; }

        public SubjectsController(DepartmentModuleContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // GET: Subjects
        public async Task<IActionResult> Index()
        {
            return View(await _context.Subject.Include("Program").Include("AdditionalLiteratures").Include("Themes").Include("Literatures").ToListAsync());
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
            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Subject subject, [Bind("programFile")]IFormFile programFile)
        {
            if (ModelState.IsValid)
            {
                subject.Program = Upload(programFile);
                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        public Book Upload(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    uploadedFile.CopyTo(fileStream);
                }
                return new Book { Name = uploadedFile.FileName, Url = path };
            }
            return null;
        }
        // GET: Subjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }
            var subject = _context.Subject
                .Include("Program")
                .Include("AdditionalLiteratures")
                .Include("Themes")
                .Include("Literatures").ToList().Find(x=>x.Id==id);
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
        public async Task<IActionResult> AddProgram()
        { 
            View("Edit", Current);

            return View("Index");
        }

    }
}

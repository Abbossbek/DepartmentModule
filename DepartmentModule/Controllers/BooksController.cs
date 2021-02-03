using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DepartmentModule.Data;
using DepartmentModule.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace DepartmentModule.Controllers
{
    public class BooksController : Controller
    {
        private readonly DepartmentModuleContext _context;
        IWebHostEnvironment _appEnvironment;

        public static BookType BookType { get; private set; }

        public BooksController(DepartmentModuleContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }
        
        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await _context.Book.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        List<Book> books = new List<Book>()
        {
            new Book(){Id=1, Name="Fizika",Url="Hksachkas"},
            new Book(){Id=1, Name="Fizika",Url="Hksachkas"},
            new Book(){Id=1, Name="Fizika",Url="Hksachkas"}
        };
        List<Book> books2 = new List<Book>()
        {
            new Book(){Id=1, Name="Matematika",Url="Hksachkassgseg eg seg"},
            new Book(){Id=1, Name="English",Url="Hksachkassgsdgsdg "},
            new Book(){Id=1, Name="C#",Url="Hksachkas sdvsd seg"}
        };
        public async Task<IActionResult> Save(int? Id)
        {
            return View();
        }
        public async Task<IActionResult> GoogleSearch()
        {
            ViewBag.fun = books2;
            return View("Create");
        }
        public IActionResult Create()
        {
            ViewBag.fun = books;
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Url")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Url")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            _context.Book.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
        
        public async  Task<IActionResult> AddLiterature()
        {
            BookType = BookType.Literature;
            return View("Select", await _context.Book.Where(x =>
            !SubjectsController.Current.Literatures.Contains(x)).ToListAsync());
        }
        public async Task<IActionResult> AddAdditionallLiterature()
        {
            BookType = BookType.AdditionalLiterature;
            return View("Select", await _context.Book.Where(x=>
            !SubjectsController.Current.AdditionalLiteratures.Contains(x)).ToListAsync());
        }
        public async Task<IActionResult> AddProgram()
        {
            BookType = BookType.Program;
            return View("Select", await _context.Book.ToListAsync());
        }
        public async Task<IActionResult> AddThemes()
        {
            BookType = BookType.Themes;
            return View("Select", await _context.Book.ToListAsync());
        }
    }
}

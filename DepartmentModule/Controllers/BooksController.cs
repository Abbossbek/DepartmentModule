﻿using System;
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
using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Net;
using HtmlAgilityPack;
using System.Web;
using System.Security.Claims;

namespace DepartmentModule.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly DepartmentModuleContext _context;
        IWebHostEnvironment _appEnvironment;
        List<Book> books;
        private static bool IsSelecting { get; set; } = false;
        public static BookType BookType { get; private set; }

        public BooksController(DepartmentModuleContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }
        
        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await _context.Book
                .Where(x=>x.UserID == User.FindFirst(ClaimTypes.NameIdentifier).Value && !x.Deleted).ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        public async Task<IActionResult> Save([Bind("Id, Name, Url")]Book book)
        {
            //Book book = books.First(x=>x.Id == id);
                book.UserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                _context.Add(book);
                await _context.SaveChangesAsync();
            if (BookType == BookType.AdditionalLiterature || BookType == BookType.Literature)
                return View("Index", GetBooksForList());
            else
                return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> GoogleSearch([Bind("query")]string query, [Bind("type")]string type)
        {
            if(books == null)
            {
                books = new List<Book>();
            }
            books.Clear();
            StringBuilder sb = new StringBuilder();
            byte[] ResultsBuffer = new byte[8192];
            string SearchResults = "http://google.com/search?q=" + query.Trim() + " filetype:"+type;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SearchResults);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream resStream = response.GetResponseStream();
            string tempString = null;
            int count = 0;
            do
            {
                count = resStream.Read(ResultsBuffer, 0, ResultsBuffer.Length);
                if (count != 0)
                {
                    tempString = Encoding.UTF8.GetString(ResultsBuffer, 0, count);
                    sb.Append(tempString);
                }
            }

            while (count > 0);
            string sbb = sb.ToString();

            HtmlDocument html = new HtmlDocument();
            html.OptionOutputAsXml = true;
            html.LoadHtml(sbb);
            HtmlNode doc = html.DocumentNode;

            foreach (HtmlNode link in doc.SelectNodes("//a[@href]"))
            {
                //HtmlAttribute att = link.Attributes["href"];
                string hrefValue = link.GetAttributeValue("href", string.Empty);
                if (hrefValue.ToString().Contains("/url?q=")
                    && (hrefValue.ToString().ToUpper().Contains("HTTP://") || hrefValue.ToString().ToUpper().Contains("HTTPS://"))
                    && hrefValue.ToString().ToUpper().Contains("."+type))
                {
                    int index = hrefValue.IndexOf("&");
                    if (index > 0)
                    {
                        hrefValue = hrefValue.Substring(0, index);
                        var name = HttpUtility.HtmlDecode(link.InnerText);
                        books.Add(new Book() { Name = name.Remove(name.IndexOf('›')),
                            Url = hrefValue.Replace("/url?q=", "") });
                    }
                }
            }

            ViewBag.fun = books;
            return View("Create");
        }
        public IActionResult Create()
        {
            ViewBag.fun = new List<Book>();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Url")] Book book,[Bind("bookUrl")]IFormFile bookUrl)
        {
            if (ModelState.IsValid)
            {
                book = Upload(bookUrl, book.Name);
                book.UserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                _context.Add(book);
                await _context.SaveChangesAsync();
                if (BookType == BookType.AdditionalLiterature || BookType == BookType.Literature)
                    return View("Index", GetBooksForList());
                else
                    return View("Index");
            }
            return View(book);
        }
        public Book Upload(IFormFile uploadedFile, string name = null)
        {
            if (uploadedFile != null)
            {
                if (name == null)
                {
                    name = uploadedFile.FileName;
                }
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    uploadedFile.CopyTo(fileStream);
                }
                return new Book { Name = name, Url = path };
            }
            return null;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Url")] Book book)
        {
            if (id != book.BookID)
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
                    if (!BookExists(book.BookID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (BookType == BookType.AdditionalLiterature || BookType == BookType.Literature)
                    return View("Index", GetBooksForList());
                else
                    return View("Index");
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
                .FirstOrDefaultAsync(m => m.BookID == id);
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
            book.Deleted =true;
            await _context.SaveChangesAsync();
            if(BookType==BookType.AdditionalLiterature || BookType == BookType.Literature)
                return View("Index", GetBooksForList());
            else
                return View("Index");
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.BookID == id);
        }
        
        public async  Task<IActionResult> AddLiterature()
        {
            IsSelecting = true;
            BookType = BookType.Literature;
            return View("Index", GetBooksForList());
        }
        public async Task<IActionResult> AddAdditionallLiterature()
        {
            IsSelecting = true;
            BookType = BookType.AdditionalLiterature;
            return View("Index", GetBooksForList());
        }
        public async Task<IActionResult> AddProgram()
        {
            IsSelecting = true;
            BookType = BookType.Program;
            return View("Index", await _context.Book
                .Where(x => x.UserID == User.FindFirst(ClaimTypes.NameIdentifier).Value && !x.Deleted)
                .ToListAsync());
        }
        public async Task<IActionResult> AddThemes()
        {
            IsSelecting = true;
            BookType = BookType.Themes;
            return View("Index", await _context.Book
                .Where(x => x.UserID == User.FindFirst(ClaimTypes.NameIdentifier).Value && !x.Deleted)
                .ToListAsync());
        }
        private List<Book> GetBooksForList()
        {
            var books = _context.Book.Where(x => x.UserID == User.FindFirst(ClaimTypes.NameIdentifier).Value && !x.Deleted).ToList();
            books = books.Where(b => SubjectsController.Current.Literatures.FirstOrDefault(l => l.Literature.BookID == b.BookID) == null).ToList();
            return books.Where(b => SubjectsController.Current.AdditionalLiteratures.FirstOrDefault(l => l.AdditionalLiterature.BookID == b.BookID) == null).ToList();
        }
       
    }
}

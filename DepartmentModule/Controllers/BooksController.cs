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

namespace DepartmentModule.Controllers
{
    [Authorize]
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

        public async Task<IActionResult> Save(int? Id)
        {
            return View();
        }
        public async Task<IActionResult> GoogleSearch([Bind("query")]string query, [Bind("type")]string type)
        {
            List<Book> books = new List<Book>();
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
                    && hrefValue.ToString().ToUpper().Contains(".PDF"))
                {
                    int index = hrefValue.IndexOf("&");
                    if (index > 0)
                    {
                        hrefValue = hrefValue.Substring(0, index);
                        books.Add(new Book() { Name = HttpUtility.HtmlDecode(link.InnerText).Remove(HttpUtility.HtmlDecode(link.InnerText).IndexOf('>')),
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

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Url")] Book book,[Bind("bookUrl")]IFormFile bookUrl)
        {
            if (ModelState.IsValid)
            {
                Book _book = new Book();
                _book = book;
                book = Upload(bookUrl);
                book.Name = _book.Name;
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
            return View("Index", await _context.Book.Where(x =>
            !SubjectsController.Current.Literatures.Contains(x)).ToListAsync());
        }
        public async Task<IActionResult> AddAdditionallLiterature()
        {
            BookType = BookType.AdditionalLiterature;
            return View("Index", await _context.Book.Where(x=>
            !SubjectsController.Current.AdditionalLiteratures.Contains(x)).ToListAsync());
        }
        public async Task<IActionResult> AddProgram()
        {
            BookType = BookType.Program;
            return View("Index", await _context.Book.ToListAsync());
        }
        public async Task<IActionResult> AddThemes()
        {
            BookType = BookType.Themes;
            return View("Index", await _context.Book.ToListAsync());
        }
    }
}

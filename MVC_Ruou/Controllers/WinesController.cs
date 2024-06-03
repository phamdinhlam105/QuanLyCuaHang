using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MVC_Ruou.Data;
using MVC_Ruou.Models;

namespace MVC_Ruou.Controllers
{
    public class WinesController : Controller
    {
        private readonly MVC_RuouContext _context;
        private readonly IWebHostEnvironment _environment;

        public WinesController(MVC_RuouContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Wines
        public async Task<IActionResult> Index(string chosenCategory, string searchString)
        {
            if ((HttpContext.Session.GetString("username") != "admin") && (HttpContext.Session.GetString("password") != "admin"))
                return RedirectToAction("Index", "Home");
            IQueryable<string> categoryQuery = from b in _context.Wine
                                               orderby b.CategoryName
                                               select b.CategoryName;
            var findwine = from b in _context.Wine
                        select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                findwine = findwine.Where(s => s.Name!.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(chosenCategory))
            {
                findwine = findwine.Where(x => x.CategoryName == chosenCategory);
            }
            ViewBag.ShowWines = true;
            ViewBag.ShowReceipts = true;
            ViewBag.ShowOrders = true;
            ViewBag.ShowLogout = true;
            var categoryVM = new CategoryNameViewModel
            {
                categoryName = new SelectList(await categoryQuery.Distinct().ToListAsync()),
                wines = await findwine.ToListAsync()
            };
            return View(categoryVM);
        }

        // GET: Wines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wine = await _context.Wine
                .FirstOrDefaultAsync(m => m.ID == id);
            if (wine == null)
            {
                return NotFound();
            }
            var imagePath = Path.Combine(_environment.WebRootPath, "assets", "wines");
            var uniqueFileName = $"{wine.ID}.*";
            // Kiểm tra xem tệp ảnh có tồn tại không
            var files = Directory.GetFiles(imagePath, uniqueFileName);
            if (files.Length > 0)
            {
                var fullFileName = Path.GetFileName(files[0]);
                ViewData["ImageUrl"] = fullFileName; // Lưu URL vào ViewData
            }
           
            return View(wine);
        }

        // GET: Wines/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Wines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,CategoryName,inputPrice,outputPrice,Amount,ImageFile")] Wine wine)
        {
            if (ModelState.IsValid)
            {
                if (wine.ImageFile != null && wine.ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "assets", "wines");
                    var uniqueFileName = $"{wine.ID}{Path.GetExtension(wine.ImageFile.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await wine.ImageFile.CopyToAsync(fileStream);
                    }
                    ViewData["ImageUrl"] = uniqueFileName; // Lưu URL vào ViewData
                }

                _context.Add(wine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wine);
        }
        
        // GET: Wines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wine = await _context.Wine.FindAsync(id);
            if (wine == null)
            {
                return NotFound();
            }
            var imagePath = Path.Combine(_environment.WebRootPath, "assets", "wines");
            var uniqueFileName = $"{wine.ID}.*";
            // Kiểm tra xem tệp ảnh có tồn tại không
            var files = Directory.GetFiles(imagePath, uniqueFileName);
            if (files.Length > 0)
            {
                var fullFileName = Path.GetFileName(files[0]);
                ViewData["ImageUrl"] = fullFileName; // Lưu URL vào ViewData
            }

            return View(wine);
        }

        // POST: Wines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,CategoryName,inputPrice,outputPrice,Amount,ImageFile")] Wine wine)
        {
            if (id != wine.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (wine.ImageFile != null && wine.ImageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(_environment.WebRootPath, "assets", "wines");
                        var uniqueFileName = $"{id}{Path.GetExtension(wine.ImageFile.FileName)}";
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        var existingImagePath = Path.Combine(_environment.WebRootPath, "assets", "wines", $"{id}.*");
                        var existingImageFiles = Directory.GetFiles(Path.GetDirectoryName(existingImagePath), Path.GetFileName(existingImagePath));

                        if (existingImageFiles.Length > 0)
                            foreach (var existingImageFile in existingImageFiles)
                            {
                                // Xóa ảnh đã tồn tại
                                System.IO.File.Delete(existingImageFile);
                            }
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await wine.ImageFile.CopyToAsync(fileStream);
                        }

                        ViewData["ImageUrl"] = filePath; // Lưu URL vào ViewData
                    }
                    _context.Update(wine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WineExists(wine.ID))
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
            return View(wine);
        }

        // GET: Wines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wine = await _context.Wine
                .FirstOrDefaultAsync(m => m.ID == id);
            if (wine == null)
            {
                return NotFound();
            }

            return View(wine);
        }

        // POST: Wines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wine = await _context.Wine.FindAsync(id);
            if (wine != null)
            {
                _context.Wine.Remove(wine);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WineExists(int id)
        {
            return _context.Wine.Any(e => e.ID == id);
        }
    }
}

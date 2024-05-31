using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Ruou.Data;
using MVC_Ruou.Models;

namespace MVC_Ruou.Controllers
{
    public class WinesController : Controller
    {
        private readonly MVC_RuouContext _context;

        public WinesController(MVC_RuouContext context)
        {
            _context = context;
        }

        // GET: Wines
        public async Task<IActionResult> Index(string chosenCategory, string searchString,Login login)
        {

            //login.Check(login);
            //if (login.CheckValid() == 0)
                //return RedirectToAction("Index", "Wines");
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
        public async Task<IActionResult> Create([Bind("ID,Name,CategoryName,inputPrice,outputPrice,Amount")] Wine wine)
        {
            if (ModelState.IsValid)
            {
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
            return View(wine);
        }

        // POST: Wines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,CategoryName,inputPrice,outputPrice,Amount")] Wine wine)
        {
            if (id != wine.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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

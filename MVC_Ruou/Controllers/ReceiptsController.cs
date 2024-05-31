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
    public class ReceiptsController : Controller
    {
        private readonly MVC_RuouContext _context;

        public ReceiptsController(MVC_RuouContext context)
        {
            _context = context;
        }

        // GET: Receipts
        public async Task<IActionResult> Index(string chosenUser,string searchString)
        {
            IQueryable<string> categoryQuery = from b in _context.Receipt
                                               orderby b.UserName
                                               select b.UserName;
            var findreceipt = from b in _context.Receipt
                           select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                findreceipt = findreceipt.Where(s => s.ReceiptID!.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(chosenUser))
            {
                findreceipt = findreceipt.Where(x => x.UserName == chosenUser);
            }

            var receiptVM = new ReceiptVM
            {
                user = new SelectList(await categoryQuery.Distinct().ToListAsync()),
                receipts = await findreceipt.ToListAsync()
            };
            return View(receiptVM);
        }

        // GET: Receipts/Details/5
        public async Task<IActionResult> Details(string? id)
        {
           
            if (id == null)
            {
                return NotFound();
            }
            ReceiptVM receiptVM = new ReceiptVM
            {
                receipt = await _context.Receipt.FirstOrDefaultAsync(m => m.ReceiptID == id),
                receiptdetails = await _context.ReceiptDetail.Where(a => a.ReceiptID == id).ToListAsync()
            };
            if (receiptVM == null)
            {
                return NotFound();
            }

            return View(receiptVM);
        }

        // GET: Receipts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Receipts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID","UserName","Quantity","Total","CreateDate")] Receipt receipt)
        {
            //auto generate ID max to 99
            var receiptID = "IMP";
            int count = _context.Receipt.Count();
            if (count < 10)
                receiptID += "00" + (count + 1).ToString();
            else
                if (count < 100)
                receiptID += "0" + (count + 1).ToString();
            receipt.ReceiptID = receiptID;
            //create date = current date
            DateTime currentdate = DateTime.Now;
            receipt.CreateDate = currentdate;
            receipt.Quantity = 0;
            receipt.Total = 0;
            _context.Add(receipt);
            await _context.SaveChangesAsync();
            return RedirectToAction("Create", "ReceiptDetails", new { receiptID = receipt.ReceiptID });
        }

        // GET: Receipts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receipt = await _context.Receipt.FindAsync(id);
            if (receipt == null)
            {
                return NotFound();
            }
            return View(receipt);
        }

        // POST: Receipts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ReceiptID,UserName,Quantity,Total,CreateDate")] Receipt receipt)
        {
            if (id != receipt.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receipt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceiptExists(receipt.ID))
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
            return View(receipt);
        }

        // GET: Receipts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receipt = await _context.Receipt
                .FirstOrDefaultAsync(m => m.ID == id);
            if (receipt == null)
            {
                return NotFound();
            }

            return View(receipt);
        }

        // POST: Receipts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receipt = await _context.Receipt.FindAsync(id);
            if (receipt != null)
            {
                _context.Receipt.Remove(receipt);
            }
            List<ReceiptDetail> receiptdetails = await _context.ReceiptDetail.Where(a => a.ReceiptID == receipt.ReceiptID).ToListAsync();
            if(receiptdetails!=null)
            {
                _context.ReceiptDetail.RemoveRange(receiptdetails);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReceiptExists(int id)
        {
            return _context.Receipt.Any(e => e.ID == id);
        }
    }
}

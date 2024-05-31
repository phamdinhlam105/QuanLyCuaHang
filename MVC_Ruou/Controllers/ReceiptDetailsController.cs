using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Ruou.Data;
using MVC_Ruou.Migrations;
using MVC_Ruou.Models;
using ReceiptDetail = MVC_Ruou.Models.ReceiptDetail;

namespace MVC_Ruou.Controllers
{
    public class ReceiptDetailsController : Controller
    {
        private readonly MVC_RuouContext _context;

        public ReceiptDetailsController(MVC_RuouContext context)
        {
            _context = context;
        }

        // GET: ReceiptDetails
        public async Task<IActionResult> Index()
        {
            return View(await _context.ReceiptDetail.ToListAsync());
        }

        // GET: ReceiptDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receiptDetail = await _context.ReceiptDetail
                .FirstOrDefaultAsync(m => m.ID == id);
            if (receiptDetail == null)
            {
                return NotFound();
            }

            return View(receiptDetail);
        }

        // GET: ReceiptDetails/Create
        public IActionResult Create(string receiptID)
        {
            IQueryable<string> productlistQuery = from b in _context.Wine
                                               orderby b.Name
                                               select b.Name;
            var receiptdetailVM = new ReceiptDetailVM()
            {
                receiptdetails = new List<ReceiptDetail>(),
                chosenreceipt = receiptID,
                chosenproduct = new ReceiptDetail() { },
                productname = new SelectList(productlistQuery.Distinct().ToList())
            };
            if (receiptdetailVM.receiptdetails == null)
            {
                receiptdetailVM.receiptdetails = new List<ReceiptDetail>(); 
            }
            receiptdetailVM.chosenproduct.ReceiptID = receiptID;
            return View(receiptdetailVM);
        }

        // POST: ReceiptDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("chosenproduct","chosenreceipt")]ReceiptDetailVM receiptdetailVM)
        {
            IQueryable<string> productlistQuery = from b in _context.Wine
                                               orderby b.Name
                                               select b.Name;
            receiptdetailVM.productname = new SelectList(await productlistQuery.Distinct().ToListAsync());
            var receiptdetaillist = from b in _context.ReceiptDetail
                                    select b;
            if (!String.IsNullOrEmpty(receiptdetailVM.chosenreceipt))
            {
                receiptdetailVM.receiptdetails = await receiptdetaillist.Where(s => s.ReceiptID == receiptdetailVM.chosenreceipt).ToListAsync();
                receiptdetailVM.receiptdetails.Add(receiptdetailVM.chosenproduct);
            }

            var chosenwine = await _context.Wine.Where(s => s.Name == receiptdetailVM.chosenproduct.Name).FirstOrDefaultAsync();
            receiptdetailVM.chosenproduct.inputPrice = chosenwine.inputPrice;
            receiptdetailVM.chosenproduct.outputPrice = chosenwine.outputPrice;

            if (ModelState.IsValid)
            {
                _context.Add(receiptdetailVM.chosenproduct);
                await _context.SaveChangesAsync();
                return View(receiptdetailVM);
            }
            return View(receiptdetailVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("UpdateToDatabase")]
        public async Task<IActionResult> UpdateToDatabase(string chosenreceiptID)
        {
            Receipt chosenreceipt = await _context.Receipt.FirstOrDefaultAsync(a => a.ReceiptID == chosenreceiptID);
            List<ReceiptDetail> receiptdetails = await _context.ReceiptDetail.Where(a => a.ReceiptID == chosenreceipt.ReceiptID).ToListAsync();

            if (receiptdetails != null)
            {
                decimal total = 0;
                int? quantity = 0;
                foreach (var item in receiptdetails)
                {
                    var wine = await _context.Wine.FirstOrDefaultAsync(w => w.Name == item.Name);
                    if (wine != null)
                    {
                        wine.Amount += item.Amount;
                        _context.Wine.Update(wine);
                    }
                    quantity += item.Amount;
                    total += wine.inputPrice * (decimal)item.Amount;
                }

                var receipt = await _context.Receipt.FirstOrDefaultAsync(w => w.ReceiptID == chosenreceipt.ReceiptID);
                if (receipt != null)
                {
                    receipt.Quantity = (int)quantity;
                    receipt.Total = total;
                    _context.Receipt.Update(receipt);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Receipts", new { id = chosenreceipt.ReceiptID });
            }
            return NotFound();
        }


        // GET: ReceiptDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receiptDetail = await _context.ReceiptDetail.FindAsync(id);
            if (receiptDetail == null)
            {
                return NotFound();
            }
            return View(receiptDetail);
        }

        // POST: ReceiptDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ReceiptID,Name,Amount,inputPrice,outputPrice")] ReceiptDetail receiptDetail)
        {
            if (id != receiptDetail.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receiptDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceiptDetailExists(receiptDetail.ID))
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
            return View(receiptDetail);
        }

        // GET: ReceiptDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receiptDetail = await _context.ReceiptDetail
                .FirstOrDefaultAsync(m => m.ID == id);
            if (receiptDetail == null)
            {
                return NotFound();
            }

            return View(receiptDetail);
        }

        // POST: ReceiptDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receiptDetail = await _context.ReceiptDetail.FindAsync(id);
            if (receiptDetail != null)
            {
                _context.ReceiptDetail.Remove(receiptDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReceiptDetailExists(int id)
        {
            return _context.ReceiptDetail.Any(e => e.ID == id);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MVC_Ruou.Data;
using MVC_Ruou.Models;
using System.Diagnostics;

namespace MVC_Ruou.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MVC_RuouContext _context;
        public HomeController(ILogger<HomeController> logger, MVC_RuouContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(string chosenCategory, string searchString, Login login)
        {
            login.Check(login);
            if (login.CheckValid()==1)
                return RedirectToAction("Index", "Wines");
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
        public async Task<IActionResult> Order()
        {
            var order = new Order();
            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order([Bind("customerName,customerPhone")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.total = 0;
                order.status = 0;
                _context.Add(order);
                await _context.SaveChangesAsync();
                order.Id = (await _context.Order.ToListAsync())[_context.Order.Count()-1].Id;
                return RedirectToAction("OrderDetail", "Home", new { idorder = order.Id});
            }
                return View(order);
        }

        public async Task<IActionResult> OrderDetail(int idorder)
        {
            IQueryable<string> productlistQuery = from b in _context.Wine
                                                  orderby b.Name
                                                  select b.Name;
            var orderdetailVM = new OrderVM()
            {
                orderdetail = new List<OrderDetail>(),
                idchosenorder = idorder,
                chosenorder = new OrderDetail() { IdOrder= idorder },
                productname = new SelectList(productlistQuery.Distinct().ToList())
            };
            if (orderdetailVM.orderdetail == null)
            {
                orderdetailVM.orderdetail = new List<OrderDetail>();
            }
            return View(orderdetailVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderDetail([Bind("chosenorder", "idchosenorder")] OrderVM orderVM)
        {
            IQueryable<string> productlistQuery = from b in _context.Wine
                                                  orderby b.Name
                                                  select b.Name;
            orderVM.productname = new SelectList(await productlistQuery.Distinct().ToListAsync());
            var orderdetaillist = from b in _context.OrderDetail
                                  select b;
           

            var chosenwine = await _context.Wine.Where(s => s.Name == orderVM.chosenorder.WineName).FirstOrDefaultAsync();
            orderVM.chosenorder.price = chosenwine.outputPrice;
            orderVM.chosenorder.IdWine = chosenwine.ID;
            orderVM.chosenorder.IdOrder = orderVM.idchosenorder;
            orderVM.total += orderVM.chosenorder.price * orderVM.chosenorder.Amount;

            if (!String.IsNullOrEmpty(orderVM.idchosenorder.ToString()))
            {
                orderVM.orderdetail = await orderdetaillist.Where(s => s.IdOrder == orderVM.idchosenorder).ToListAsync();
                orderVM.orderdetail.Add(orderVM.chosenorder);
            }
            if (ModelState.IsValid)
            {
                _context.Add(orderVM.chosenorder);
                await _context.SaveChangesAsync();
                return View(orderVM);
            }
            return View(orderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("FinishOrder")]
        public async Task<IActionResult> FinishOrder(int idchosenorder, decimal total)
        {
            Order chosenorder  = await _context.Order.FirstOrDefaultAsync(a => a.Id == idchosenorder);
          
            if (chosenorder!=null)
            {
               
                    chosenorder.total = total;
                    _context.Order.Update(chosenorder);
                await _context.SaveChangesAsync();
                return RedirectToAction("ReviewOrder", "Home", new { id = idchosenorder });
            }
            else
            {
                _context.Order.Remove(chosenorder);
                await _context.SaveChangesAsync();
                
                return NotFound();
            }
            
        }

        public async Task<IActionResult> ReviewOrder(int id)
        {
            var orderdetaillist = from b in _context.OrderDetail
                                  select b;

            ReviewOrder revieworder = new()
            {
                order = await _context.Order.FirstOrDefaultAsync(w => w.Id == id),
                orderdetails = await orderdetaillist.Where(s => s.IdOrder == id).ToListAsync()
            };
            return View(revieworder);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

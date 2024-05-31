﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Ruou.Data;
using MVC_Ruou.Models;

namespace MVC_Ruou.Controllers
{
    public class OrdersController : Controller
    {
        private readonly MVC_RuouContext _context;
        public OrdersController(MVC_RuouContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string searchString, Login login)
        {
            if (login == null)
                return RedirectToAction("Index", "Home");
            var findorder = from b in _context.Order
                           select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                findorder = findorder.Where(s => s.customerName!.Contains(searchString));
            }
           
            var orderlistVM = new OrderListVM
            {
                orders = await findorder.ToListAsync()
            };
            return View(orderlistVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int id)
        {
            var orderlistVM = new OrderListVM
            {
                orders = await _context.Order.ToListAsync()
            };
            if (!String.IsNullOrEmpty(id.ToString()))
            {
                
                var order = await _context.Order.FirstOrDefaultAsync(w => w.Id == id);
                order.status = 1;
                _context.Order.Update(order);
                await _context.SaveChangesAsync();
                orderlistVM.orders = await _context.Order.ToListAsync();
                return View(orderlistVM);
            }
            return View(orderlistVM);
        }
    }
}

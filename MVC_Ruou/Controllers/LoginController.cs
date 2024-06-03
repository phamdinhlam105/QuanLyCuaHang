using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Ruou.Models;

namespace MVC_Ruou.Controllers
{
    public class LoginController : Controller
    {
        public async Task<IActionResult> Index()
        {
            Login model = new Login();
            ViewBag.ShowLogin = true;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("username,password")] Login login)
        {
            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("username",login.username);
                HttpContext.Session.SetString("password", login.username);
                login.Check(login);
                if (login.CheckValid() == 1)
                    return RedirectToAction("Index", "Wines");
                else
                    ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu(admin)");
            }       
            return View(login);
        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
        }
    }
}

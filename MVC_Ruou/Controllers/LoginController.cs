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
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("username,password")] Login login)
        {
            if (ModelState.IsValid)
            {
                login.Check(login);
                if (login.CheckValid() == 1)
                    return RedirectToAction("Index", "Wines", new {login = login});
            }
            return View(login);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(Login login)
        {
            login = null;
            return RedirectToAction("Index","Home");
        }
    }
}

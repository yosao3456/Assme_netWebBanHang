using Assm_KiemThuNangCao.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Assm_KiemThuNangCao.Controllers
{
    public class AccountController : Controller
    {

        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;  
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(Customer model)
        {
            _context.Customers.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }
[HttpPost]
public IActionResult Login(string email, string password)
{
    var user = _context.Customers
                       .FirstOrDefault(x => x.Email == email && x.Password == password);

    if (user == null)
    {
        ViewBag.Error = "Thông tin không chính xác!!!";
        return View();
    }

    CookieOptions option = new CookieOptions
    {
        Expires = DateTime.Now.AddDays(7)
    };

    // CHỈ DÙNG 1 COOKIE DUY NHẤT
    Response.Cookies.Append("UserEmail", user.Email, option);

    return RedirectToAction("Index", "Home");
}

public IActionResult Logout()
{
    // XÓA ĐÚNG COOKIE
    Response.Cookies.Delete("UserEmail");
    return RedirectToAction("Index", "Home");
}
    }
}

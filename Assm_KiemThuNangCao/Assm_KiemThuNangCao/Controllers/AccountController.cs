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

    // Lưu tên người dùng vào Cookie
    CookieOptions option = new CookieOptions
    {
        Expires = DateTime.Now.AddDays(7)
    };

    Response.Cookies.Append("FullName", user.FullName, option);

    return RedirectToAction("Index", "Home");
}
public IActionResult Logout()
{
    Response.Cookies.Delete("FullName");
    return RedirectToAction("Index", "Home");
}
    }
}

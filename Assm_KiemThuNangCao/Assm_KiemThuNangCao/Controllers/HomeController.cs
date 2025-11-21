using Assm_KiemThuNangCao.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Assm_KiemThuNangCao.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;  
        }
        public IActionResult Index()
        {
            try
            {
                // Thử truy vấn database để kiểm tra kết nối
                var count = _context.Products.Count();
                ViewBag.Message = $"✅ Kết nối thành công! Có {count} sản phẩm trong database.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"❌ Kết nối thất bại: {ex.Message}";
            }

            return View();
            
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Header()
        {
            return View();
        }
        public IActionResult footer()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

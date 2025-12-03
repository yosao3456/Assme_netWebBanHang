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
        public IActionResult TimSanPhamTheoMucLuc(int categoryId)
        {
            var products = _context.Products
                                   .Where(p => p.CategoryID == categoryId)
                                   .ToList();

            var category = _context.Categories.FirstOrDefault(c => c.CategoryID == categoryId);
            ViewBag.CategoryName = category?.CategoryName ?? "Sản phẩm";

            return View(products);
        }
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            var products = _context.Products.ToList();

            ViewBag.Products = products;
            return View(categories);
            
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

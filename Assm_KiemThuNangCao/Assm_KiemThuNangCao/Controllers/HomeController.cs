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
        public IActionResult MucLucSanPham(int categoryId)
        {
            var products = _context.Products
                                   .Where(p => p.CategoryID == categoryId)
                                   .ToList();

            var category = _context.Categories.FirstOrDefault(c => c.CategoryID == categoryId);
            ViewBag.CategoryName = category?.CategoryName ?? "Sản phẩm";

            return View(products);
        }
public IActionResult Index(int page = 1)
{
    HttpContext.Session.Remove("Cart");

    var categories = _context.Categories.ToList();
    ViewBag.Categories = categories;

    // --------- PHÂN TRANG SẢN PHẨM ----------
    int pageSize = 10; 
    int totalProducts = _context.Products.Count();
    int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

var products = _context.Products
                    .OrderByDescending(p => p.ProductID)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

    ViewBag.Products = products;
    ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
    ViewBag.CurrentPage = page;
    ViewBag.CurrentPage = page;
    ViewBag.TotalPages = totalPages;
    return View(products);
}
        public IActionResult ProductDetail(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductID == id);

            if (product == null)
            {
                return RedirectToAction("Index");
            }
            var related = _context.Products
                         .Where(p => p.CategoryID == product.CategoryID && p.ProductID != id)
                         .Take(4) 
                         .ToList();
ViewBag.RelatedProducts = related;
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Header()
        {
            return View();
        }
        public IActionResult Search(string keyword)
{
    if (string.IsNullOrEmpty(keyword))
        return View(new List<Product>());

    keyword = keyword.ToLower();

    var results = _context.Products
        .Include(p => p.Category)
        .Where(p =>
            p.ProductName.ToLower().Contains(keyword) ||     // tìm theo tên
            p.Description.ToLower().Contains(keyword) ||     // tìm theo mô tả
            p.Color.ToLower().Contains(keyword) ||           // tìm theo màu
            p.Size.ToLower().Contains(keyword) ||            // tìm theo kích thước
            p.Category.CategoryName.ToLower().Contains(keyword) // tìm theo mục lục
        )
        .ToList();

    return View(results);
}
public IActionResult SearchSuggest(string keyword)
{
    if (string.IsNullOrEmpty(keyword))
        return Json(new List<string>());

    keyword = keyword.ToLower();

    var suggestions = _context.Products
        .Where(p => p.ProductName.ToLower().Contains(keyword))
        .Select(p => p.ProductName)
        .Distinct()
        .Take(8)
        .ToList();

    return Json(suggestions);
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

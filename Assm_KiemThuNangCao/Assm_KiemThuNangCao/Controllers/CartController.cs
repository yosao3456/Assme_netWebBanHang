using Assm_KiemThuNangCao.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assm_KiemThuNangCao.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        //========================= MUA NGAY ============================
        [HttpGet]
public IActionResult Buy(int id)
{
    // kiểm tra đăng nhập
    var email = Request.Cookies["UserEmail"];
    if (string.IsNullOrEmpty(email))
        return RedirectToAction("Login", "Account");

    var product = _context.Products.FirstOrDefault(x => x.ProductID == id);
    if (product == null)
        return RedirectToAction("Index", "Home");

    return View(product);
}
[HttpPost]
public IActionResult BuyConfirm(int productId, int quantity)
{
    var email = Request.Cookies["UserEmail"];
    if (string.IsNullOrEmpty(email))
        return RedirectToAction("Login", "Account");

    var user = _context.Customers.FirstOrDefault(x => x.Email == email);
    if (user == null)
        return RedirectToAction("Login", "Account");

    var cart = _context.Carts.FirstOrDefault(x => x.CustomerID == user.CustomerID);
    if (cart == null)
    {
        cart = new Cart
        {
            CustomerID = user.CustomerID,
            CreatedDate = DateTime.Now
        };
        _context.Carts.Add(cart);
        _context.SaveChanges();
    }

    var detail = _context.CartDetails.FirstOrDefault(
        x => x.CartID == cart.CartID && x.ProductID == productId);

    var product = _context.Products.FirstOrDefault(x => x.ProductID == productId);

    if (detail == null)
    {
        detail = new CartDetail
        {
            CartID = cart.CartID,
            ProductID = productId,
            Quantity = quantity,
            Price = product.Price
        };
        _context.CartDetails.Add(detail);
    }
    else
    {
        detail.Quantity += quantity;
    }

    _context.SaveChanges();

    TempData["Success"] = "Đặt hàng thành công!";
    return RedirectToAction("Index", "Cart");
}
        //========================= XEM GIỎ HÀNG ============================
        public IActionResult Index()
        {
            var email = Request.Cookies["UserEmail"];

            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Account");

            var user = _context.Customers.FirstOrDefault(x => x.Email == email);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var cart = _context.Carts.FirstOrDefault(x => x.CustomerID == user.CustomerID);
            if (cart == null)
                return View(new List<CartDetail>());


            var items = _context.CartDetails
                        .Where(x => x.CartID == cart.CartID)
                        .Include(x => x.Product)
                        .ToList();

            return View(items);
        }

        //========================= CẬP NHẬT SỐ LƯỢNG ============================
        [HttpPost]
        public IActionResult UpdateQuantity(int detailId, int quantity)
        {
            var detail = _context.CartDetails
                .Include(x => x.Product)
                .FirstOrDefault(x => x.CartDetailID == detailId);

            if (detail == null)
                return Json(new { success = false, message = "Không tìm thấy sản phẩm!" });

            if (quantity < 1)
                return Json(new { success = false, message = "Số lượng tối thiểu là 1!" });

            if (quantity > detail.Product.Quantity)
                return Json(new { success = false, message = "Vượt quá số lượng trong kho!" });

            detail.Quantity = quantity;
            _context.SaveChanges();

            return Json(new
            {
                success = true,
                subtotal = detail.Quantity * detail.Price
            });
        }

        //========================= XÓA SẢN PHẨM ============================
        [HttpPost]
        public IActionResult RemoveItem(int detailId)
        {
            var detail = _context.CartDetails.FirstOrDefault(x => x.CartDetailID == detailId);

            if (detail == null)
                return Json(new { success = false });

            _context.CartDetails.Remove(detail);
            _context.SaveChanges();

            return Json(new { success = true });
        }
[HttpPost]
public IActionResult Checkout()
{
    var email = Request.Cookies["UserEmail"];
    if (string.IsNullOrEmpty(email))
        return RedirectToAction("Login", "Account");

    var user = _context.Customers.FirstOrDefault(x => x.Email == email);
    if (user == null)
        return RedirectToAction("Login", "Account");

    var cart = _context.Carts.FirstOrDefault(x => x.CustomerID == user.CustomerID);
    if (cart == null)
    {
        TempData["Error"] = "Giỏ hàng của bạn đang trống!";
        return RedirectToAction("Index");
    }

    var details = _context.CartDetails
        .Include(x => x.Product)
        .Where(x => x.CartID == cart.CartID)
        .ToList();

    if (!details.Any())
    {
        TempData["Error"] = "Giỏ hàng của bạn đang trống!";
        return RedirectToAction("Index");
    }

    foreach (var item in details)
    {
        if (item.Quantity > item.Product.Quantity)
        {
            TempData["Error"] = $"Sản phẩm {item.Product.ProductName} không đủ số lượng!";
            return RedirectToAction("Index");
        }

        // TRỪ TỒN KHO
        item.Product.Quantity -= item.Quantity;
    }

    // XÓA GIỎ HÀNG SAU KHI ĐẶT
    _context.CartDetails.RemoveRange(details);
    _context.Carts.Remove(cart);

    _context.SaveChanges();

    TempData["Success"] = "Đặt hàng thành công!";

    return RedirectToAction("Index", "Home");
}

    }
}
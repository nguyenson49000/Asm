using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSonCoffee.Models;

namespace TSonCoffee.Controllers
{
    public class ProductTypeController : Controller
    {
        private readonly TsonCoffeeContext db;

        public ProductTypeController(TsonCoffeeContext db)
        {
            this.db = db;
        }
        public IActionResult Index(int id)
        {
            var products = db.Products
                             .Where(p => p.ProductTypeId == id)
                             .Include(p => p.Img) // Bao gồm thông tin ảnh
                             .ToList();

            // Nếu không có sản phẩm, chuyển hướng về trang chủ hoặc xử lý phù hợp
            if (products == null || !products.Any())
            {
                return RedirectToAction("Index", "Home");
            }

            return View(products);
        }
    }
}

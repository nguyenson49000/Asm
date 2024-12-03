using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSonCoffee.Models;

namespace TSonCoffee.Controllers
{
    public class ProductController : Controller
    {
        private readonly TsonCoffeeContext db;

        public ProductController(TsonCoffeeContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var sp = db.Products.Include(x => x.Img).ToList();
            return View(sp);
        }

        public IActionResult Details(int id)
        {
            // Lấy sản phẩm theo ID
            var product = db.Products
                            .Where(p => p.Id == id)
                            .Include(p => p.Img) // Bao gồm thông tin ảnh
                            .FirstOrDefault(); // Lấy sản phẩm duy nhất

            // Kiểm tra nếu không tìm thấy sản phẩm
            if (product == null)
            {
                return RedirectToAction("Index", "Home"); // Hoặc trả về trang lỗi nếu không tìm thấy
            }

            // Trả về sản phẩm đơn lẻ đến View
            return View(product); // Truyền đối tượng product duy nhất vào View
        }


    }
}

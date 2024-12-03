using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TSonCoffee.Models;

namespace TSonCoffee.Controllers
{
    public class AdminController : Controller
    {
        private readonly TsonCoffeeContext db;

        public AdminController(TsonCoffeeContext db)
        {
            this.db = db;
        }
        public IActionResult QuanLyUser()
        {
            var user = db.Users.ToList();
            return View(user);
        }
        public IActionResult EditUser(int id)
        {
            var user = db.Users.Find(id);
            return View(user);
        }
        [HttpPost]
        public IActionResult EditUser(User user)
        {
            db.Users.Update(user);
            db.SaveChanges();
            return RedirectToAction("QuanLyUser");
        }

        public IActionResult DetailsUser(int id)
        {
            var user = db.Users.Find(id);
            return View(user);
        }

        public IActionResult QuanLyHoaDon()
        {
            var user = db.Invoices.ToList();
            return View(user);
        }
        public IActionResult EditHoaDon(int id)
        {
            var user = db.Invoices.Find(id);
            return View(user);
        }
        [HttpPost]
        public IActionResult EditHoaDon(Invoice hd)
        {
            db.Invoices.Update(hd);
            db.SaveChanges();
            return RedirectToAction("QuanLyHoaDon");
        }

        public IActionResult DetailsHoaDon(int id)
        {
            var data = (from detail in db.DetailedInvoices
                        join product in db.Products on detail.ProductsId equals product.Id
                        join img in db.Images on product.ImgId equals img.Id
                        join ice in db.AmountOfIces on detail.AmountOfIceId equals ice.Id
                        join topping in db.Toppings on detail.ToppingId equals topping.Id
                        join sweet in db.Sweetnesses on detail.SweetnessId equals sweet.Id
                        join size in db.Sizes on detail.SizeId equals size.Id
                        join invoice in db.Invoices on detail.InvoicesId equals invoice.Id
                        where detail.InvoicesId == id  // Điều kiện lọc theo ID hóa đơn
                        select new DetailedInvoiceViewModel
                        {
                            InvoiceId = id,
                            Img = img.UrlImg,
                            Name = product.Name,
                            Size = size.Name,
                            Sweetness = sweet.Name,
                            AmontofIce = ice.Name,
                            Topping = topping.Name,
                            Quantity = detail.Quantity,
                            Price = detail.Price
                        }).ToList();

            // Trả về dữ liệu ra view
            return View(data);
        }


        public IActionResult QuanLySanPham()
        {
            var user = db.Products.Include(x => x.Img).Include(x => x.ProductType).ToList();
            return View(user);
        }
        public IActionResult EditSanPham(int id)
        {
            var user = db.Products.Find(id);
            var productTypes = db.ProductTypes.ToList();
            ViewBag.ProductTypes = new SelectList(productTypes, "Id", "Name");
            return View(user);
        }
        [HttpPost]
        public IActionResult EditSanPham(Product hd)
        {
            db.Products.Update(hd);
            db.SaveChanges();
            return RedirectToAction("QuanLySanPham");
        }

        public IActionResult CreateSanPham()
        {
            var productTypes = db.ProductTypes.ToList();
            ViewBag.ProductTypes = new SelectList(productTypes, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSanPham(Product sp, IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError("ImageFile", "Vui lòng chọn một hình ảnh.");
                return View(sp);
            }

            try
            {
                // Tạo tên file mà không thêm dấu gạch dưới và giá trị thời gian
                var fileName = Path.GetFileName(imageFile.FileName); // Giữ nguyên tên tệp gốc
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);

                // Kiểm tra nếu tệp đã tồn tại để tránh ghi đè
                if (System.IO.File.Exists(filePath))
                {
                    ModelState.AddModelError("ImageFile", "Hình ảnh đã tồn tại. Vui lòng chọn một tên khác.");
                    return View(sp);
                }

                // Lưu file vào thư mục images
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Tạo đối tượng Image với đường dẫn của hình ảnh
                Image img = new Image()
                {
                    UrlImg = fileName // Lưu đường dẫn tương đối để sử dụng trên website
                };

                db.Images.Add(img);
                await db.SaveChangesAsync();

                // Lưu thông tin sản phẩm với Id hình ảnh
                sp.ImgId = img.Id;
                db.Products.Add(sp);
                await db.SaveChangesAsync();

                return RedirectToAction("QuanLySanPham");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Có lỗi xảy ra: {ex.Message}");
                return View(sp);
            }
        }





    }
}

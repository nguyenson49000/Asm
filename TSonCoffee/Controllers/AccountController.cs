using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Net.Http.Headers;
using TSonCoffee.Models;

namespace TSonCoffee.Controllers
{
    public class AccountController : Controller
    {
        private readonly TsonCoffeeContext db;

        public AccountController(TsonCoffeeContext db)
        {
            this.db = db;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string pass)
        {
            if (username == null || pass == null)
            {
                return View(); // Nếu không nhập thông tin, giữ nguyên view đăng nhập
            }

            // Tìm kiếm tài khoản trong CSDL
            var acc = db.Users.ToList().FirstOrDefault(x => x.UserName == username && x.PassWord == pass);
            if (acc == null) // Nếu không tìm thấy tài khoản
            {
                TempData["Status"] = "Đăng nhập không thành công. Mời kiểm tra lại tài khoản và mật khẩu!";
                return RedirectToAction("Login"); // Trở lại trang đăng nhập và hiển thị thông báo
            }
            else // Nếu tìm thấy tài khoản
            {
                HttpContext.Session.SetString("Account", username);
                if (acc.Role != null)
                {
                    HttpContext.Session.SetString("Role", acc.Role);
                }
                else
                {
                    HttpContext.Session.SetString("Role", "DefaultRole"); // Giá trị mặc định nếu Role là null
                }

                TempData["Status"] = $"Đăng nhập thành công chào mừng {acc.FullName} đến với TSonCoffee";

                return RedirectToAction("Index", "Home"); // Chuyển hướng tới trang chính
            }
        }

        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignIn(User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Lấy tất cả lỗi trong ModelState
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    TempData["Status"] = "Dữ liệu không hợp lệ: " + string.Join(", ", errors);
                    return View(user);
                }

                // Thiết lập các giá trị mặc định
                if (string.IsNullOrEmpty(user.Role))
                {
                    user.Role = "user"; // Gán giá trị mặc định cho Role
                }
                var maxId = db.Users.Max(u => u.Id);

                // Thiết lập giá trị mặc định cho user.Id (ID mới phải lớn hơn maxId)
                user.Id = maxId + 1;
                user.PurchaseCount = 0;
                user.Status = true;
                user.CreateAt ??= DateOnly.FromDateTime(DateTime.Now); // Gán ngày tạo
                user.CreateBy ??= "user";

                // Lưu người dùng vào cơ sở dữ liệu
                db.Users.Add(user);
                db.SaveChanges(); // Lưu trước để lấy ID

                // Tạo giỏ hàng mới cho người dùng
                Cart gioHang = new Cart
                {
                    UserId = user.Id
                };
                db.Carts.Add(gioHang);
                db.SaveChanges();

                TempData["Status"] = "Chúc mừng bạn đã tạo tài khoản thành công!";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                // Log the exception details to view the inner exception message
                TempData["Status"] = "Có lỗi xảy ra: " + ex.Message;
                if (ex.InnerException != null)
                {
                    TempData["Status"] += "\nInner Exception: " + ex.InnerException.Message;
                }
                return View();
            }
        }

        public IActionResult LogOut()
        {
            // Clear the session data
            HttpContext.Session.Clear();

            // Redirect the user to a different page after logging out
            return RedirectToAction("Index", "Home"); // You can adjust the redirect target as needed
        }

        [HttpPost]
        public IActionResult AddAddress(string City, string State, string AddressLine)
        {
            var acc = HttpContext.Session.GetString("Account");
            if (acc == null)
            {
                return Content("Chưa đăng nhập hoặc phiên đăng nhập hết hạn");
            }
            else
            {
                //lấy ra thông tin của người dùng vs user trên
                var getAcc = db.Users.FirstOrDefault(x => x.UserName == acc);
                var newAddress = new Address
                {
                    City = City,
                    State = State,
                    AddressLine = AddressLine,
                    UserId = getAcc.Id,
                };
                db.Addresses.Add(newAddress);
                db.SaveChanges();
                return RedirectToAction("Index", "Cart");


            }
        }
    }
}

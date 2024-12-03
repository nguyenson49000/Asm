using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSonCoffee.Models;

namespace TSonCoffee.Controllers
{
    public class CartController : Controller
    {
        private readonly TsonCoffeeContext db;

        public CartController(TsonCoffeeContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
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
                //lấy giỏ hàng tương ứng vs người dùng
                var giohang = db.Carts.FirstOrDefault(x => x.UserId == getAcc.Id);


                //lấy tòa bộ sản phẩn có trong ghct của acc

                var data = (from cartItem in db.CartItems
                            join product in db.Products on cartItem.ProductsId equals product.Id
                            join img in db.Images on product.ImgId equals img.Id
                            join ice in db.AmountOfIces on cartItem.AmountOfIceId equals ice.Id
                            join topping in db.Toppings on cartItem.ToppingId equals topping.Id
                            join sweet in db.Sweetnesses on cartItem.SweetnessId equals sweet.Id
                            join size in db.Sizes on cartItem.SizeId equals size.Id
                            where cartItem.CartId == giohang.Id
                            select new CartItemViewModel
                            {
                                ProductName = product.Name,
                                Quantity = cartItem.Quantity,
                                Size = size.Name,
                                sweet = sweet.Name,
                                ice = ice.Name,
                                topping = topping.Name,
                                price = cartItem.Price,
                                totalamount = cartItem.TotalAmount,
                                Img = img.UrlImg
                                
                            }).ToList();

                return View(data);
            }
            return View();
        }
        [HttpPost]
        public IActionResult SaveOrder(int productId, int size, int sweet, int ice, int topping, int quantity)
        {
            try
            {
                // Kiểm tra xem người dùng đã đăng nhập chưa
                var acc = HttpContext.Session.GetString("Account");
                if (acc == null)
                {
                    return Content("Chưa đăng nhập hoặc phiên đăng nhập hết hạn");
                }

                // Lấy thông tin người dùng từ database
                var getAcc = db.Users.FirstOrDefault(x => x.UserName == acc);
                if (getAcc == null)
                {
                    return Content("Tài khoản không tồn tại");
                }

                // Lấy giỏ hàng của người dùng
                var giohang = db.Carts.FirstOrDefault(x => x.UserId == getAcc.Id);
                if (giohang == null)
                {
                    return Content("Giỏ hàng không tồn tại");
                }

                // Lấy thông tin sản phẩm từ database
                var product = db.Products.FirstOrDefault(x => x.Id == productId);
                if (product == null)
                {
                    return Content("Sản phẩm không tồn tại");
                }

                // Lấy thông tin size và topping từ database
                var sizeAdd = db.Sizes.FirstOrDefault(x => x.Id == size);
                var toppingAdd = db.Toppings.FirstOrDefault(x => x.Id == topping);
                if (sizeAdd == null || toppingAdd == null)
                {
                    return Content("Size hoặc Topping không hợp lệ");
                }

                // Tạo đối tượng CartItem mới
                var data = new CartItem
                {
                    CartId = giohang.Id,
                    ProductsId = productId,
                    Quantity = quantity,
                    SizeId = size,
                    SweetnessId = sweet,
                    AmountOfIceId = ice,
                    ToppingId = topping,
                    Price = product.Price,
                    TotalAmount = (product.Price * quantity) + sizeAdd.AdditionalAmount + toppingAdd.AdditionalAmount
                };

                // Lưu dữ liệu vào cơ sở dữ liệu
                db.CartItems.Add(data);
                db.SaveChanges();

                // Chuyển hướng đến trang giỏ hàng
                return RedirectToAction("Index", "Cart");
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần
                Console.WriteLine("Error: " + ex.Message);
                return Content("Đã xảy ra lỗi. Vui lòng thử lại sau.");
            }
        }

        public IActionResult CheckOut()
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
                //lấy giỏ hàng tương ứng vs người dùng
                var giohang = db.Carts.FirstOrDefault(x => x.UserId == getAcc.Id);


                //lấy tòa bộ sản phẩn có trong ghct của acc

                var data = (from cartItem in db.CartItems
                            join product in db.Products on cartItem.ProductsId equals product.Id
                            join img in db.Images on product.ImgId equals img.Id
                            join ice in db.AmountOfIces on cartItem.AmountOfIceId equals ice.Id
                            join topping in db.Toppings on cartItem.ToppingId equals topping.Id
                            join sweet in db.Sweetnesses on cartItem.SweetnessId equals sweet.Id
                            join size in db.Sizes on cartItem.SizeId equals size.Id
                            where cartItem.CartId == giohang.Id
                            select new CartItemViewModel
                            {
                                ProductName = product.Name,
                                Quantity = cartItem.Quantity,
                                Size = size.Name,
                                sweet = sweet.Name,
                                ice = ice.Name,
                                topping = topping.Name,
                                price = cartItem.Price,
                                totalamount = cartItem.TotalAmount,
                                Img = img.UrlImg

                            }).ToList();

                return View(data);
            }
            return View();
        }

        public IActionResult DatHang(string recipientName, string phoneNumber, int paymentMethodId)
        {
            var acc = HttpContext.Session.GetString("Account");
            if (acc == null)
            {
                return RedirectToAction("Login", "Account"); // Điều hướng tới trang đăng nhập nếu chưa đăng nhập
            }

            // Lấy thông tin người dùng
            var getAcc = db.Users.FirstOrDefault(x => x.UserName == acc);
            if (getAcc == null)
            {
                return Content("Không tìm thấy tài khoản người dùng");
            }

            // Lấy giỏ hàng của người dùng
            var giohang = db.Carts.FirstOrDefault(x => x.UserId == getAcc.Id);
            if (giohang == null)
            {
                return Content("Giỏ hàng của bạn hiện đang trống");
            }

            // Lấy danh sách sản phẩm từ giỏ hàng
            var cartItems = (from cartItem in db.CartItems
                             join product in db.Products on cartItem.ProductsId equals product.Id
                             where cartItem.CartId == giohang.Id
                             select cartItem).ToList();

            if (!cartItems.Any())
            {
                return Content("Không có sản phẩm nào trong giỏ hàng để thanh toán");
            }

            // Tạo hóa đơn
            double temporaryTotal = cartItems.Sum(x => x.TotalAmount);
            double vat = temporaryTotal * 0.1;
            double shippingFee = 20000;
            double totalAmount = temporaryTotal + vat + shippingFee;

            var invoice = new Invoice
            {
                NameShop = "TSonCoffee",
                TemporaryTotalAmount = temporaryTotal,
                Vat = vat,
                ShippingFee = shippingFee,
                TotalAmount = totalAmount,
                CreateBy = getAcc.Id,
                PaymentMethodId = paymentMethodId,
                AddressId = 1,
                Status = false // Chưa thanh toán
            };

            db.Invoices.Add(invoice);
            db.SaveChanges();

            // Thêm chi tiết hóa đơn
            foreach (var item in cartItems)
            {
                var detailedInvoice = new DetailedInvoice
                {
                    InvoicesId = invoice.Id,
                    ProductsId = item.ProductsId,
                    SizeId = item.SizeId,
                    SweetnessId = item.SweetnessId,
                    AmountOfIceId = item.AmountOfIceId,
                    ToppingId = item.ToppingId,
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                db.DetailedInvoices.Add(detailedInvoice);
            }

            db.SaveChanges();

            // Xóa giỏ hàng sau khi tạo hóa đơn
            db.CartItems.RemoveRange(cartItems);
            db.SaveChanges();

            return RedirectToAction("Index","Home");
        }


    }
}

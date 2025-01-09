using GM_Shop.DAL;
using GM_Shop.Helper;
using GM_Shop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GM_Shop.Controllers
{
    public class GioHangController : Controller
    {
        SanPhamDAL sanPhamDAL = new SanPhamDAL();
        KhachHangDAL khachHangDAL = new KhachHangDAL();
        GioHangDAL gioHangDAL = new GioHangDAL();
        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MyConst.CART_KEY) ?? new List<CartItem>();
        public IActionResult Index()
        {
            return View(Cart);
        }
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MASP == id);
            if (item == null)
            {
                SanPham? productById = sanPhamDAL.GetSPById(id);

                if (productById == null)
                {
                    TempData["Message"] = "Khong tim thay san pham";
                    return Redirect("/404");
                }

                item = new CartItem
                {
                    MASP = productById.MASP,
                    ANH = productById.ANH,
                    TENSP = productById.TENSP,
                    DONGIA = productById.DONGIA,
                    DANHGIA = productById.DANHGIA,
                    SOLUONG = quantity
                };
                gioHang.Add(item);
            }
            else
            {
                item.SOLUONG += quantity;
            }

            HttpContext.Session.Set(MyConst.CART_KEY, gioHang);

            return RedirectToAction("Index");
        }
        public IActionResult ChangeQuantityCart(int id, bool isIncrement = true, int quantity = 1)
        {
            // Lấy toàn bộ giỏ hàng 
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MASP == id);

            //kiểm tra tồn tại Product 
            if (item == null)
            {

                TempData["Message"] = "Khong tim thay san pham";
                return Redirect("/404");
            }
            else
            {
                // Nếu là button tăng số lượng 
                if (isIncrement)
                {
                    item.SOLUONG += quantity;
                }
                // Nếu là button giảm số lượng 
                else
                {
                    item.SOLUONG -= quantity;
                    // Nếu khách hàng nhập số lượng <= 0 thì xóa sản phẩm đó ra khỏi giỏ hàng
                    if (item.SOLUONG <= 0)
                    {
                        gioHang.Remove(item);
                    }
                }
            }

            // Lưu thay đổi 
            HttpContext.Session.Set(MyConst.CART_KEY, gioHang);

            return RedirectToAction("Index");
        }
        public IActionResult RemoveCart(int id)
        {
            var gioHang = Cart;

            var item = gioHang.SingleOrDefault(p => p.MASP == id);
            if (item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(MyConst.CART_KEY, gioHang);
            }
            return RedirectToAction("Index");
        }
        [Authorize] //Kiểm tra user đăng nhập ?  
        public IActionResult CheckOut()
        {
            try
            {
                //Không thanh toán khi không có sản phẩm trong giỏ hàng 
                if (Cart.Count() == 0)
                {
                    TempData["CheckOutErrorMessage"] = "Thanh toán thất bại";
                    return RedirectToAction("Index");
                }
                // Lấy Customer id nếu người dùng đã đăng nhập 
                string? idTam = null;
                if (HttpContext.User.FindFirstValue("Id") != null)
                {
                    idTam = HttpContext.User.FindFirstValue("Id");
                }

                if (idTam == null)
                {
                    return Redirect("/KhachHang/SignIn");
                }

                //Lấy thông tin User 
                int id = Convert.ToInt32(idTam!);
                KhachHang? khachHang = khachHangDAL.GetCustomerById(id);

                // Nếu không tìm thấy User thì trả về trang 404 - Not Found 
                if (khachHang == null)
                {
                    return Redirect("/404");
                }

                //Insert Vào Database 
                bool isSuccess = gioHangDAL.CheckOut(khachHang, Cart);
                if (isSuccess)
                {
                    //Insert Database thành công 
                    TempData["CheckOutSuccessMessage"] = "Thanh toán thành công";

                    var gioHang = Cart;
                    gioHang = new List<CartItem>();
                    HttpContext.Session.Set(MyConst.CART_KEY, gioHang);
                }
                else
                {
                    //Insert Database thất bại 
                    TempData["CheckOutErrorMessage"] = "Thanh toán thất bại";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                TempData["CheckOutErrorMessage"] = "Lỗi hệ thống: " + ex.Message.ToString();
                return RedirectToAction("Index");
            }

        }
        //Lấy tổng tiền thanh toán
        public IActionResult GetTotalAmount()
        {
            // Giả sử bạn có phương thức GetTotal để tính tổng số tiền của giỏ hàng
            decimal totalAmount = Cart.Sum(item => item.Total);
            return Json(totalAmount);
        }
        //Refresh Cart View Component
        public IActionResult RefreshCartViewComponent()
        {
            // Trả về ViewComponent("Tên ViewComponent muốn gọi", tham số truyền vào nếu có)
            return ViewComponent("Cart");
        }
        //Lấy tổng tiền theo từng Product
        public IActionResult GetTotalProduct(int idProduct)
        {
            // Giả sử bạn có phương thức GetTotal để tính tổng số tiền của giỏ hàng
            var productFind = Cart.Find(item => item.MASP == idProduct);

            decimal totalAmount = 0;
            if (productFind != null)
            {
                totalAmount = productFind.Total;
            }
            return Json(totalAmount);
        }


    }
}

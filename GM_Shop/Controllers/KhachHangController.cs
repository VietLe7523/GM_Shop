using GM_Shop.DAL;
using GM_Shop.Helper;
using GM_Shop.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Claims;

namespace GM_Shop.Controllers
{
    public class KhachHangController : Controller
    {
        KhachHangDAL khachHangDAL = new KhachHangDAL();
        NhanVienDAL nhanVienDAL = new NhanVienDAL();
        public IActionResult Index()
        {
            return View();
        }
        // Hiển thị thông tin khách hàng
        [Authorize]
        public IActionResult Profile()
        {
            try
            {
                string? idTam = null;
                if (HttpContext.User.FindFirstValue("Id") != null)
                {
                    idTam = HttpContext.User.FindFirstValue("Id");
                }
                if (idTam == null)
                {
                    return RedirectToAction("SignIn");
                }

                int id = Convert.ToInt32(idTam!);

                KhachHang? khachHang = khachHangDAL.GetCustomerById(id);
                if (khachHang == null)
                {
                    return Redirect("/404");
                }
                return View(khachHang);
            }
            catch (Exception)
            {
                return Redirect("/404");
            }
        }

        // Cập nhật thông tin khách hàng
        [HttpPost]
        public IActionResult UpdateDetailCustomer(KhachHang customerUpdate, IFormFile Img)
        {
            // Tự động lấy thời gian updateAt theo giờ hệ thống khi khách hàng cập nhật thông tin
            DateTime now = DateTime.Now;
            customerUpdate.NGAYSINH = customerUpdate.NGAYSINH ?? DateOnly.FromDateTime(now);  // Giả sử bạn muốn cập nhật ngày sinh mặc định nếu không có

            // Nếu có hình ảnh được upload
            if (Img != null)
            {
                // Upload hình ảnh (có thể dùng helper để xử lý)
                var ImageName = ImageHelper.UpLoadImage(Img, "KhachHangs");
                customerUpdate.ANH = ImageName;
            }
            else if (customerUpdate.ANH == null)
            {
                customerUpdate.ANH = ""; // Nếu không có ảnh, gán giá trị rỗng
            }

            // Kiểm tra thông tin địa chỉ có null hay không
            if (customerUpdate.DIACHI == null)
            {
                customerUpdate.DIACHI = ""; // Nếu không có địa chỉ, gán giá trị rỗng
            }

            bool isSuccess = khachHangDAL.UpdateCustomerDetails(customerUpdate, customerUpdate.MAKH);

            // Kiểm tra truy vấn SQL thành công hay không?
            if (isSuccess)
            {
                // Truy vấn thành công
                TempData["ProfileSuccessMessage"] = "Cập nhật thông tin thành công";
                return RedirectToAction("Profile");
            }
            else
            {
                // Truy vấn thất bại
                TempData["ProfileErrorMessage"] = "Lỗi hệ thống";
                return RedirectToAction("Profile");
            }
        }

        #region SIGN_UP
        // ------------------ SIGN UP --------------------
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(KhachHang khachHangSignUp, IFormFile Img)
        {
            try
            {
                //Nếu có hình ảnh được Upload
                if (Img != null)
                {
                    //Upload Hinh
                    var ImageName = ImageHelper.UpLoadImage(Img, "KhachHangs");
                    khachHangSignUp.ANH = ImageName;
                }
                else
                {
                    //Sử dụng avatar mặc định của project
                    khachHangSignUp.ANH = "avatar-default.jpg";
                }

                //kiểm tra Address có null hay không
                if (khachHangSignUp.DIACHI == null)
                {
                    khachHangSignUp.DIACHI = "";
                }

                bool isSuccess = khachHangDAL.SignUp(khachHangSignUp);

                // Kiểm tra truy vấn SQL thành công hay không?
                if (isSuccess)
                {
                    // Truy vấn Thành công
                    Console.WriteLine("Update Customer Success");
                    TempData["SignInSuccessMessage"] = "Đăng ký thành công";
                    return RedirectToAction("SignIn");
                }
                else
                {
                    // Truy vấn Thất bại
                    Console.WriteLine("Update Customer Fail");
                    TempData["SignUpErrorMessage"] = "Lỗi hệ thống";
                    return RedirectToAction("SignUp");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View();
            }
        }
        #endregion

        #region SIGN_IN
        // ------------------ SIGN IN --------------------
        public IActionResult SignIn(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(User user, string? returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.ReturnUrl = returnUrl;

                    KhachHang? khachHang = khachHangDAL.CheckCustomerLogin(user.TAIKHOAN, user.MATKHAU);
                    NhanVien? nhanVien = null;

                    if (khachHang == null)
                    {
                        nhanVien = nhanVienDAL.CheckEmployeeLogin(user.TAIKHOAN, user.MATKHAU);
                    }
                    if (khachHang == null && nhanVien == null)
                    {
                        ModelState.AddModelError("Error", "Sai tài khoản hoặc mật khẩu");
                        return View();
                    }

                    var claims = new List<Claim>();
                    if (khachHang != null)
                    {
                        claims.Add(new Claim(ClaimTypes.Name, khachHang.HOKH + " " + khachHang.TENKH));
                        claims.Add(new Claim("Id", khachHang.UserID.ToString()));
                        claims.Add(new Claim(ClaimTypes.Role, "Customer"));
                    }
                    else if (nhanVien != null)
                    {
                        claims.Add(new Claim(ClaimTypes.Name, nhanVien.HONV + " " + nhanVien.TENNV));
                        claims.Add(new Claim("Id", nhanVien.UserID.ToString()));
                        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                        claims.Add(new Claim("ANH", nhanVien.ANH));
                    }

                    var claimIdentity = new ClaimsIdentity(claims, "login");
                    var claimPrincipal = new ClaimsPrincipal(claimIdentity);

                    await HttpContext.SignInAsync(claimPrincipal);

                    // Điều hướng dựa trên Role
                    if (khachHang != null) // Nếu là khách hàng
                    {
                        return RedirectToAction("Index", "Home"); // Chuyển tới trang chủ
                    }
                    else if (nhanVien != null) // Nếu là nhân viên
                    {
                        return RedirectToAction("Index", "SanPhamAdmin", new { area = "Admin" }); // Chuyển tới Area Admin
                    }
                    // Điều hướng mặc định nếu không xác định Role
                    return Redirect("/");
                }
                else return View();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return View();
            }
        }
        #endregion 
        #region SIGN_OUT
        //------------ SIGN OUT -------------
        [Authorize]
        public async Task<IActionResult> SignOutAsync()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
        #endregion
        //------------ Change password -------------
        public IActionResult ChangePassword()
        {
            return View();
        }

        // Xử lý đổi mật khẩu
        [HttpPost]
        public IActionResult ChangePassword(string username, string oldPassword, string newPassword)
        {
            // Kiểm tra đầu vào
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword))
            {
                TempData["Error"] = "Vui lòng nhập đầy đủ thông tin.";
                return View();
            }

            // Kiểm tra thông tin tài khoản
            var customer = khachHangDAL.CheckCustomerLogin(username, oldPassword);
            if (customer == null)
            {
                TempData["Error"] = "Tài khoản hoặc mật khẩu cũ không chính xác.";
                return View();
            }

            // Thay đổi mật khẩu
            bool isPasswordChanged = khachHangDAL.ChangePassword(username, oldPassword, newPassword);
            if (isPasswordChanged)
            {
                TempData["Success"] = "Đổi mật khẩu thành công.";
                return RedirectToAction("SignIn");
            }
            else
            {
                TempData["Error"] = "Đổi mật khẩu không thành công. Vui lòng thử lại.";
                return View();
            }
        }
    }
}
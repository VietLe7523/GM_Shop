using GM_Shop.Areas.Admin.DAL;
using GM_Shop.Areas.Admin.Models;
using GM_Shop.DAL;
using GM_Shop.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GM_Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class KhachHangAdminController : Controller
    {
        KhachHangAdminDAL khachHangAdminDAL = new KhachHangAdminDAL();
        
        // GET: KhachHangAdminController
        public ActionResult Index(int page = 1)
        {
            //Khai báo số lượng Row trong 1 trang
            int pageSize = 5;

            // Lấy danh sách Product sau khi phân trang
            List<KhachHangAdmin> khachHangs = new List<KhachHangAdmin>();
            khachHangs = khachHangAdminDAL.getKhachHang_Pagination(page, pageSize);
            int numRows = khachHangAdminDAL.getCountRow_Pagination(page, pageSize);

            //Tính số lượng trang sẽ có (làm tròn lên, vd: 4.3 -> 5)
            double pageCount = (double)numRows / pageSize;
            int maxPage = (int)Math.Ceiling(pageCount);

            //Tạo model để hiển thị
            KhachHangAdminModel model = new KhachHangAdminModel();
            model.KhachHangAdmins = khachHangs;
            model.CurrentPageIndex = page;
            model.PageCount = maxPage;

            return View(model);
        }

        // GET: KhachHangAdminController/Details/5
        public ActionResult Details(int id)
        {
            KhachHangAdmin khachHang = new KhachHangAdmin();
            khachHang = khachHangAdminDAL.GetKhachHangById(id);
            return View(khachHang);
        }

        // GET: KhachHangAdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KhachHangAdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KhachHangAdmin khachHang, IFormFile Img)
        {
            try
            {
                //Upload Hinh 
                if (Img == null)
                {
                    khachHang.ANH = "";
                }
                else
                {
                    var ImageName = ImageHelper.UpLoadImage(Img, "KhachHangs");
                    khachHang.ANH = ImageName;
                }
                bool IsInserted = khachHangAdminDAL.AddNew(khachHang); ;
                if (IsInserted)
                {
                    // Truy vấn Thành công 
                    Console.WriteLine("Insert customer Success");
                    TempData["SuccessMessage"] = "Insert Success";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Truy vấn Thất bại 
                    Console.WriteLine("Insert customer Fail");
                    TempData["ErrorMessage"] = "Insert Fail";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // GET: KhachHangAdminController/Edit/5
        public ActionResult Edit(int id)
        {
            KhachHangAdmin khachHang = new KhachHangAdmin();
            khachHang = khachHangAdminDAL.GetKhachHangById(id);
            return View(khachHang);
        }

        // POST: KhachHangAdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, KhachHangAdmin khachHang, IFormFile Img)
        {
            try
            {
                //Nếu có hình ảnh được Upload 
                if (Img != null)
                {
                    //Upload Hinh 
                    var ImageName = ImageHelper.UpLoadImage(Img, "KhachHangs");
                    khachHang.ANH = ImageName;
                }
                // truy vấn tới CSDL 
                bool IsInserted = khachHangAdminDAL.UpdateKhachHangById(khachHang, id);

                // Kiểm tra truy vấn SQL thành công hay không? 
                if (IsInserted)
                {
                    // Truy vấn Thành công 
                    Console.WriteLine("Update Product Success");
                    TempData["SuccessMessage"] = "Update Success";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Truy vấn Thất bại 
                    Console.WriteLine("Update Product Fail");
                    TempData["ErrorMessage"] = "Update Fail";
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: KhachHangAdminController/Delete/5
        public ActionResult Delete(int id)
        {
            KhachHangAdmin khachHang = new KhachHangAdmin();
            khachHang = khachHangAdminDAL.GetKhachHangById(id);
            return View(khachHang);
        }

        // POST: KhachHangAdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                //Truy vấn SQL 
                var IsSuccess = khachHangAdminDAL.DeleteCustomerAndUserById(id);

                // Kiểm tra truy vấn SQL thành công hay không? 
                if (IsSuccess)
                {
                    // Truy vấn Thành công 
                    Console.WriteLine("Delete Product Success");
                    TempData["SuccessMessage"] = "Update Success";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Truy vấn Thất bại 
                    Console.WriteLine("Delete Product Fail");
                    TempData["ErrorMessage"] = "Update Fail";
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }
    }
}

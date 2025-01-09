using GM_Shop.Areas.Admin.DAL;
using GM_Shop.Areas.Admin.Models;
using GM_Shop.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GM_Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DonHangAdminController : Controller
    {
        DonHangAdminDAL donHangDAL = new DonHangAdminDAL();
        // GET: DonHangAdminController
        public ActionResult Index(int page = 1)
        {
            //Khai báo số lượng Row trong 1 trang
            int pageSize = 5;

            // Lấy danh sách Product sau khi phân trang
            List<DonHangAdmin> donHangs = new List<DonHangAdmin>();
            donHangs = donHangDAL.getDonHang_Pagination(page, pageSize);
            int numRows = donHangDAL.getCountRow_Pagination(page, pageSize);

            //Tính số lượng trang sẽ có (làm tròn lên, vd: 4.3 -> 5)
            double pageCount = (double)numRows / pageSize;
            int maxPage = (int)Math.Ceiling(pageCount);

            //Tạo model để hiển thị
            DonHangAdminModel model = new DonHangAdminModel();
            model.DonHangAdmins = donHangs;
            model.CurrentPageIndex = page;
            model.PageCount = maxPage;

            return View(model);
        }

        // GET: DonHangAdminController/Details/5
        public ActionResult Details(int id)
        {
            var donHang = donHangDAL.GetOrderById(id);
            return View(donHang);
        }

        // GET: DonHangAdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DonHangAdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DonHangAdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DonHangAdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DonHangAdminController/Delete/5
        public ActionResult Delete(int id)
        {
            var donhang = donHangDAL.GetOrderById(id);
            return View(donhang);
        }

        // POST: DonHangAdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                bool isDeleted = donHangDAL.DeleteOrderById(id);
                if (isDeleted)
                {
                    TempData["SuccessMessage"] = "Xóa hóa đơn thành công.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Xóa hóa đơn thất bại. Vui lòng thử lại.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete error: " + ex.Message);
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }
    }
}

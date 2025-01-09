using GM_Shop.Areas.Admin.DAL;
using GM_Shop.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GM_Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ThuongHieuAdminController : Controller
    {
        ThuongHieuAdminDAL thuongHieuDAL = new ThuongHieuAdminDAL();
        // GET: ThuongHieuAdminController
        public ActionResult Index(int page = 1)
        {
            //Khai báo số lượng Row trong 1 trang
            int pageSize = 5;

            // Lấy danh sách Product sau khi phân trang
            List<ThuongHieuAdmin> thuongHieus = new List<ThuongHieuAdmin>();
            thuongHieus = thuongHieuDAL.getThuongHieu_Pagination(page, pageSize);
            int numRows = thuongHieuDAL.getCountRow_Pagination(page, pageSize);

            //Tính số lượng trang sẽ có (làm tròn lên, vd: 4.3 -> 5)
            double pageCount = (double)numRows / pageSize;
            int maxPage = (int)Math.Ceiling(pageCount);

            //Tạo model để hiển thị
            ThuongHieuAdminModel model = new ThuongHieuAdminModel();
            model.ThuongHieuAdmins = thuongHieus;
            model.CurrentPageIndex = page;
            model.PageCount = maxPage;

            return View(model);
        }

        // GET: ThuongHieuAdminController/Details/5
        public ActionResult Details(int id)
        {
            ThuongHieuAdmin thuongHieu = new ThuongHieuAdmin();
            thuongHieu = thuongHieuDAL.getThuongHieuById(id);
            return View(thuongHieu);
        }

        // GET: ThuongHieuAdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ThuongHieuAdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ThuongHieuAdmin thuongHieuNew)
        {
            try
            {
                bool IsInserted = false;
                if (ModelState.IsValid)
                {
                    IsInserted = thuongHieuDAL.AddNew(thuongHieuNew);
                    if (IsInserted)
                    {
                        TempData["SuccessMessage"] = "Insert Success";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Insert Fail";
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // GET: ThuongHieuAdminController/Edit/5
        public ActionResult Edit(int id)
        {
            ThuongHieuAdmin thuongHieu = new ThuongHieuAdmin();
            thuongHieu = thuongHieuDAL.getThuongHieuById(id);
            return View(thuongHieu);
        }

        // POST: ThuongHieuAdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ThuongHieuAdmin thuongHieuNew)
        {
            try
            {
                bool IsInserted = false;
                if (ModelState.IsValid)
                {
                    Console.WriteLine("Update Thuong Hieu Id = ", id);

                    IsInserted = thuongHieuDAL.updateThuongHieuById(id, thuongHieuNew);
                    if (IsInserted)
                    {
                        Console.WriteLine("Update Success");
                        TempData["SuccessMessage"] = "Insert Success";
                    }
                    else
                    {
                        Console.WriteLine("Update Fail");
                        TempData["ErrorMessage"] = "Insert Fail";
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Update error " + ex.Message);
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // GET: ThuongHieuAdminController/Delete/5
        public ActionResult Delete(int id)
        {
            ThuongHieuAdmin thuongHieu = new ThuongHieuAdmin();
            thuongHieu = thuongHieuDAL.getThuongHieuById(id);
            return View(thuongHieu);
        }

        // POST: ThuongHieuAdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                bool IsInserted = false;
                if (ModelState.IsValid)
                {
                    Console.WriteLine("Delete Role Id = ", id);

                    IsInserted = thuongHieuDAL.deleteThuongHieuById(id);
                    if (IsInserted)
                    {
                        Console.WriteLine("Delete Success");
                        TempData["SuccessMessage"] = "Delete Success";
                    }
                    else
                    {
                        Console.WriteLine("Delete Fail");
                        TempData["ErrorMessage"] = "Delete Fail";
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete error " + ex.Message);
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }
    }
}

using GM_Shop.Areas.Admin.DAL;
using GM_Shop.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GM_Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ThongSoKyThuatAdminController : Controller
    {
        ThongSoKyThuatAdminDAL tsktDAL = new ThongSoKyThuatAdminDAL();
        // GET: ThongSoKyThuatAdminController
        public ActionResult Index(int page = 1)
        {
            //Khai báo số lượng Row trong 1 trang
            int pageSize = 5;

            // Lấy danh sách Product sau khi phân trang
            List<ThongSoKyThuatAdmin> tskts = new List<ThongSoKyThuatAdmin>();
            tskts = tsktDAL.getThongSoKyThuat_Pagination(page, pageSize);
            int numRows = tsktDAL.getCountRow_Pagination(page, pageSize);

            //Tính số lượng trang sẽ có (làm tròn lên, vd: 4.3 -> 5)
            double pageCount = (double)numRows / pageSize;
            int maxPage = (int)Math.Ceiling(pageCount);

            //Tạo model để hiển thị
            ThongSoKyThuatAdminModel model = new ThongSoKyThuatAdminModel();
            model.ThongSoKyThuatAdmins = tskts;
            model.CurrentPageIndex = page;
            model.PageCount = maxPage;

            return View(model);
        }

        // GET: ThongSoKyThuatAdminController/Details/5
        public ActionResult Details(int id)
        {
            ThongSoKyThuatAdmin tskt = new ThongSoKyThuatAdmin();
            tskt = tsktDAL.getTSKTById(id);
            return View(tskt);
        }

        // GET: ThongSoKyThuatAdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ThongSoKyThuatAdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ThongSoKyThuatAdmin tskt)
        {
            try
            {
                bool IsInserted = false;
                if (ModelState.IsValid)
                {
                    IsInserted = tsktDAL.AddNew(tskt);
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

        // GET: ThongSoKyThuatAdminController/Edit/5
        public ActionResult Edit(int id)
        {
            ThongSoKyThuatAdmin tskt = new ThongSoKyThuatAdmin();
            tskt = tsktDAL.getTSKTById(id);
            return View(tskt);
        }

        // POST: ThongSoKyThuatAdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ThongSoKyThuatAdmin tskt)
        {
            try
            {
                bool IsInserted = false;
                if (ModelState.IsValid)
                {
                    Console.WriteLine("Update tskt Id = ", id);

                    IsInserted = tsktDAL.UpdateTSKTById(id, tskt);
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

        // GET: ThongSoKyThuatAdminController/Delete/5
        public ActionResult Delete(int id)
        {
            ThongSoKyThuatAdmin tskt = new ThongSoKyThuatAdmin();
            tskt = tsktDAL.getTSKTById(id);
            return View(tskt);
        }

        // POST: ThongSoKyThuatAdminController/Delete/5
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

                    IsInserted = tsktDAL.DeleteTSKTById(id);
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

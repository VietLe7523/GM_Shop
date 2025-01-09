using GM_Shop.Areas.Admin.DAL;
using GM_Shop.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GM_Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DanhMucSPAdminController : Controller
    {
        DanhMucSPAdminDAL danhMucDAL = new DanhMucSPAdminDAL();
        // GET: DanhMucSPAdminController
        public ActionResult Index(int page = 1)
        {
            //Khai báo số lượng Row trong 1 trang
            int pageSize = 5;

            // Lấy danh sách Product sau khi phân trang
            List<DanhMucSPAdmin> danhMucs = new List<DanhMucSPAdmin>();
            danhMucs = danhMucDAL.getDanhMuc_Pagination(page, pageSize);
            int numRows = danhMucDAL.getCountRow_Pagination(page, pageSize);

            //Tính số lượng trang sẽ có (làm tròn lên, vd: 4.3 -> 5)
            double pageCount = (double)numRows / pageSize;
            int maxPage = (int)Math.Ceiling(pageCount);

            //Tạo model để hiển thị
            DanhMucSPAdminModel model = new DanhMucSPAdminModel();
            model.DanhMucSPAdmins = danhMucs;
            model.CurrentPageIndex = page;
            model.PageCount = maxPage;

            return View(model);
        }

        // GET: DanhMucSPAdminController/Details/5
        public ActionResult Details(int id)
        {
            DanhMucSPAdmin danhMuc = new DanhMucSPAdmin();
            danhMuc =  danhMucDAL.getDanhMucById(id);
            return View(danhMuc);
        }

        // GET: DanhMucSPAdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DanhMucSPAdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DanhMucSPAdmin danhMuc)
        {
            try
            {
                bool isInserted = false;
                if (ModelState.IsValid)
                {
                    isInserted = danhMucDAL.AddNew(danhMuc);
                    if (isInserted)
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

        // GET: DanhMucSPAdminController/Edit/5
        public ActionResult Edit(int id)
        {
            DanhMucSPAdmin danhMuc = new DanhMucSPAdmin();
            danhMuc = danhMucDAL.getDanhMucById(id);
            return View(danhMuc);
        }

        // POST: DanhMucSPAdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, DanhMucSPAdmin danhMuc)
        {
            try
            {
                bool isUpdated = false;
                if (ModelState.IsValid)
                {
                    Console.WriteLine("Update DMSP Id = ", id);

                    isUpdated = danhMucDAL.updateDanhMucById(id, danhMuc);
                    if (isUpdated)
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

        // GET: DanhMucSPAdminController/Delete/5
        public ActionResult Delete(int id)
        {
            DanhMucSPAdmin danhMuc = new DanhMucSPAdmin();
            danhMuc = danhMucDAL.getDanhMucById(id);
            return View(danhMuc);
        }

        // POST: DanhMucSPAdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {

                bool isDeleted = false;
                Console.WriteLine("Delete DMSP Id = ", id);

                isDeleted = danhMucDAL.deleteDanhMucById(id);
                if (isDeleted)
                {
                    Console.WriteLine("Delete Success");
                    TempData["SuccessMessage"] = "Delete Success";
                }
                else
                {
                    Console.WriteLine("Delete Fail");
                    TempData["ErrorMessage"] = "Delete Fail";
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

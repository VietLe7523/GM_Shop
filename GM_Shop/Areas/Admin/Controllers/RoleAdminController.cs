using GM_Shop.Areas.Admin.DAL;
using GM_Shop.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GM_Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoleAdminController : Controller
    {
        RoleAdminDAL roleAdminDAL = new RoleAdminDAL();
        // GET: RoleAdminController
        public ActionResult Index(int page = 1)
        {
            //Khai báo số lượng Row trong 1 trang
            int pageSize = 5;

            // Lấy danh sách Product sau khi phân trang
            List<RoleAdmin> roles = new List<RoleAdmin>();
            roles = roleAdminDAL.getRole_Pagination(page, pageSize);
            int numRows = roleAdminDAL.getCountRow_Pagination(page, pageSize);

            //Tính số lượng trang sẽ có (làm tròn lên, vd: 4.3 -> 5)
            double pageCount = (double)numRows / pageSize;
            int maxPage = (int)Math.Ceiling(pageCount);

            //Tạo model để hiển thị
            RoleAdminModel model = new RoleAdminModel();
            model.RoleAdmins = roles;
            model.CurrentPageIndex = page;
            model.PageCount = maxPage;

            return View(model);
        }

        // GET: RoleAdminController/Details/5
        public ActionResult Details(int id)
        {
            RoleAdmin role = new RoleAdmin();  
            role = roleAdminDAL.getRoleById(id);
            return View(role);
        }

        // GET: RoleAdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoleAdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoleAdmin roleNew)
        {
            try
            {
                bool IsInserted = false;
                if (ModelState.IsValid)
                {
                    IsInserted = roleAdminDAL.AddNew(roleNew);
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

        // GET: RoleAdminController/Edit/5
        public ActionResult Edit(int id)
        {
            RoleAdmin role = new RoleAdmin();
            role = roleAdminDAL.getRoleById(id);
            return View(role);
        }

        // POST: RoleAdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, RoleAdmin roleNew)
        {
            try
            {
                bool IsInserted = false;
                if (ModelState.IsValid)
                {
                    Console.WriteLine("Update Role Id = ", id);

                    IsInserted = roleAdminDAL.updateRoleById(id, roleNew);
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

        // GET: RoleAdminController/Delete/5
        public ActionResult Delete(int id)
        {
            RoleAdmin role = new RoleAdmin();
            role = roleAdminDAL.getRoleById(id);
            return View(role);
        }

        // POST: RoleAdminController/Delete/5
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

                    IsInserted = roleAdminDAL.deleteRoleById(id);
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

using GM_Shop.Areas.Admin.DAL;
using GM_Shop.Areas.Admin.Models;
using GM_Shop.Helper;
using GM_Shop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GM_Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class NhanVienAdminController : Controller
    {
        NhanVienAdminDAL nhanVienAdminDAL = new NhanVienAdminDAL();
        RoleAdminDAL roleAdminDAL = new RoleAdminDAL();
        // GET: NhanVienAdminController
        public ActionResult Index(int page = 1)
        {
            //Khai báo số lượng Row trong 1 trang
            int pageSize = 5;

            // Lấy danh sách Product sau khi phân trang
            List<NhanVienAdmin> nhanViens = new List<NhanVienAdmin>();
            nhanViens = nhanVienAdminDAL.getNhanVien_Pagination(page, pageSize);
            int numRows = nhanVienAdminDAL.getCountRow_Pagination(page, pageSize);

            //Tính số lượng trang sẽ có (làm tròn lên, vd: 4.3 -> 5)
            double pageCount = (double)numRows / pageSize;
            int maxPage = (int)Math.Ceiling(pageCount);

            //Tạo model để hiển thị
            NhanVienAdminModel model = new NhanVienAdminModel();
            model.NhanVienAdmins = nhanViens;
            model.CurrentPageIndex = page;
            model.PageCount = maxPage;

            return View(model);
        }

        // GET: NhanVienAdminController/Details/5
        public ActionResult Details(int id)
        {
            NhanVienAdmin nhanVien = new NhanVienAdmin();
            nhanVien = nhanVienAdminDAL.GetNVById(id);
            return View(nhanVien);
        }

        // GET: NhanVienAdminController/Create
        public ActionResult Create()
        {
            // Lấy danh sách role từ DataBase 
            List<RoleAdmin> roles = new List<RoleAdmin>();
            roles = roleAdminDAL.getAll();

            // Khai báo Model
            NhanVienFormAdmin nvAddNew = new NhanVienFormAdmin();

            // Tạo danh sách Input Select 
            nvAddNew.RoleList = new List<SelectListItem>();
            foreach (var item in roles)
            {
                nvAddNew.RoleList.Add(
                    new SelectListItem
                    {
                        Text = item.RoleName,
                        Value = item.RoleID.ToString()
                    }
                );
            }

            return View(nvAddNew);
        }

        // POST: NhanVienAdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NhanVienFormAdmin nvAddNew, IFormFile Img)
        {
            try
            {
                //Upload Hinh 
                if (Img == null)
                {
                    nvAddNew.ANH = "";
                }
                else
                {
                    var ImageName = ImageHelper.UpLoadImage(Img, "NhanViens");
                    nvAddNew.ANH = ImageName;
                }
                //lấy roleid
                int roleId = Convert.ToInt32(nvAddNew.MaRole);
                nvAddNew.RoleID = roleId;

                // truy vấn tới CSDL 
                bool IsInserted = nhanVienAdminDAL.AddNew(nvAddNew);

                // Kiểm tra truy vấn SQL thành công hay không? 
                if (IsInserted)
                {
                    // Truy vấn Thành công 
                    Console.WriteLine("Insert Product Success");
                    TempData["SuccessMessage"] = "Insert Success";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Truy vấn Thất bại 
                    Console.WriteLine("Insert Product Fail");
                    TempData["ErrorMessage"] = "Insert Fail";
                    return View();
                }
            }
            catch (Exception ex)
            {
                //Error 
                Console.WriteLine("Insert Product error ", ex.Message);
                return View();
            }
        }

        // GET: NhanVienAdminController/Edit/5
        public ActionResult Edit(int id)
        {
            // Lấy Thông tin Sản phẩm từ Id 
            NhanVienAdmin nhanVien = new NhanVienAdmin();
            nhanVien = nhanVienAdminDAL.GetNVById(id);

            //Khai báo Model SanPhamFormAdmin
            NhanVienFormAdmin nvFormAdmin = new NhanVienFormAdmin();
            nvFormAdmin.MANV = id;
            nvFormAdmin.HONV = nhanVien.HONV;
            nvFormAdmin.TENNV = nhanVien.TENNV;
            nvFormAdmin.DIACHI = nhanVien.DIACHI;
            nvFormAdmin.SDT = nhanVien.SDT;
            nvFormAdmin.EMAIL = nhanVien.EMAIL;
            nvFormAdmin.GIOITINH = nhanVien.GIOITINH;
            nvFormAdmin.ANH = nhanVien.ANH;
            nvFormAdmin.NGAYSINH = nhanVien.NGAYSINH;
            nvFormAdmin.CCCD = nhanVien.CCCD;
            nvFormAdmin.RoleID = nhanVien.RoleID;
            nvFormAdmin.UserId = nhanVien.UserId;

            // Lấy danh sách role từ DataBase 
            List<RoleAdmin> roles = new List<RoleAdmin>();
            roles = roleAdminDAL.getAll();


            // Tạo danh sách Input Select 
            nvFormAdmin.RoleList = new List<SelectListItem>();
            foreach (var item in roles)
            {
                nvFormAdmin.RoleList.Add(
                    new SelectListItem
                    {
                        Text = item.RoleName,
                        Value = item.RoleID.ToString()
                    }
                );
            }
            return View(nvFormAdmin);
        }

        // POST: NhanVienAdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, NhanVienFormAdmin nvEdit, IFormFile Img)
        {
            try
            {
                //Nếu có hình ảnh được Upload 
                if (Img != null)
                {
                    //Upload Hinh 
                    var ImageName = ImageHelper.UpLoadImage(Img, "SanPhams");
                    nvEdit.ANH = ImageName;
                }
                //lấy roleid
                int roleId = Convert.ToInt32(nvEdit.MaRole);
                nvEdit.RoleID = roleId;

                // truy vấn tới CSDL 
                bool IsInserted = nhanVienAdminDAL.UpdateNVById(id, nvEdit);

                // Kiểm tra truy vấn SQL thành công hay không? 
                if (IsInserted)
                {
                    // Truy vấn Thành công 
                    Console.WriteLine("Insert Product Success");
                    TempData["SuccessMessage"] = "Insert Success";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Truy vấn Thất bại 
                    Console.WriteLine("Insert Product Fail");
                    TempData["ErrorMessage"] = "Insert Fail";
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: NhanVienAdminController/Delete/5
        public ActionResult Delete(int id)
        {
            NhanVienAdmin nhanVien = new NhanVienAdmin();
            nhanVien = nhanVienAdminDAL.GetNVById(id);
            return View(nhanVien);
        }

        // POST: NhanVienAdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                //Truy vấn SQL 
                var IsSuccess = nhanVienAdminDAL.DeleteNhanVienAndUserById(id);

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

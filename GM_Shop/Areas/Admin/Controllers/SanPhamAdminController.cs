using GM_Shop.Areas.Admin.DAL;
using GM_Shop.Areas.Admin.Models;
using GM_Shop.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

namespace GM_Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SanPhamAdminController : Controller
    {
        SanPhamAdminDAL sanPhamAdminDAL = new SanPhamAdminDAL();
        DanhMucSPAdminDAL danhMucSPAdminDAL = new DanhMucSPAdminDAL();
        ThuongHieuAdminDAL thuongHieuAdminDAL = new ThuongHieuAdminDAL();
        ThongSoKyThuatAdminDAL thongSoKyThuatAdminDAL = new ThongSoKyThuatAdminDAL();

        // Phương thức gọi DAL để lấy thông tin chi tiết thông số kỹ thuật
        [HttpGet]
        public IActionResult GetTechnicalInfo(int id)
        {
            var technicalInfo = thongSoKyThuatAdminDAL.getTSKTById(id);

            // Trả về thông tin chi tiết dưới dạng JSON để JavaScript có thể sử dụng
            if (technicalInfo == null)
            {
                return NotFound("Không tìm thấy thông số kỹ thuật.");
            }

            return Json(new
            {
                tentskt = technicalInfo.TENTSKT,
                hedieuhanh = technicalInfo.HEDIEUHANH,
                ram = technicalInfo.RAM,
                rom = technicalInfo.ROM,
                kichcomanhinh = technicalInfo.KICHCOMANHINH,
                vixuly = technicalInfo.VIXULY,
                pin = technicalInfo.PIN,
                camera = technicalInfo.CAMERA
            });
        }
        // GET: SanPhamAdminController
        public ActionResult Index(int page = 1, string searchString = "")
        {
            //Khai báo số lượng Row trong 1 trang
            int pageSize = 5;

            //Text hiển thị trong input sau khi tìm kiếm
            ViewData["CurrentFilter"] = searchString;

            // Lấy danh sách Product sau khi phân trang
            List<SanPhamAdmin> products = new List<SanPhamAdmin>();
            products = sanPhamAdminDAL.getProduct_Pagination(page, pageSize, searchString.Trim());

            // Lấy số lượng Row sau khi truy vấn có thêm điều kiện (tìm kiếm)
            int numRows = sanPhamAdminDAL.getCountRow_Pagination(page, pageSize, searchString.Trim());

            //Tính số lượng trang sẽ có (làm tròn lên, vd: 4.3 -> 5)
            double pageCount = (double)numRows / pageSize;
            int maxPage = (int)Math.Ceiling(pageCount);

            //Tạo model để hiển thị
            SanPhamAdminModel model = new SanPhamAdminModel();
            model.SanPhamAdmins = products;
            model.CurrentPageIndex = page;
            model.PageCount = maxPage;

            return View(model);
        }

        // GET: SanPhamAdminController/Details/5
        public ActionResult Details(int id)
        {
            // Lấy Thông tin Sản phẩm từ Id 
            SanPhamAdmin sanPham = new SanPhamAdmin();
            sanPham = sanPhamAdminDAL.GetSPById(id);
            return View(sanPham);
        }

        // GET: SanPhamAdminController/Create
        public ActionResult Create()
        {
            // Lấy danh sách danh mục sản phẩm từ DataBase 
            List<DanhMucSPAdmin> danhMucSPs = new List<DanhMucSPAdmin>();
            danhMucSPs = danhMucSPAdminDAL.getAll();

            // Lấy danh sách danh mục sản phẩm từ DataBase
            List<ThuongHieuAdmin> thuongHieus = new List<ThuongHieuAdmin>();
            thuongHieus = thuongHieuAdminDAL.getAll();

            // Lấy danh sách danh mục sản phẩm từ DataBase
            List<ThongSoKyThuatAdmin> tskts = new List<ThongSoKyThuatAdmin>();
            tskts = thongSoKyThuatAdminDAL.getAll();

            // Khai báo Model
            SanPhamFormAdmin spAddNew = new SanPhamFormAdmin();

            // Tạo danh sách Input Select 
            spAddNew.DanhMucSPList = new List<SelectListItem>();
            foreach (var item in danhMucSPs)
            {
                spAddNew.DanhMucSPList.Add(
                    new SelectListItem
                    {
                        Text = item.TENDMSP,
                        Value = item.MADMSP.ToString()
                    }
                );
            }

            spAddNew.ThuongHieuList = new List<SelectListItem>();
            foreach (var item in thuongHieus)
            {
                spAddNew.ThuongHieuList.Add(
                    new SelectListItem
                    {
                        Text = item.TENTHUONGHIEU,
                        Value = item.MATH.ToString()
                    }
                );
            }
            spAddNew.ThongSoKyThuatList = new List<SelectListItem>();
            foreach (var item in tskts)
            {
                spAddNew.ThongSoKyThuatList.Add(
                    new SelectListItem
                    {
                        Text = item.TENTSKT,
                        Value = item.MATSKT.ToString()
                    }
                );
            }
            return View(spAddNew);
        }

        // POST: SanPhamAdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SanPhamFormAdmin spAddNew, IFormFile Img)
        {
            try
            {
                //Upload Hinh 
                if (Img == null)
                {
                    spAddNew.ANH = "";
                }
                else
                {
                    var ImageName = ImageHelper.UpLoadImage(Img, "SanPhams");
                    spAddNew.ANH = ImageName;
                }

                //lấy mã danh mục sản phẩm 
                int DanhMucID = Convert.ToInt32(spAddNew.MaDanhMucSP);
                spAddNew.MADMSP = DanhMucID;

                //lấy mã thương hiệu 
                int ThuongHieuID = Convert.ToInt32(spAddNew.MaThuongHieu);
                spAddNew.MATH = ThuongHieuID;

                //lấy mã thông số kỹ thuật
                int tsktID = Convert.ToInt32(spAddNew.MaThongSoKyThuat);
                spAddNew.MATSKT = tsktID;

                // truy vấn tới CSDL 
                bool IsInserted = sanPhamAdminDAL.AddNew(spAddNew);

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

        // GET: SanPhamAdminController/Edit/5
        public ActionResult Edit(int id)
        {
            // Lấy Thông tin Sản phẩm từ Id 
            SanPhamAdmin sanPham = new SanPhamAdmin();
            sanPham = sanPhamAdminDAL.GetSPById(id);

            //Khai báo Model SanPhamFormAdmin
            SanPhamFormAdmin spFormAdmin = new SanPhamFormAdmin();
            spFormAdmin.MASP = id;
            spFormAdmin.TENSP = sanPham.TENSP;
            spFormAdmin.DONGIA = sanPham.DONGIA;
            spFormAdmin.MOTA = sanPham.MOTA;
            spFormAdmin.SOLUONG = sanPham.SOLUONG;
            spFormAdmin.ANH = sanPham.ANH;
            spFormAdmin.DANHGIA = sanPham.DANHGIA;
            spFormAdmin.MADMSP = sanPham.MADMSP;
            spFormAdmin.MATH = sanPham.MATH;
            spFormAdmin.MATSKT = sanPham.MATSKT;

            // Lấy danh sách danh mục sản phẩm từ DataBase
            List<DanhMucSPAdmin> danhMucSPs = new List<DanhMucSPAdmin>();
            danhMucSPs = danhMucSPAdminDAL.getAll();

            // Lấy danh sách danh mục sản phẩm từ DataBase
            List<ThuongHieuAdmin> thuongHieus = new List<ThuongHieuAdmin>();
            thuongHieus = thuongHieuAdminDAL.getAll();

            // Lấy danh sách danh mục sản phẩm từ DataBase
            List<ThongSoKyThuatAdmin> tskts = new List<ThongSoKyThuatAdmin>();
            tskts = thongSoKyThuatAdminDAL.getAll();

            // Tạo danh sách Input Select 
            spFormAdmin.DanhMucSPList = new List<SelectListItem>();
            foreach (var item in danhMucSPs)
            {
                spFormAdmin.DanhMucSPList.Add(
                    new SelectListItem
                    {
                        Text = item.TENDMSP,
                        Value = item.MADMSP.ToString()
                    }
                );
            }
            spFormAdmin.ThuongHieuList = new List<SelectListItem>();
            foreach (var item in thuongHieus)
            {
                spFormAdmin.ThuongHieuList.Add(
                    new SelectListItem
                    {
                        Text = item.TENTHUONGHIEU,
                        Value = item.MATH.ToString()
                    }
                );
            }
            spFormAdmin.ThongSoKyThuatList = new List<SelectListItem>();
            foreach (var item in tskts)
            {
                spFormAdmin.ThongSoKyThuatList.Add(
                    new SelectListItem
                    {
                        Text = item.TENTSKT,
                        Value = item.MATSKT.ToString()
                    }
                );
            }
            return View(spFormAdmin);
        }

        // POST: SanPhamAdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, SanPhamFormAdmin spEdit, IFormFile Img)
        {
            try
            {
                //Nếu có hình ảnh được Upload 
                if (Img != null)
                {
                    //Upload Hinh 
                    var ImageName = ImageHelper.UpLoadImage(Img, "SanPhams");
                    spEdit.ANH = ImageName;
                }

                //lấy mã danh mục sản phẩm 
                int DanhMucID = Convert.ToInt32(spEdit.MaDanhMucSP);
                spEdit.MADMSP = DanhMucID;

                //lấy mã thương hiệu 
                int ThuongHieuID = Convert.ToInt32(spEdit.MaThuongHieu);
                spEdit.MATH = ThuongHieuID;

                //lấy mã thông số kỹ thuật
                int tsktID = Convert.ToInt32(spEdit.MaThongSoKyThuat);
                spEdit.MATSKT = tsktID;

                // truy vấn tới CSDL 
                bool IsInserted = sanPhamAdminDAL.UpdateSPById(spEdit, id);

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

        // GET: SanPhamAdminController/Delete/5
        public ActionResult Delete(int id)
        {
            // Lấy Thông tin Sản phẩm từ Id 
            SanPhamAdmin sanPham = new SanPhamAdmin();
            sanPham = sanPhamAdminDAL.GetSPById(id);
            return View(sanPham);
        }

        // POST: SanPhamAdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                //Truy vấn SQL 
                var IsSuccess = sanPhamAdminDAL.DeleteSPById(id);

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

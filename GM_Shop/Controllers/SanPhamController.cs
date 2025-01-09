using GM_Shop.DAL;
using GM_Shop.Models;
using Microsoft.AspNetCore.Mvc;

namespace GM_Shop.Controllers
{
    public class SanPhamController : Controller
    {
        SanPhamDAL sanPhamDAL = new SanPhamDAL();
        public IActionResult Index(int? idCategory, int? idBrand, int page = 1, string searchString = "")
        {
            //Khai báo số lượng Row trong 1 trang
            int pageSize = 6;

            // Lưu Id Category 
            ViewData["IdCategory"] = idCategory;

            // Lưu idBrand 
            ViewData["IdBrand"] = idBrand;

            //Text hiển thị trong input sau khi tìm kiếm
            ViewData["CurrentFilter"] = searchString;

            // Lấy danh sách Product sau khi phân trang
            List<SanPham> sanPhams = new List<SanPham>();
            sanPhams = sanPhamDAL.getProduct_Pagination(idCategory, idBrand, page, pageSize, searchString.Trim());

            //Lấy tổng số lượng Product
            int rowCount = sanPhamDAL.GetListProduct(idCategory, idBrand).Count();

            //Tính số lượng trang
            double pageCount = (double)rowCount / pageSize;
            int maxPage = (int)Math.Ceiling(pageCount);

            //Tạo model để hiển thị
            SanPhamPagination model = new SanPhamPagination();
            model.sanPhams = sanPhams;
            model.CurrentPageIndex = page;
            model.PageCount = maxPage;

            return View(model);
        }
        //View Detail 
        public IActionResult Detail(int id)
        {
            SanPham sanPham = new SanPham();

            sanPham = sanPhamDAL.GetSPById(id);

            return View(sanPham);
        }
    }
}

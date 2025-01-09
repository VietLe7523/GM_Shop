using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GM_Shop.Areas.Admin.Models
{
    public class SanPhamAdmin
    {
        [Key]
        public int MASP { get; set; }

        [Display(Name = "Tên sản phẩm")]
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        [MaxLength(100, ErrorMessage = "Tên sản phẩm tối đa 100 kí tự.")]
        public string TENSP { get; set; }

        [Display(Name = "Đơn giá")]
        [Required(ErrorMessage = "Đơn giá là bắt buộc.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Đơn giá phải lớn hơn 0.")]
        public decimal DONGIA { get; set; }

        [Display(Name = "Số lượng")]
        [Required(ErrorMessage = "Số lượng là bắt buộc.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int SOLUONG { get; set; }
        [Display(Name = "Đánh giá")]
        [Required(ErrorMessage = "Đánh giá là bắt buộc.")]
        public double DANHGIA { get; set; }

        [Display(Name = "Mô tả")]
        [Required(ErrorMessage = "Mô tả là bắt buộc.")]
        [MaxLength(500, ErrorMessage = "Mô tả tối đa 500 kí tự.")]
        public string MOTA { get; set; }

        [Display(Name = "Ảnh")]
        [Required(ErrorMessage = "Ảnh là bắt buộc.")]
        [MaxLength(150, ErrorMessage = "Tên file ảnh tối đa 150 kí tự.")]
        public string ANH { get; set; }

        [Display(Name = "Mã danh mục sản phẩm")]
        [Required(ErrorMessage = "Mã danh mục sản phẩm là bắt buộc.")]
        public int MADMSP { get; set; }

        [Display(Name = "Mã thương hiệu")]
        [Required(ErrorMessage = "Mã thương hiệu là bắt buộc.")]
        public int MATH { get; set; }

        [Display(Name = "Mã thông số kỹ thuật")]
        [Required(ErrorMessage = "Mã thông số kỹ thuật là bắt buộc.")]
        public int MATSKT { get; set; }

        // Liên kết với các bảng khác
        [ForeignKey("MADMSP")]
        public virtual DanhMucSPAdmin DanhMucSP { get; set; }

        [ForeignKey("MATH")]
        public virtual ThuongHieuAdmin ThuongHieu { get; set; }

        [ForeignKey("MATSKT")]
        public virtual ThongSoKyThuatAdmin ThongSoKyThuat { get; set; }
    }

    public class SanPhamFormAdmin : SanPhamAdmin
    {
        // Danh mục sản phẩm được chọn (Mã danh mục)
        [Display(Name = "Chọn danh mục sản phẩm")]
        public int? MaDanhMucSP { get; set; }

        // Danh sách danh mục sản phẩm cho dropdown
        public List<SelectListItem>? DanhMucSPList { get; set; }

        // Thương hiệu được chọn (Mã thương hiệu)
        [Display(Name = "Chọn thương hiệu")]
        public int? MaThuongHieu { get; set; }

        // Danh sách thương hiệu cho dropdown
        public List<SelectListItem>? ThuongHieuList { get; set; }

        // Thông số kỹ thuật được chọn (Mã thông số)
        [Display(Name = "Chọn thông số kỹ thuật")]
        public int? MaThongSoKyThuat { get; set; }

        // Danh sách thông số kỹ thuật cho dropdown
        public List<SelectListItem>? ThongSoKyThuatList { get; set; }
    }
    public class SanPhamAdminModel
    {
        ///<summary>
        /// Gets or sets ProductAdmin.
        ///</summary>
        public List<SanPhamAdmin> SanPhamAdmins { get; set; }

        ///<summary>
        /// Gets or sets CurrentPageIndex.
        ///</summary>
        public int CurrentPageIndex { get; set; }

        ///<summary>
        /// Gets or sets PageCount.
        ///</summary>
        public int PageCount { get; set; }
    }
}

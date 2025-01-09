using GM_Shop.Areas.Admin.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GM_Shop.Models
{
    public class SanPham
    {
        [Key]
        public int MASP { get; set; }
        public string TENSP { get; set; }
        public decimal DONGIA { get; set; }
        public int SOLUONG { get; set; }
        public string MOTA { get; set; }
        public string ANH { get; set; }
        public double DANHGIA {  get; set; }
        public int MADMSP { get; set; }
        public int MATH { get; set; }
        public int MATSKT { get; set; }

        // Liên kết với các bảng khác
        [ForeignKey("MADMSP")]
        public virtual DanhMucSPAdmin DanhMucSP { get; set; }

        [ForeignKey("MATH")]
        public virtual ThuongHieuAdmin ThuongHieu { get; set; }

        [ForeignKey("MATSKT")]
        public virtual ThongSoKyThuatAdmin ThongSoKyThuat { get; set; }
    }
    public class SanPhamPagination
    {
        public List<SanPham> sanPhams { get; set; }
        public int CurrentPageIndex { get; set; }
        public int PageCount { get; set; }
    }
}

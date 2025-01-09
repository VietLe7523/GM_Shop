using System.ComponentModel.DataAnnotations.Schema;

namespace GM_Shop.Areas.Admin.Models
{
    public class ChiTietDonHangAdmin
    {
        public int OrderId { get; set; } // MADONHANG
        public int ProductId { get; set; } // MASP
        public int Price { get; set; } // GIA
        public int Quantity { get; set; } // SOLUONG
        public double TotalPrice { get; set; } // TONGTIEN
        public DateTime CreatedDate { get; set; } // NGAYTAO
        [ForeignKey("ProductId")]
        public virtual SanPhamAdmin SanPham { get; set; }
    }
}

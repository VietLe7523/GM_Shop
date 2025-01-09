namespace GM_Shop.Areas.Admin.Models
{
    public class DonHangAdmin
    {
        public int OrderId { get; set; } // MADONHANG
        public int CustomerId { get; set; } // MAKH
        public DateTime? CreatedDate { get; set; } // NGAYTAO
        public double TotalAmount { get; set; } // TONGTIEN
        public string CustomerLastName { get; set; } // HOKH
        public string CustomerFirstName { get; set; } // TENKH
        public string Phone { get; set; } // SDT
        public string Email { get; set; } // EMAIL
        public List<ChiTietDonHangAdmin> ChiTietDonHangs { get; set; } // Chi tiết đơn hàng
    }
    public class DonHangAdminModel
    {
        ///<summary>
        /// Gets or sets ProductAdmin.
        ///</summary>
        public List<DonHangAdmin> DonHangAdmins { get; set; }

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

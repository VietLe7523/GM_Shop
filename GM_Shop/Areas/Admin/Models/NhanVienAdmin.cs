using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GM_Shop.Areas.Admin.Models
{
    public class NhanVienAdmin
    {
        [Key]
        public int MANV { get; set; }
        public int UserId { get; set; }

        [Display(Name = "Họ")]
        [Required(ErrorMessage = "*")]
        [MaxLength(100, ErrorMessage = "Tối đa 50 kí tự")]
        public string HONV { get; set; }

        [Display(Name = "Tên")]
        [Required(ErrorMessage = "*")]
        [MaxLength(100, ErrorMessage = "Tối đa 15 kí tự")]
        public string TENNV { get; set; }

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "*")]
        [MaxLength(500, ErrorMessage = "Tối đa 100 kí tự")]
        public string DIACHI { get; set; }

        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "*")]
        [MaxLength(15, ErrorMessage = "Tối đa 50 kí tự")]
        public string SDT { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "*")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
        public string EMAIL { get; set; }
        public string GIOITINH { get; set; }
        public string ANH { get; set; }

        [Display(Name = "Ngày sinh")]
        public DateOnly? NGAYSINH { get; set; }

        [Display(Name = "Căn cước công dân")]
        [Required(ErrorMessage = "*")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
        public string CCCD { get; set; }
        public int RoleID { get; set; }
        // Liên kết với các bảng khác
        [ForeignKey("UserID")]
        public UserAdmin User { get; set; } // Navigation property tới bảng USERS
        [ForeignKey("RoleID")]
        public RoleAdmin Role { get; set; } // Navigation property tới bảng Role
    }
    public class NhanVienFormAdmin : NhanVienAdmin
    {
        // Role được chọn (RoleID)
        [Display(Name = "Chọn Role")]
        public int? MaRole { get; set; }

        // Danh sách Role cho dropdown
        public List<SelectListItem>? RoleList { get; set; }

    }
    public class NhanVienAdminModel
    {
        ///<summary>
        /// Gets or sets ProductAdmin.
        ///</summary>
        public List<NhanVienAdmin> NhanVienAdmins { get; set; }

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

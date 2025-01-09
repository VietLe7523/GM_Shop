using GM_Shop.Areas.Admin.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GM_Shop.Models
{
    public class NhanVien
    {
        [Key]
        public int MANV { get; set; }
        public int UserID { get; set; }

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
        public User User { get; set; } // Navigation property tới bảng USERS
        [ForeignKey("RoleID")]
        public Role Role { get; set; } // Navigation property tới bảng Role
    }
}

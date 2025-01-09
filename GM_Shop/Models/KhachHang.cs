using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GM_Shop.Models
{
    public class KhachHang
    {
        [Key]
        public int MAKH { get; set; }
        public int UserID { get; set; }

        [Display(Name = "Họ")]
        [Required(ErrorMessage = "*")]
        [MaxLength(100, ErrorMessage = "Tối đa 50 kí tự")]
        public string HOKH { get; set; }

        [Display(Name = "Tên")]
        [Required(ErrorMessage = "*")]
        [MaxLength(100, ErrorMessage = "Tối đa 15 kí tự")]
        public string TENKH { get; set; }

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
        // Liên kết với các bảng khác
        [ForeignKey("UserID")]
        public User User { get; set; } // Navigation property tới bảng USERS
    }
}

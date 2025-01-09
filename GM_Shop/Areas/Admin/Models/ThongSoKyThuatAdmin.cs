using System.ComponentModel.DataAnnotations;

namespace GM_Shop.Areas.Admin.Models
{
    public class ThongSoKyThuatAdmin
    {
        [Key]
        public int MATSKT { get; set; } // Mã thông số kỹ thuật (khóa chính)
        [Display(Name = "TENTSKT")]
        [Required(ErrorMessage = "TENTSKT là bắt buộc.")]
        [MaxLength(100, ErrorMessage = "TENTSKT tối đa 100 ký tự.")]
        public string TENTSKT { get; set; } // Hệ điều hành

        [Display(Name = "HEDIEUHANH")]
        [Required(ErrorMessage = "Hệ điều hành là bắt buộc.")]
        [MaxLength(10, ErrorMessage = "Hệ điều hành tối đa 10 ký tự.")]
        public string HEDIEUHANH { get; set; } // Hệ điều hành

        [Display(Name = "RAM")]
        [Required(ErrorMessage = "RAM là bắt buộc.")]
        [Range(1, int.MaxValue, ErrorMessage = "RAM phải lớn hơn 0.")]
        public int RAM { get; set; } // RAM

        [Display(Name = "ROM")]
        [Required(ErrorMessage = "ROM là bắt buộc.")]
        [MaxLength(10, ErrorMessage = "ROM tối đa 10 ký tự.")]
        public string ROM { get; set; } // Bộ nhớ trong

        [Display(Name = "KICHCOMANHINH")]
        [Required(ErrorMessage = "Kích cỡ màn hình là bắt buộc.")]
        [Range(0.1, float.MaxValue, ErrorMessage = "Kích cỡ màn hình phải lớn hơn 0.")]
        public float KICHCOMANHINH { get; set; } // Kích cỡ màn hình

        [Display(Name = "VIXULY")]
        [Required(ErrorMessage = "Vi xử lý là bắt buộc.")]
        [MaxLength(30, ErrorMessage = "Tên vi xử lý tối đa 30 ký tự.")]
        public string VIXULY { get; set; } // Tên vi xử lý

        [Display(Name = "PIN")]
        [Required(ErrorMessage = "Dung lượng pin là bắt buộc.")]
        [Range(1, int.MaxValue, ErrorMessage = "Dung lượng pin phải lớn hơn 0.")]
        public int PIN { get; set; } // Pin

        [Display(Name = "CAMERA")]
        [Required(ErrorMessage = "Camera là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Thông số camera tối đa 50 ký tự.")]
        public string CAMERA { get; set; } // Thông số camera
    }
    public class ThongSoKyThuatAdminModel
    {
        ///<summary>
        /// Gets or sets ProductAdmin.
        ///</summary>
        public List<ThongSoKyThuatAdmin> ThongSoKyThuatAdmins { get; set; }

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

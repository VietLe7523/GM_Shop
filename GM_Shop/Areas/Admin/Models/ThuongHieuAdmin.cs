using System.ComponentModel.DataAnnotations;

namespace GM_Shop.Areas.Admin.Models
{
    public class ThuongHieuAdmin
    {
        [Key]
        public int MATH { get; set; }
        [Display(Name = "TENTHUONGHIEU")]
        [Required(ErrorMessage = "Tên thương hiệu là bắt buộc.")]
        [MaxLength(20, ErrorMessage = "Tên thương hiệu tối đa 20 kí tự.")]
        public string TENTHUONGHIEU { get; set; }

        [Display(Name = "QUOCGIA")]
        [Required(ErrorMessage = "Tên quốc gia là bắt buộc.")]
        [MaxLength(20, ErrorMessage = "Tên quốc gia tối đa 30 kí tự.")]
        public string QUOCGIA { get; set; }
    }
    public class ThuongHieuAdminModel
    {
        ///<summary>
        /// Gets or sets ProductAdmin.
        ///</summary>
        public List<ThuongHieuAdmin> ThuongHieuAdmins { get; set; }

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

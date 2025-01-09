using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace GM_Shop.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        [Display(Name = "Tài khoản")]
        [Required(ErrorMessage = "Tài khoản là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Tài khoản tối đa 50 kí tự.")]
        public string TAIKHOAN { get; set; }
        [Display(Name = "Tên sản phẩm")]
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Tên sản phẩm tối đa 50 kí tự.")]
        public string MATKHAU { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace GM_Shop.Models
{
    public class ChangePassword
    {
        [Display(Name = "Tài khoản")]
        public string TAIKHOAN { get; set; }
        [Display(Name = "Mật khẩu cũ")]
        public string MATKHAUCU { get; set; }
        [Display(Name = "Mật khẩu mới")]
        public string MATKHAUMOI { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace GM_Shop.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }
        [Display(Name = "RoleName")]
        [Required(ErrorMessage = "Tên vai trò là bắt buộc.")]
        [MaxLength(20, ErrorMessage = "Tên vai trò tối đa 20 kí tự.")]
        public string RoleName { get; set; }
    }
}

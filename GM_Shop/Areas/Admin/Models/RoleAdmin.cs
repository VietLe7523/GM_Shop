using System.ComponentModel.DataAnnotations;

namespace GM_Shop.Areas.Admin.Models
{
    public class RoleAdmin
    {
        [Key]
        public int RoleID { get; set; }
        [Display(Name = "RoleName")]
        [Required(ErrorMessage = "Tên vai trò là bắt buộc.")]
        [MaxLength(20, ErrorMessage = "Tên vai trò tối đa 20 kí tự.")]
        public string RoleName { get; set; }
    }
    public class RoleAdminModel
    {
        ///<summary>
        /// Gets or sets ProductAdmin.
        ///</summary>
        public List<RoleAdmin> RoleAdmins { get; set; }

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

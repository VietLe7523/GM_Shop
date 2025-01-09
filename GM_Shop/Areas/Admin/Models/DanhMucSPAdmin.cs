using System.ComponentModel.DataAnnotations;

namespace GM_Shop.Areas.Admin.Models
{
    public class DanhMucSPAdmin
    {
        [Key]
        public int MADMSP { get; set; }
        [Display(Name = "TENDMSP")]
        [Required(ErrorMessage = "Tên danh mục sản phẩm là bắt buộc.")]
        [MaxLength(20, ErrorMessage = "Tên danh mục sản phẩm tối đa 20 kí tự.")]
        public string TENDMSP { get; set; }
    }
    public class DanhMucSPAdminModel
    {
        ///<summary>
        /// Gets or sets ProductAdmin.
        ///</summary>
        public List<DanhMucSPAdmin> DanhMucSPAdmins { get; set; }

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

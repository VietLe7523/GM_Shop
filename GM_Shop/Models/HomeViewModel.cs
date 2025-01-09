namespace GM_Shop.Models
{
    public class HomeViewModel
    {
        public List<MenuDanhMuc> DanhMucList { get; set; }
        public List<SanPham> FeaturedProducts { get; set; }
        public Dictionary<int, (string TENDMSP, List<SanPham> Products)> ProductsByCategory { get; set; }
    }
}

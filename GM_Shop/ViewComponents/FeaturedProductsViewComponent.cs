using GM_Shop.DAL;
using GM_Shop.Models;
using Microsoft.AspNetCore.Mvc;

namespace GM_Shop.ViewComponents
{
    public class FeaturedProductsViewComponent: ViewComponent
    {
        SanPhamDAL sanPhamDAL = new SanPhamDAL();
        public IViewComponentResult Invoke(int? limit)
        {
            int limitProduct = limit ?? 4;

            List<SanPham> featuredProducts = new List<SanPham>();

            featuredProducts = sanPhamDAL.GetFeaturedProducts(limitProduct);

            return View("FeatureProduct", featuredProducts);
        }
    }
}

using GM_Shop.DAL;
using GM_Shop.Models;
using Microsoft.AspNetCore.Mvc;

namespace GM_Shop.ViewComponents
{
    public class RelatedProductsViewComponent: ViewComponent
    {
        SanPhamDAL sanPhamDAL = new SanPhamDAL();
        public IViewComponentResult Invoke(int maDMSP, int currentMASP, int? limit)
        {
            int limitProduct = limit ?? 4;
            var relatedProducts = sanPhamDAL.GetRelatedProducts(maDMSP, currentMASP);

            // Đảm bảo không trả về null
            if (relatedProducts == null)
            {
                relatedProducts = new List<SanPham>();
            }

            return View("RelatedProduct", relatedProducts);
        }
    }
}

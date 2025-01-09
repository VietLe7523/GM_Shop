using GM_Shop.Helper;
using GM_Shop.Models;
using Microsoft.AspNetCore.Mvc;

namespace GM_Shop.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Lấy danh sách giỏ h 
            var cart = HttpContext.Session.Get<List<CartItem>>(MyConst.CART_KEY)
            ?? new List<CartItem>();

            return View(new CartModel()
            {
                Quantity = cart.Sum(p => p.SOLUONG),
                Total = cart.Sum(p => p.Total)
            });
        }
    }
}

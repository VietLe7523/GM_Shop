using GM_Shop.DAL;
using GM_Shop.Models;
using Microsoft.AspNetCore.Mvc;

namespace GM_Shop.ViewComponents
{
    public class MenuThuongHieuViewComponent: ViewComponent
    {
        // query SQL 
        ThuongHieuDAL thuongHieuDAL = new ThuongHieuDAL();
        public IViewComponentResult Invoke()
        {
            List<MenuThuongHieu> menuThuongHieus = new List<MenuThuongHieu>();

            menuThuongHieus = thuongHieuDAL.GetAllWithCount();
            return View("Default", menuThuongHieus);
        }
    }
}

using GM_Shop.DAL;
using GM_Shop.Models;
using Microsoft.AspNetCore.Mvc;

namespace GM_Shop.ViewComponents
{
    public class MenuDanhMucViewComponent: ViewComponent
    {
        // query SQL 
        DanhMucDAL danhMucDAL = new DanhMucDAL();
        public IViewComponentResult Invoke()
        {
            List<MenuDanhMuc> menuDanhMucs = new List<MenuDanhMuc>();

            menuDanhMucs = danhMucDAL.getAllWithCount();
            return View("Default", menuDanhMucs);
        }
    }
}

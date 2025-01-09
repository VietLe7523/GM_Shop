using Microsoft.AspNetCore.Mvc;

namespace GM_Shop.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

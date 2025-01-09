using GM_Shop.DAL;
using GM_Shop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GM_Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        DanhMucDAL danhMucDAL = new DanhMucDAL();
        SanPhamDAL sanPhamDAL = new SanPhamDAL();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var danhMucList = danhMucDAL.getAllWithCount();
            var featuredProducts = sanPhamDAL.GetFeaturedProducts(4);
            var productsByCategory = new Dictionary<int, (string TENDMSP, List<SanPham> Products)>();

            foreach (var category in danhMucList)
            {
                var products = sanPhamDAL.GetSPByDMSP(category.MADMSP);
                productsByCategory.Add(category.MADMSP, (category.TENDMSP, products));
            }

            var viewModel = new HomeViewModel
            {
                DanhMucList = danhMucList,
                FeaturedProducts = featuredProducts,
                ProductsByCategory = productsByCategory
            };

            return View(viewModel);
        }


        public IActionResult Privacy()
        {
            return View();
        }
        [Route("/404")]
        public IActionResult PageNotfound()
        {
            return View();
        }
        [Route("/AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

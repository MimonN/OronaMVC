using Microsoft.AspNetCore.Mvc;
using OronaMVC.DataAccess.Repository.IRepository;
using OronaMVC.Models;
using OronaMVC.Models.ViewModels;
using System.Diagnostics;

namespace OronaMVC.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWOrk;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWOrk = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<WindowType> windowTypeList = await _unitOfWOrk.WindowType.GetAllWindowTypesWithProductsWithCleaningTypes();

            return View(windowTypeList);
        }

        public async Task<IActionResult> Details(int id)
        {
            ShoppingCart cartObj = new()
            {
                Count = 1,
                Product = await _unitOfWOrk.Product.GetFirstOrDefaultAsync(x => x.Id == id, includeProperties: "WindowType,CleaningType")
            };

            return View(cartObj);
        }

        public IActionResult Privacy()
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OronaMVC.DataAccess.Repository.IRepository;
using OronaMVC.Models;
using OronaMVC.Models.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

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

        public async Task<IActionResult> Details(int productId)
        {
            ShoppingCart cartObj = new()
            {
                Count = 1,
                ProductId = productId,
                Product = await _unitOfWOrk.Product.GetFirstOrDefaultAsync(x => x.Id == productId, includeProperties: "WindowType,CleaningType")
            };

            return View(cartObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Details(ShoppingCart shoppingCart)
        {
            //extracting userId from ClaimsIdentity
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;

            ShoppingCart cartFromDb = await _unitOfWOrk.ShoppingCart.GetFirstOrDefaultAsync(
                u => u.ApplicationUserId == claim.Value && u.ProductId==shoppingCart.ProductId);

            if(cartFromDb == null)
            {
                await _unitOfWOrk.ShoppingCart.AddAsync(shoppingCart);
            }
            else
            {
                _unitOfWOrk.ShoppingCart.IncrementCount(cartFromDb, shoppingCart.Count);
            }

            await _unitOfWOrk.SaveAsync();

            return RedirectToAction(nameof(Index));
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
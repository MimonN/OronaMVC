using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OronaMVC.DataAccess.Repository.IRepository;
using OronaMVC.Models.ViewModels;
using System.Security.Claims;

namespace OronaMVC.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = await _unitOfWork.ShoppingCart.GetAllShopCartWithProductWithWindowTypeAndCleaningType()
            };

            foreach(var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = cart.Product.Price;
                ShoppingCartVM.CartTotal += (cart.Count * cart.Price);
            }

            return View(ShoppingCartVM);
        }

		public IActionResult Summary()
		{
			//var claimsIdentity = (ClaimsIdentity)User.Identity;
			//var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			//ShoppingCartVM = new ShoppingCartVM()
			//{
			//	ListCart = await _unitOfWork.ShoppingCart.GetAllShopCartWithProductWithWindowTypeAndCleaningType()
			//};

			//foreach (var cart in ShoppingCartVM.ListCart)
			//{
			//	cart.Price = cart.Product.Price;
			//	ShoppingCartVM.CartTotal += (cart.Count * cart.Price);
			//}

			return View();
		}

		public async Task<IActionResult> Plus(int cartId)
        {
            var cart = await _unitOfWork.ShoppingCart.GetFirstOrDefaultAsync(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.IncrementCount(cart, 1);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

		public async Task<IActionResult> Minus(int cartId)
		{
			var cart = await _unitOfWork.ShoppingCart.GetFirstOrDefaultAsync(u => u.Id == cartId);
            if(cart.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cart);
            }
            else
            {
				_unitOfWork.ShoppingCart.DecrementCount(cart, 1);
			}
			await _unitOfWork.SaveAsync();
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Remove(int cartId)
		{
			var cart = await _unitOfWork.ShoppingCart.GetFirstOrDefaultAsync(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cart);
			await _unitOfWork.SaveAsync();
			return RedirectToAction(nameof(Index));
		}
	}
}

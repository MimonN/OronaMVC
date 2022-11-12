using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OronaMVC.DataAccess.Repository.IRepository;
using OronaMVC.Models;
using OronaMVC.Models.ViewModels;
using OronaMVC.Utility;
using System.Security.Claims;

namespace OronaMVC.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
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
                ListCart = await _unitOfWork.ShoppingCart.GetAllShopCartBasedOnClaim(claim.Value),
                OrderHeader = new()
            };

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = cart.Product.Price;
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Count * cart.Price);
            }

            return View(ShoppingCartVM);
        }

        public async Task<IActionResult> Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = await _unitOfWork.ShoppingCart.GetAllShopCartBasedOnClaim(claim.Value),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = await _unitOfWork.ApplicationUser.GetFirstOrDefaultAsync(u => u.Id == claim.Value);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = cart.Product.Price;
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Count * cart.Price);
            }

            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SummaryPOST(ShoppingCartVM ShoppingCartVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM.ListCart = await _unitOfWork.ShoppingCart.GetAllShopCartBasedOnClaim(claim.Value);

            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusNew;
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = cart.Product.Price;
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Count * cart.Price);
            }

            await _unitOfWork.OrderHeader.AddAsync(ShoppingCartVM.OrderHeader);
            await _unitOfWork.SaveAsync();

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderId = ShoppingCartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };
                await _unitOfWork.OrderDetail.AddAsync(orderDetail);
                await _unitOfWork.SaveAsync();
            }

            _unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVM.ListCart);
            await _unitOfWork.SaveAsync();

            return RedirectToAction("OrderConfirmation", new { ShoppingCartVM.OrderHeader.Id });
        }


        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
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

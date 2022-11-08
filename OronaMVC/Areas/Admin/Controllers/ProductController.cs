using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OronaMVC.DataAccess;
using OronaMVC.DataAccess.Repository.IRepository;
using OronaMVC.Models;

namespace OronaMVC.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
			_unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> objProduct = await _unitOfWork.Product.GetAllAsync(includeProperties:"CleaningType,WindowType");

            return View(objProduct);
        }

        public async Task<IActionResult> Create()
        {
            Product product = new();
            var cleaningTypesFromDb = await _unitOfWork.CleaningType.GetAllAsync();
            IEnumerable<SelectListItem> CleaningTypeList = cleaningTypesFromDb.Select(
                u => new SelectListItem
                {
                    Text = u.CleaningName,
                    Value = u.Id.ToString()
                });
            var windowTypesFromDb = await _unitOfWork.WindowType.GetAllAsync();
            IEnumerable<SelectListItem> WindowTypeList = windowTypesFromDb.Select(
                u => new SelectListItem
                {
                    Text = u.WindowTypeName,
                    Value = u.Id.ToString()
                });

            ViewBag.WindowTypeList = WindowTypeList;
            ViewData["CleaningTypeList"] = CleaningTypeList;

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product obj)
        {
            if (ModelState.IsValid)
            {
                var objFromDb = await _unitOfWork.Product.ProductExistAsync(obj);

                if (objFromDb == null)
                {
                    await _unitOfWork.Product.AddAsync(obj);
                    await _unitOfWork.SaveAsync();
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Product", "Product already exists.");

					var cleaningTypesFromDb = await _unitOfWork.CleaningType.GetAllAsync();
					IEnumerable<SelectListItem> CleaningTypeList = cleaningTypesFromDb.Select(
						u => new SelectListItem
						{
							Text = u.CleaningName,
							Value = u.Id.ToString()
						});
					var windowTypesFromDb = await _unitOfWork.WindowType.GetAllAsync();
					IEnumerable<SelectListItem> WindowTypeList = windowTypesFromDb.Select(
						u => new SelectListItem
						{
							Text = u.WindowTypeName,
							Value = u.Id.ToString()
						});

					ViewBag.WindowTypeList = WindowTypeList;
					ViewData["CleaningTypeList"] = CleaningTypeList;
				}

            }
            return View(obj);

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var productFromDb = await _unitOfWork.Product.GetFirstOrDefaultAsync(u => u.Id == id, includeProperties: "CleaningType,WindowType");
            if (productFromDb == null)
            {
                return NotFound();
            }
			return View(productFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product obj)
        {
            var objFromDb = await _unitOfWork.Product.GetFirstOrDefaultAsync(u => u.Id == obj.Id, includeProperties: "CleaningType,WindowType");
			if (objFromDb != null)
			{
                objFromDb.Price = obj.Price;
                objFromDb.Description = obj.Description;
				await _unitOfWork.Product.UpdateAsync(objFromDb);
				await _unitOfWork.SaveAsync();
				TempData["success"] = "Product updated successfully";
				return RedirectToAction("Index");
			}
			else
			{
				ModelState.AddModelError("Product", "Product already exists.");
			}
			return View(objFromDb);
		}

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var productFromDb = await _unitOfWork.Product.GetFirstOrDefaultAsync(u => u.Id == id, includeProperties: "CleaningType,WindowType");
            if (productFromDb == null)
            {
                return NotFound();
            }

            return View(productFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            var obj = await _unitOfWork.Product.GetFirstOrDefaultAsync(u => u.Id == id, includeProperties: "CleaningType,WindowType");
            if (obj == null)
            {
                return NotFound();
            }

			_unitOfWork.Product.Remove(obj);
            await _unitOfWork.SaveAsync();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }
    }
}

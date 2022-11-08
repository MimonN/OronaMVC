using Microsoft.AspNetCore.Mvc;
using OronaMVC.DataAccess;
using OronaMVC.DataAccess.Repository.IRepository;
using OronaMVC.Models;

namespace OronaMVC.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
			IEnumerable<Company> objCompany = await _unitOfWork.Company.GetAllAsync();

			return View(objCompany);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Company obj)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Company.AddAsync(obj);
                await _unitOfWork.SaveAsync();
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var companyFromDb = await _unitOfWork.Company.GetFirstOrDefaultAsync(u => u.Id == id);
            if (companyFromDb == null)
            {
                return NotFound();
            }
            return View(companyFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Company obj)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Company.UpdateAsync(obj);
                await _unitOfWork.SaveAsync();
                TempData["success"] = "Company updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var companyFromDb = await _unitOfWork.Company.GetFirstOrDefaultAsync(u => u.Id == id);
            if (companyFromDb == null)
            {
                return NotFound();
            }

            return View(companyFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            var obj = await _unitOfWork.Company.GetFirstOrDefaultAsync(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Company.Remove(obj);
            await _unitOfWork.SaveAsync();
            TempData["success"] = "Company deleted successfully";
            return RedirectToAction("Index");
        }
    }
}

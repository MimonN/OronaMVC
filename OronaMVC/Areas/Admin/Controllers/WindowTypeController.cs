using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OronaMVC.DataAccess;
using OronaMVC.DataAccess.Repository;
using OronaMVC.DataAccess.Repository.IRepository;
using OronaMVC.Models;

namespace OronaMVC.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WindowTypeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public WindowTypeController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<WindowType> objWindowType = await _unitOfWork.WindowType.GetAllAsync();

            return View(objWindowType);
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            WindowType windowType = new();
            if (id == null || id == 0)
            {
                return View(windowType);
            }
            else
            {
                var objWindowTypeFromDb = await _unitOfWork.WindowType.GetFirstOrDefaultAsync(x => x.Id == id);
                return View(objWindowTypeFromDb);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(WindowType obj, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\windowTypes");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.ImageUrl = @"\images\windowTypes\" + fileName + extension;
                }
                if (obj.Id == 0)
                {
                    await _unitOfWork.WindowType.AddAsync(obj);
                }
                else
                {
                    await _unitOfWork.WindowType.UpdateAsync(obj);
                    await _unitOfWork.SaveAsync();
                    TempData["success"] = "Window Type updated successfully";
                    return RedirectToAction("Index");
                }
                await _unitOfWork.SaveAsync();
                TempData["success"] = "Window Type created successfully";
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

            var windowTypeFromDb = await _unitOfWork.WindowType.GetFirstOrDefaultAsync(u => u.Id == id);
            if (windowTypeFromDb == null)
            {
                return NotFound();
            }

            return View(windowTypeFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            var obj = await _unitOfWork.WindowType.GetFirstOrDefaultAsync(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

			_unitOfWork.WindowType.Remove(obj);
            await _unitOfWork.SaveAsync();
            TempData["success"] = "Window Type deleted successfully";
            return RedirectToAction("Index");
        }
    }
}

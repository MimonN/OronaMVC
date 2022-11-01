using Microsoft.AspNetCore.Mvc;
using OronaMVC.DataAccess;
using OronaMVC.DataAccess.Repository.IRepository;
using OronaMVC.Models;

namespace OronaMVC.Controllers
{
    public class CleaningTypeController : Controller
    {
        private readonly ICleaningTypeRepository _db;

        public CleaningTypeController(ICleaningTypeRepository db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<CleaningType> objCleaningType = await _db.GetAllAsync();

            return View(objCleaningType);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CleaningType obj)
        {
            if(ModelState.IsValid)
            {
                var objFromDb = await _db.GetFirstOrDefaultAsync(u => u.CleaningName == obj.CleaningName);
                if(objFromDb == null || objFromDb.CleaningName != obj.CleaningName)
                {
                    await _db.AddAsync(obj);
                    await _db.SaveAsync();
                    TempData["success"] = "Cleaning Type created successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("CleaningName", "Cleaning Type already exists.");
                }
                
            }
            return View(obj);
              
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var cleaningTypeFromDb = await _db.GetFirstOrDefaultAsync(u => u.Id == id);
            if(cleaningTypeFromDb == null) 
            {
                return NotFound();
            }
            return View(cleaningTypeFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CleaningType obj)
        {
			var objFromDb = await _db.GetFirstOrDefaultAsync(u => u.CleaningName == obj.CleaningName);
			if (objFromDb == null || objFromDb.CleaningName != obj.CleaningName)
			{
				await _db.UpdateAsync(obj);
				await _db.SaveAsync();
                TempData["success"] = "Cleaning Type updated successfully";
                return RedirectToAction("Index");
			}
			else
			{
				ModelState.AddModelError("CleaningName", "Cleaning Type already exists.");
			}
			return View(obj);
		}

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var cleaningTypeFromDb = await _db.GetFirstOrDefaultAsync(u => u.Id == id);
            if (cleaningTypeFromDb == null)
            {
                return NotFound();
            }

            return View(cleaningTypeFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            var obj = await _db.GetFirstOrDefaultAsync(u => u.Id == id);  
            if (obj == null)
            {
                return NotFound();
            }

            await _db.RemoveAsync(obj);
            await _db.SaveAsync();
            TempData["success"] = "Cleaning Type deleted successfully";
            return RedirectToAction("Index");
        }
    }
}

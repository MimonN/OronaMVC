using Microsoft.AspNetCore.Mvc;
using OronaMVC.DataAccess;
using OronaMVC.Models;

namespace OronaMVC.Controllers
{
    public class CleaningTypeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CleaningTypeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<CleaningType> objCleaningType = _db.CleaningTypes;

            return View(objCleaningType);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CleaningType obj)
        {
            if(ModelState.IsValid)
            {
                var objFromDb = _db.CleaningTypes.FirstOrDefault(u => u.CleaningName == obj.CleaningName);
                if(objFromDb == null || objFromDb.CleaningName != obj.CleaningName)
                {
                    _db.CleaningTypes.Add(obj);
                    _db.SaveChanges();
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

        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var cleaningTypeFromDb = _db.CleaningTypes.Find(id);
            if(cleaningTypeFromDb == null) 
            {
                return NotFound();
            }
            return View(cleaningTypeFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CleaningType obj)
        {
			var objFromDb = _db.CleaningTypes.FirstOrDefault(u => u.CleaningName == obj.CleaningName);
			if (objFromDb == null || objFromDb.CleaningName != obj.CleaningName)
			{
				_db.CleaningTypes.Update(obj);
				_db.SaveChanges();
                TempData["success"] = "Cleaning Type updated successfully";
                return RedirectToAction("Index");
			}
			else
			{
				ModelState.AddModelError("CleaningName", "Cleaning Type already exists.");
			}
			return View(obj);
		}

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var cleaningTypeFromDb = _db.CleaningTypes.Find(id);
            if (cleaningTypeFromDb == null)
            {
                return NotFound();
            }

            return View(cleaningTypeFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.CleaningTypes.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.CleaningTypes.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Cleaning Type deleted successfully";
            return RedirectToAction("Index");
        }
    }
}

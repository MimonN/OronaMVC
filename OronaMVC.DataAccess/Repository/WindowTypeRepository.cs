using Microsoft.EntityFrameworkCore;
using OronaMVC.DataAccess.Repository.IRepository;
using OronaMVC.Models;

namespace OronaMVC.DataAccess.Repository
{
    public class WindowTypeRepository : Repository<WindowType>, IWindowTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public WindowTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<IEnumerable<WindowType>> GetAllWindowTypesWithProductsWithCleaningTypes()
        {
            var result = await _db.WindowTypes.Include(u => u.Products).ThenInclude(c => c.CleaningType).ToListAsync();
            return result;
        }

        public async Task UpdateAsync(WindowType obj)
        {
            _db.WindowTypes.Update(obj);
            await _db.SaveChangesAsync();
        }
    }
}

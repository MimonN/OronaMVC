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

        public async Task UpdateAsync(WindowType obj)
        {
            _db.WindowTypes.Update(obj);
            await _db.SaveChangesAsync();
        }
    }
}

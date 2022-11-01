using OronaMVC.DataAccess.Repository.IRepository;
using OronaMVC.Models;

namespace OronaMVC.DataAccess.Repository
{
    public class CleaningTypeRepository : Repository<CleaningType>, ICleaningTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public CleaningTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(CleaningType obj)
        {
            _db.CleaningTypes.Update(obj);
            await _db.SaveChangesAsync();
        }
    }
}

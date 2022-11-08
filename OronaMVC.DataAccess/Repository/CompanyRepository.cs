using OronaMVC.DataAccess.Repository.IRepository;
using OronaMVC.Models;

namespace OronaMVC.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
	{
        private readonly ApplicationDbContext _db;

        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Company obj)
        {
            _db.Companies.Update(obj);
            await _db.SaveChangesAsync();
        }
    }
}

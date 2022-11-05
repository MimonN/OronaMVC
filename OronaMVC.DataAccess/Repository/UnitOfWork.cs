using OronaMVC.DataAccess.Repository.IRepository;

namespace OronaMVC.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            CleaningType = new CleaningTypeRepository(_db);
            WindowType = new WindowTypeRepository(_db);
            Product = new ProductRepository(_db);
        }
        public ICleaningTypeRepository CleaningType { get; private set; }
        public IWindowTypeRepository WindowType { get; private set; }
        public IProductRepository Product { get; private set; }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

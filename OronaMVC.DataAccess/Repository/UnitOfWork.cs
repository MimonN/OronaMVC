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
            Company = new CompanyRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
        }
        public ICleaningTypeRepository CleaningType { get; private set; }
        public IWindowTypeRepository WindowType { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICompanyRepository Company { get; private set; }

        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }

		public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

using OronaMVC.DataAccess.Repository.IRepository;
using OronaMVC.Models;
using OronaMVC.Models.ViewModels;

namespace OronaMVC.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _db;

        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

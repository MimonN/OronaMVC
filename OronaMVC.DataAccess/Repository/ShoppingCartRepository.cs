using Microsoft.EntityFrameworkCore;
using OronaMVC.DataAccess.Repository.IRepository;
using OronaMVC.Models;
using System.Security.Claims;

namespace OronaMVC.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _db;

        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public int DecrementCount(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count -= count;
            return shoppingCart.Count;
        }

		public int IncrementCount(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count += count;
            return shoppingCart.Count;
        }

		public async Task<IEnumerable<ShoppingCart>> GetAllShopCartBasedOnClaim(string id)
		{
            var result = await _db.ShoppingCarts.Where(u=>u.ApplicationUser.Id == id)
                .Include(i => i.Product).ThenInclude(c => c.WindowType)
                .Include(i => i.Product).ThenInclude(c => c.CleaningType)
                .ToListAsync();
            return result;
                
		}
	}
}

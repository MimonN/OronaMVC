using Microsoft.EntityFrameworkCore;
using OronaMVC.DataAccess.Repository.IRepository;
using OronaMVC.Models;
using System.Web.Mvc;

namespace OronaMVC.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Product> ProductExistAsync(Product obj)
        {
            var productExist = await _db.Products.AsNoTracking().Where(p => p.WindowTypeId == obj.WindowTypeId).FirstOrDefaultAsync(p => p.CleaningTypeId == obj.CleaningTypeId);
            return productExist;
        }

        public async Task UpdateAsync(Product obj)
        {
            _db.Products.Update(obj);
            await _db.SaveChangesAsync();
        }

    }
}

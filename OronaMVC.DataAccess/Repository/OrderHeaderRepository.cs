using Microsoft.EntityFrameworkCore;
using OronaMVC.DataAccess.Repository.IRepository;
using OronaMVC.Models;

namespace OronaMVC.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(OrderHeader obj)
        {
            _db.OrderHeaders.Update(obj);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateStatus(int id, string orderStatus)
        {
            var orderFromDb = await _db.OrderHeaders.FirstOrDefaultAsync(u=>u.Id== id);
            if(orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
            }
        }
    }
}

using OronaMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OronaMVC.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        Task UpdateAsync(OrderHeader obj);
        Task UpdateStatus(int id, string orderStatus);
    }
}

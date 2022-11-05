using OronaMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OronaMVC.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task UpdateAsync(Product obj);
        Task<Product> ProductExistAsync(Product obj);
    }
}

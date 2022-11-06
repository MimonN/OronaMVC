using OronaMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OronaMVC.DataAccess.Repository.IRepository
{
    public interface IWindowTypeRepository : IRepository<WindowType>
    {
        Task UpdateAsync(WindowType obj);
        Task<IEnumerable<WindowType>> GetAllWindowTypesWithProductsWithCleaningTypes();
    }
}

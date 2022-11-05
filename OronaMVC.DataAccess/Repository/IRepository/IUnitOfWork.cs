using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OronaMVC.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICleaningTypeRepository CleaningType { get; }
        IWindowTypeRepository WindowType { get; }
        IProductRepository Product { get; }

        Task SaveAsync();
    }
}

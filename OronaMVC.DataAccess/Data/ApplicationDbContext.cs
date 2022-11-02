using Microsoft.EntityFrameworkCore;
using OronaMVC.Models;

namespace OronaMVC.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<CleaningType> CleaningTypes { get; set; }
        public DbSet<WindowType> WindowTypes { get; set; }
    }
}

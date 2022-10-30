using Microsoft.EntityFrameworkCore;
using OronaMVC.Models;

namespace OronaMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<CleaningType> CleaningTypes { get; set; }
    }
}

using Majestic.WarehouseService.Repository.Configurations.Cars;
using Majestic.WarehouseService.Repository.Configurations.Internal;
using Microsoft.EntityFrameworkCore;

namespace Majestic.WarehouseService.Repository.Contexts
{
    public class WarehouseDbContext : DbContext
    {
        public WarehouseDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddInternalConfiguration();
            modelBuilder.AddCarsConfiguration();
        }
    }
}

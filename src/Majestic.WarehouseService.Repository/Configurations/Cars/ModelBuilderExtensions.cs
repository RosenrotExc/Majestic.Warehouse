using Majestic.WarehouseService.Repository.Configurations.Base.BaseExtensions;
using Majestic.WarehouseService.Repository.Models.Cars;
using Microsoft.EntityFrameworkCore;

namespace Majestic.WarehouseService.Repository.Configurations.Cars
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder AddCarsConfiguration(this ModelBuilder modelBuilder)
        {
            // Two lines below should be added to support audit log
            modelBuilder.AddEntityCodeConfiguration<CarEntityCode>();
            modelBuilder.AddEntityStateConfiguration<CarEntity, CarEntityState, CarEntityCode>();

            modelBuilder.Entity<CarEntity>(new CarConfiguration().Configure);

            return modelBuilder;
        }
    }
}

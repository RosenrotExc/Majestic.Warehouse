/*
 * We need this code to support audit log, you should not change it
 */

using Majestic.WarehouseService.Repository.Configurations.Base.BaseConfigurations;
using Majestic.WarehouseService.Repository.Models.Internal;
using Microsoft.EntityFrameworkCore;

namespace Majestic.WarehouseService.Repository.Configurations.Internal
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder AddInternalConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Initiator>(new EntityTypeConfiguration<Initiator>().Configure);
            modelBuilder.Entity<State>(new EntityTypeConfiguration<State>().Configure);

            return modelBuilder;
        }
    }
}

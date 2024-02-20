/*
 * We need this code to support audit log, you should not change it
 */

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Majestic.WarehouseService.Repository.Configurations.Base.BaseConfigurations
{
    public abstract class BaseConfiguration<T> where T : class
    {
        public abstract void Configure(EntityTypeBuilder<T> builder);
    }
}

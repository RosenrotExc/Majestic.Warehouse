/*
 * We need this code to support audit log, you should not change it
 */

using Majestic.WarehouseService.Repository.Models.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Majestic.WarehouseService.Repository.Configurations.Base.BaseConfigurations
{
    public class EntityTypeConfiguration<TBaseEntityType> 
        : BaseConfiguration<TBaseEntityType> 
            where TBaseEntityType : BaseEntityType
    {
        public override void Configure(EntityTypeBuilder<TBaseEntityType> builder)
        {
            builder.HasKey((TBaseEntityType x) => x.Id);
            builder.Property((TBaseEntityType x) => x.Id).ValueGeneratedOnAdd();
            builder.Property((TBaseEntityType x) => x.Value).HasMaxLength(100).IsRequired().IsUnicode();
            builder.HasIndex((TBaseEntityType x) => x.Value).IsUnique();
        }
    }
}

/*
 * We need this code to support audit log, you should not change it
 */

using Majestic.WarehouseService.Repository.Models.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Majestic.WarehouseService.Repository.Configurations.Base.BaseConfigurations
{
    public class EntityCodeConfiguration<TBaseEntityCode>
        : BaseConfiguration<TBaseEntityCode>
            where TBaseEntityCode : BaseEntityCode
    {
        public override void Configure(EntityTypeBuilder<TBaseEntityCode> builder)
        {
            builder.HasKey((TBaseEntityCode x) => x.Id);
            builder.Property((TBaseEntityCode x) => x.Id).ValueGeneratedOnAdd();
            builder.Property((TBaseEntityCode x) => x.Id).IsRequired();
            builder.HasIndex((TBaseEntityCode x) => x.Id).IsUnique();
            builder.Property((TBaseEntityCode x) => x.Value).HasMaxLength(100).IsRequired().IsUnicode();
            builder.HasIndex((TBaseEntityCode x) => x.Value ).IsUnique();
        }
    }
}

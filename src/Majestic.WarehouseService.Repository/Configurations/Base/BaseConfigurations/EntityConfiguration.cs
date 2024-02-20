/*
 * We need this code to support audit log, you should not change it
 */

using Majestic.WarehouseService.Repository.Models.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Majestic.WarehouseService.Repository.Configurations.Base.BaseConfigurations
{
    public class EntityConfiguration<TBaseEntity, TBaseEntityState, TBaseEntityCode>
        : BaseConfiguration<TBaseEntity>
            where TBaseEntity : class, IBaseEntity<TBaseEntityState, TBaseEntityCode>
                where TBaseEntityState : BaseEntityState
                where TBaseEntityCode : BaseEntityCode
    {
        public override void Configure(EntityTypeBuilder<TBaseEntity> builder)
        {
            builder.HasKey((x) => x.Id);
            builder.Property((x) => x.Id).ValueGeneratedOnAdd();
            builder.HasMany((x) => x.States).WithOne().HasForeignKey(x => x.EntityId);
            builder.HasIndex((x) => x.CodeId);
            builder.HasOne((x) => x.Code).WithMany().HasForeignKey((x) => x.CodeId);
        }
    }
}

/*
 * We need this code to support audit log, you should not change it
 */

using Majestic.WarehouseService.Repository.Models.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Majestic.WarehouseService.Repository.Configurations.Base.BaseConfigurations
{
    public class EntityStateConfiguration<TBaseEntityState>
        : BaseConfiguration<TBaseEntityState>
            where TBaseEntityState : BaseEntityState
    {
        public override void Configure(EntityTypeBuilder<TBaseEntityState> builder)
        {
            builder.HasKey((x) => x.Id);
            builder.Property((x) => x.Id).ValueGeneratedOnAdd();
            builder.Property((x) => x.RefId).IsRequired();
            builder.HasIndex((x) => x.RefId).IsUnique();
            builder.HasIndex((x) => new { x.EntityId });
            builder.HasIndex((x) => new { x.CreateDateTime, x.ExpireDateTime, x.EntityId });
            builder.Property((x) => x.InitiatorId).IsRequired();
            builder.HasOne((b) => b.Initiator).WithMany().HasForeignKey((x) => x.InitiatorId);
            builder.Property((x) => x.ETag).IsRequired();
            builder.Property((x) => x.StateId).IsRequired();
            builder.HasIndex((x) => x.StateId);
            builder.HasOne((e) => e.State).WithMany().HasForeignKey((x) => x.StateId);
            builder.Property((x) => x.Message).HasMaxLength(100).IsUnicode();
            builder.Property((x) => x.Task).IsRequired();
            builder.HasIndex((x) => new { x.InitiatorId, x.ETag, x.StateId }).IsUnique();
        }
    }
}

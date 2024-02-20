using Majestic.WarehouseService.Repository.Configurations.Base.BaseConfigurations;
using Majestic.WarehouseService.Repository.Models.Cars;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Majestic.WarehouseService.Repository.Configurations.Cars
{
    public class CarConfiguration : EntityConfiguration<CarEntity, CarEntityState>
    {
        public override void Configure(EntityTypeBuilder<CarEntity> builder)
        {
            base.Configure(builder);

            builder
                .Property(b => b.CarName)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(b => b.ModelName)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(b => b.OwnerName)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(b => b.OwnersPrice)
                .IsRequired();

            builder
                .Property(b => b.DealersPrice)
                .IsRequired();

            builder
                .Property(b => b.DealerNotes)
                .IsRequired()
                .HasMaxLength(1000);
        }
    }
}

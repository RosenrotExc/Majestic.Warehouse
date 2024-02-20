/*
 * We need this code to support audit log, you should not change it
 */

using Majestic.WarehouseService.Repository.Configurations.Base.BaseConfigurations;
using Majestic.WarehouseService.Repository.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace Majestic.WarehouseService.Repository.Configurations.Base.BaseExtensions
{
    public static class BaseModelBuilderExtensions
    {
        public static ModelBuilder AddEntityStateConfiguration<TBaseEntity, TBaseEntityState, TBaseEntityCode>(this ModelBuilder modelBuilder)
            where TBaseEntity : BaseEntity<TBaseEntityState, TBaseEntityCode>
            where TBaseEntityState : BaseEntityState
            where TBaseEntityCode : BaseEntityCode
        {
            modelBuilder.Entity<TBaseEntity>(new EntityConfiguration<TBaseEntity, TBaseEntityState, TBaseEntityCode>().Configure);
            modelBuilder.Entity<TBaseEntityState>(new EntityStateConfiguration<TBaseEntityState>().Configure);
            return modelBuilder;
        }

        public static ModelBuilder AddEntityCodeConfiguration<TBaseEntityCode>(this ModelBuilder modelBuilder) 
            where TBaseEntityCode : BaseEntityCode
        {
            modelBuilder.Entity<TBaseEntityCode>(new EntityCodeConfiguration<TBaseEntityCode>().Configure);
            return modelBuilder;
        }
    }
}

using Majestic.WarehouseService.Repository.Models.Base;

namespace Majestic.WarehouseService.Repository.Extensions
{
    public static class QueryExtensions
    {
        public static IQueryable<TBaseEntityState> WhereNotExpired<TBaseEntityState>(this IQueryable<TBaseEntityState> pointInTimeQueryable, DateTime utcNow)
            where TBaseEntityState : IBaseEntityState
        {
            return pointInTimeQueryable.Where(x => x.CreateDateTime <= utcNow && (x.ExpireDateTime == null || x.ExpireDateTime > utcNow));
        }
    }
}

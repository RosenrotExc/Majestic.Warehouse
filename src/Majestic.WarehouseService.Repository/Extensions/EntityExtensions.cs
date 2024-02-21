using Majestic.WarehouseService.Repository.Models.Base;
using Majestic.WarehouseService.Repository.Models.Internal;

namespace Majestic.WarehouseService.Repository.Extensions
{
    public static class EntityExtensions
    {
        public static TBaseEntityState CreateState<TBaseEntityState, TBaseEntityCode>(this IBaseEntity<TBaseEntityState, TBaseEntityCode> request, State.Values state, 
            int initiatorId, DateTime utcNow, string message = null, string task = null, Guid? refId = null, Guid? eTag = null) 
            where TBaseEntityState : BaseEntityState, new()
            where TBaseEntityCode : BaseEntityCode, new()
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (!refId.HasValue)
            {
                refId = Guid.NewGuid();
            }

            if (!eTag.HasValue)
            {
                eTag = Guid.NewGuid();
            }

            return new TBaseEntityState
            {
                RefId = refId.Value,
                ETag = eTag.Value,
                EntityId = request.Id,
                StateId = (byte)state,
                InitiatorId = initiatorId,
                Message = message,
                Task = task,
                CreateDateTime = utcNow,
            };
        }

        public static TBaseEntityState AddState<TBaseEntityState, TBaseEntityCode>(this IBaseEntity<TBaseEntityState, TBaseEntityCode> request, State.Values state, int 
            initiatorId, DateTime utcNow, string message = null, string task = null, Guid? refId = null, Guid? eTag = null) 
            where TBaseEntityState : BaseEntityState, new()
            where TBaseEntityCode : BaseEntityCode, new()
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            TBaseEntityState val = request.CreateState(state, initiatorId, utcNow, message, task, refId, eTag);
            if (request.States == null)
            {
                request.States = new List<TBaseEntityState>();
            }

            request.States.Add(val);
            return val;
        }
    }
}

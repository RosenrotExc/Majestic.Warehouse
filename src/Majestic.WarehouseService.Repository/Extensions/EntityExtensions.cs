using Majestic.WarehouseService.Repository.Models.Base;
using Majestic.WarehouseService.Repository.Models.Internal;

namespace Majestic.WarehouseService.Repository.Extensions
{
    public static class EntityExtensions
    {
        public static TBaseEntityState CreateState<TBaseEntityState, TBaseEntityCode>(this IBaseEntity<TBaseEntityState, TBaseEntityCode> request, State.Values state, 
            int initiatorId, string message = null, Guid? refId = null) 
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

            return new TBaseEntityState
            {
                RefId = refId.Value,
                EntityId = request.Id,
                StateId = (byte)state,
                InitiatorId = initiatorId,
                Message = message
            };
        }

        public static TBaseEntityState AddState<TBaseEntityState, TBaseEntityCode>(this IBaseEntity<TBaseEntityState, TBaseEntityCode> request, State.Values state, int 
            initiatorId, string message = null, Guid? refId = null) 
            where TBaseEntityState : BaseEntityState, new()
            where TBaseEntityCode : BaseEntityCode, new()
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            TBaseEntityState val = request.CreateState(state, initiatorId, message, refId);
            if (request.States == null)
            {
                request.States = new List<TBaseEntityState>();
            }

            request.States.Add(val);
            return val;
        }
    }
}

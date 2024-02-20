using Majestic.WarehouseService.Repository.Models.Internal;

namespace Majestic.WarehouseService.Repository.Models.Base
{
    public abstract class BaseEntityState : IBaseEntityState
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public Guid RefId { get; set; }
        public Guid ETag { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? ExpireDateTime { get; set; }
        public int StateId { get; set; } 
        public State State { get; set; }
        public int InitiatorId { get; set; }
        public Initiator Initiator { get; set; }
        public string Task { get; set; }
        public string Message { get; set; }

        protected BaseEntityState()
        {
        }

        protected BaseEntityState(int id)
        {
            Id = id;
        }
    }
}

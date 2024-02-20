using Majestic.WarehouseService.Repository.Models.Internal;

namespace Majestic.WarehouseService.Repository.Models.Base
{
    public interface IBaseEntityState
    {
        int Id { get; set; }
        int EntityId { get; set; }
        Guid RefId { get; set; }
        Guid ETag { get; set; }
        DateTime CreateDateTime { get; set; }
        DateTime? ExpireDateTime { get; set; }
        int StateId { get; set; }
        State State { get; set; }
        int InitiatorId { get; set; }
        Initiator Initiator { get; set; }
        string Task { get; set; }
        string Message { get; set; }
    }
}

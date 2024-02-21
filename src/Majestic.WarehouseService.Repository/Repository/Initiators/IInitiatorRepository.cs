using Majestic.WarehouseService.Models.Misc;
using InternalContextModels = Majestic.WarehouseService.Repository.Models.Internal;

namespace Majestic.WarehouseService.Repository.Repository.Initiators
{
    public interface IInitiatorRepository
    {
        Task<ServiceResultWrapper<InternalContextModels.Initiator>> CreateOrGetInitiatorAsync(string subjectId);
    }
}

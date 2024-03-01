using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.ProcessCarSell.Event;

namespace Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarHandler
{
    public interface IProcessSellCardHandler
    {
        Task<ServiceResult> HandleAsync(ProcessSellCarEvent request);
    }
}

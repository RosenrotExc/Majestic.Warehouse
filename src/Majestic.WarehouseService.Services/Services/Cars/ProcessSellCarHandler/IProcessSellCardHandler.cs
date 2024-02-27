using Majestic.WarehouseService.Models.v1.ProcessCarSell.Event;

namespace Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarHandler
{
    public interface IProcessSellCardHandler
    {
        Task HandleAsync(ProcessSellCarEvent request);
    }
}

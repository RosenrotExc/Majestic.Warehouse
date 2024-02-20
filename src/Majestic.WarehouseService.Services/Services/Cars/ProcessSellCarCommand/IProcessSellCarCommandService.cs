using Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarCommand.CommandModels;
using Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarCommand.Result;

namespace Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarCommand
{
    public interface IProcessSellCarCommandService
    {
        Task<ProcessCarFlowResult> HandleAsync(ProcessSellCarModelCommand command);
    }
}

using Majestic.WarehouseService.Services.Services.Cars.UpdateCarCommand.CommandModels;
using Majestic.WarehouseService.Services.Services.Cars.UpdateCarCommand.Result;

namespace Majestic.WarehouseService.Services.Services.Cars.UpdateCarCommand
{
    public interface IUpdateCarCommandService
    {
        Task<UpdateCarFlowResult> HandleAsync(UpdateCarModelCommand command);
    }
}

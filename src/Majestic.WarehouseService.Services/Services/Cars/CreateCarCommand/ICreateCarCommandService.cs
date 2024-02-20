using Majestic.WarehouseService.Services.Services.Cars.CreateCarCommand.CommandModels;
using Majestic.WarehouseService.Services.Services.Cars.CreateCarCommand.Result;

namespace Majestic.WarehouseService.Services.Services.Cars.CreateCarCommand
{
    public interface ICreateCarCommandService
    {
        Task<CreateCarFlowResult> HandleAsync(CreateCarModelCommand command);
    }
}

using Majestic.WarehouseService.Services.Services.Cars.DeleteCarCommand.CommandModels;
using Majestic.WarehouseService.Services.Services.Cars.DeleteCarCommand.Result;

namespace Majestic.WarehouseService.Services.Services.Cars.DeleteCarCommand
{
    public interface IDeleteCarCommandService
    {
        Task<DeleteCarFlowResult> HandleAsync(DeleteCarModelCommand command);
    }
}

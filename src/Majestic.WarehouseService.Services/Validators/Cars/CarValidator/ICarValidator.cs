using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.CreateCars.Request;
using Majestic.WarehouseService.Models.v1.ProcessCarSell.Event;
using Majestic.WarehouseService.Models.v1.UpdateCars.Request;

namespace Majestic.WarehouseService.Services.Validators.Cars.CreateCarValidator
{
    public interface ICarValidator
    {
        ServiceResult Validate(IEnumerable<CreateCarRequest> requests);
        ServiceResult Validate(CreateCarRequest request);
        ServiceResult Validate(UpdateCarRequest request);
        ServiceResult Validate(ProcessSellCarEvent request);
    }
}

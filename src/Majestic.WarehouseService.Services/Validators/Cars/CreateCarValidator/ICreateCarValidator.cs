using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.CreateCars.Request;

namespace Majestic.WarehouseService.Services.Validators.Cars.CreateCarValidator
{
    public interface ICreateCarValidator
    {
        ServiceResult Validate(IEnumerable<CreateCarRequest> requests);
        ServiceResult Validate(CreateCarRequest request);
    }
}

using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.CreateCars.Request;

namespace Majestic.WarehouseService.Services.Validators.Cars.CreateCarValidator
{
    public class CreateCarValidator : ICreateCarValidator
    {
        public ServiceResult Validate(CreateCarRequest request)
        {
            return new ServiceResult(false);
        }
    }
}

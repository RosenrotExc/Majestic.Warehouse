using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.GetCars.Request;
using Majestic.WarehouseService.Models.v1.GetCars.Response;
using Majestic.WarehouseService.Repository.Models.Cars;
using InitiatorServiceModel = Majestic.WarehouseService.Models.Misc.Initiator;

namespace Majestic.WarehouseService.Repository.Repository.Cars
{
    public interface ICarsRepository
    {
        Task<ServiceResult> CreateCarAsync(CarEntity entity, InitiatorServiceModel initiator);
        Task<PaginatedServiceResultWrapper<IEnumerable<CarEntity>, GetCarFilter>> QueryCarsAsync(GetCarFilter filter);
    }
}

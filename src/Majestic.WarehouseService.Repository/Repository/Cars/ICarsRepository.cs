using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.GetCars.Request;
using Majestic.WarehouseService.Models.v1.GetCars.Response;
using Majestic.WarehouseService.Repository.Models.Cars;

namespace Majestic.WarehouseService.Repository.Repository.Cars
{
    public interface ICarsRepository
    {
        Task<ServiceResult> CreateCarAsync(CarEntity entity);
        Task<PaginatedServiceResultWrapper<GetCarResponse, GetCarFilter>> QueryCarsAsync(GetCarFilter filter);
    }
}

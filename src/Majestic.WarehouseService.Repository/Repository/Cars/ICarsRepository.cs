using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.GetCars.Request;
using Majestic.WarehouseService.Models.v1.GetCarsMetrics.Response;
using Majestic.WarehouseService.Repository.Models.Cars;
using InitiatorServiceModel = Majestic.WarehouseService.Models.Misc.Initiator;

namespace Majestic.WarehouseService.Repository.Repository.Cars
{
    public interface ICarsRepository
    {
        Task<ServiceResult> CreateCarAsync(List<CarEntity> entity, InitiatorServiceModel initiator);
        Task<ServiceResult> UpdateCarAsync(string code, CarEntity entity, InitiatorServiceModel initiator);
        Task<ServiceResult> DeleteCarAsync(string code, InitiatorServiceModel initiator);
        Task<PaginatedServiceResultWrapper<IEnumerable<CarEntity>, GetCarFilter>> QueryCarsAsync(GetCarFilter filter);
        Task<ServiceResultWrapper<GetCarsMetricsResponse>> QueryMetricsAsync();
    }
}

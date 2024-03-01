using Majestic.WarehouseService.Models.v1.CreateCars.Request;
using Majestic.WarehouseService.Models.v1.GetCars.Response;
using Majestic.WarehouseService.Models.v1.ProcessCarSell.Event;
using Majestic.WarehouseService.Models.v1.ProcessSellCar.Request;
using Majestic.WarehouseService.Models.v1.UpdateCars.Request;
using Majestic.WarehouseService.Repository.Models.Cars;

namespace Majestic.WarehouseService.Services.Mappers.Cars
{
    public interface ICarMapper
    {
        CarEntity MapCreateCarRequestToCarEntity(CreateCarRequest request);
        CarEntity MapUpdateCarRequestToCarEntity(UpdateCarRequest request);
        GetCarResponse MapCarEntityToCarResponse(CarEntity entity);
        ProcessSellCarEvent MapProcessSellCarRequestToEvent(ProcessSellCarRequest request, string requestId);
    }
}

using Majestic.WarehouseService.Models.v1.CreateCars.Request;
using Majestic.WarehouseService.Models.v1.GetCars.Response;
using Majestic.WarehouseService.Repository.Models.Cars;

namespace Majestic.WarehouseService.Services.Mappers.Cars
{
    public interface ICreateCarMapper
    {
        CarEntity MapCarRequestToCarEntity(CreateCarRequest request);
        GetCarResponse MapCarEntityToCarResponse(CarEntity entity);
    }
}

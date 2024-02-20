using Majestic.WarehouseService.Models.v1.CreateCars.Request;
using Majestic.WarehouseService.Models.v1.CreateCars.Response;
using Majestic.WarehouseService.Repository.Models.Cars;

namespace Majestic.WarehouseService.Services.Mappers.Cars
{
    public interface ICreateCarMapper
    {
        CarEntity MapCarRequestToCarEntity(CreateCarRequest request);
        CreateCarResponse MapCarEntityToCarResponse(CarEntity entity);
    }
}

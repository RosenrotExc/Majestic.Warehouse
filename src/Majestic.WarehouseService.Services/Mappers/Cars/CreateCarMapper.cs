using Majestic.WarehouseService.Models.v1.CreateCars.Request;
using Majestic.WarehouseService.Models.v1.CreateCars.Response;
using Majestic.WarehouseService.Repository.Models.Cars;

namespace Majestic.WarehouseService.Services.Mappers.Cars
{
    public class CreateCarMapper : ICreateCarMapper
    {
        public CarEntity MapCarRequestToCarEntity(CreateCarRequest request)
        {
            return new CarEntity();
        }

        public CreateCarResponse MapCarEntityToCarResponse(CarEntity entity)
        {
            return new CreateCarResponse();
        }
    }
}

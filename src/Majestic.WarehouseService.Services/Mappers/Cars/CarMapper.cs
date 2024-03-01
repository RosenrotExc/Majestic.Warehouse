using Majestic.WarehouseService.Models.v1.CreateCars.Request;
using Majestic.WarehouseService.Models.v1.GetCars.Response;
using Majestic.WarehouseService.Models.v1.ProcessCarSell.Event;
using Majestic.WarehouseService.Models.v1.ProcessSellCar.Request;
using Majestic.WarehouseService.Models.v1.UpdateCars.Request;
using Majestic.WarehouseService.Repository.Models.Cars;

namespace Majestic.WarehouseService.Services.Mappers.Cars
{
    public class CarMapper : ICarMapper
    {
        public CarEntity MapCreateCarRequestToCarEntity(CreateCarRequest request)
        {
            return new CarEntity
            {
                CarName = request.CarName,
                ModelName = request.ModelName,
                OwnerName = request.OwnerName,
                OwnersPrice = request.OwnersPrice,
                DealersPrice = request.DealersPrice,
                DealerNotes = request.DealerNotes
            };
        }

        public CarEntity MapUpdateCarRequestToCarEntity(UpdateCarRequest request)
        {
            return new CarEntity
            {
                CarName = request.CarName,
                ModelName = request.ModelName,
                OwnerName = request.OwnerName,
                OwnersPrice = request.OwnersPrice,
                DealersPrice = request.DealersPrice,
                SellFinalPrice = request.SellFinalPrice,
                DealerNotes = request.DealerNotes
            };
        }

        public GetCarResponse MapCarEntityToCarResponse(CarEntity entity)
        {
            return new GetCarResponse
            {
                Code = entity.Code.Value,
                CarName = entity.CarName,
                ModelName = entity.ModelName,
                OwnerName = entity.OwnerName,
                DealersPrice = entity.DealersPrice,
                OwnersPrice = entity.OwnersPrice,
                SellFinalPrice = entity.SellFinalPrice,
                DealerNotes = entity.DealerNotes
            };
        }

        public ProcessSellCarEvent MapProcessSellCarRequestToEvent(ProcessSellCarRequest request, string requestId)
        {
            return new ProcessSellCarEvent
            {
                RequestId = requestId,
                Code = request.Code,
                Amount = request.Amount,
                NewOwnerName = request.NewOwnerName
            };
        }
    }
}

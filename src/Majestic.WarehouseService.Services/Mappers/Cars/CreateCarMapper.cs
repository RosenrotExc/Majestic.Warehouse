﻿using Majestic.WarehouseService.Models.v1.CreateCars.Request;
using Majestic.WarehouseService.Models.v1.GetCars.Response;
using Majestic.WarehouseService.Repository.Models.Cars;

namespace Majestic.WarehouseService.Services.Mappers.Cars
{
    public class CreateCarMapper : ICreateCarMapper
    {
        public CarEntity MapCarRequestToCarEntity(CreateCarRequest request)
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
                DealerNotes = entity.DealerNotes
            };
        }
    }
}

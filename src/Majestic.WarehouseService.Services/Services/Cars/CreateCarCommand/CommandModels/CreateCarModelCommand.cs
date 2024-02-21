using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.CreateCars.Request;

namespace Majestic.WarehouseService.Services.Services.Cars.CreateCarCommand.CommandModels
{
    public class CreateCarModelCommand
    {
        public CreateCarsRequest Request { get; }
        public Initiator Initiator { get; }

        public CreateCarModelCommand(CreateCarsRequest request, Initiator initiator)
        {
            Request = request;
            Initiator = initiator;
        }
    }
}

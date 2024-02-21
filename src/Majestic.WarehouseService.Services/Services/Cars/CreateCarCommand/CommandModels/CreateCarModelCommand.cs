using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.CreateCars.Request;

namespace Majestic.WarehouseService.Services.Services.Cars.CreateCarCommand.CommandModels
{
    public class CreateCarModelCommand
    {
        public CreateCarRequest Request { get; }
        public Initiator Initiator { get; }

        public CreateCarModelCommand(CreateCarRequest request, Initiator initiator)
        {
            Request = request;
            Initiator = initiator;
        }
    }
}

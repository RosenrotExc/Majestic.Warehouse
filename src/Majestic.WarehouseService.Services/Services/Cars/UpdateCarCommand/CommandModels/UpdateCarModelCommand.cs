using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.CreateCars.Request;
using Majestic.WarehouseService.Models.v1.UpdateCars.Request;

namespace Majestic.WarehouseService.Services.Services.Cars.UpdateCarCommand.CommandModels
{
    public class UpdateCarModelCommand
    {
        public string Code { get; set; }
        public UpdateCarRequest Request { get; }
        public Initiator Initiator { get; }

        public UpdateCarModelCommand(string code, UpdateCarRequest request, Initiator initiator)
        {
            Code = code;
            Request = request;
            Initiator = initiator;
        }
    }
}

using System.Runtime.CompilerServices;
using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.ProcessSellCar.Request;

namespace Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarCommand.CommandModels
{
    public class ProcessSellCarModelCommand
    {
        public ProcessSellCarRequest Request { get; set; }
        public Initiator Initiator { get; set; }
        public string RequestId { get; set; }

        public ProcessSellCarModelCommand(ProcessSellCarRequest request, Initiator initiator, string requestId)
        {
            Request = request;
            Initiator = initiator;
            RequestId = requestId;
        }
    }
}

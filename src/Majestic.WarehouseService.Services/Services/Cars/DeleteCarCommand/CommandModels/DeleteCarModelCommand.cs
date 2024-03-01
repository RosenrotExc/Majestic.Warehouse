using Majestic.WarehouseService.Models.Misc;

namespace Majestic.WarehouseService.Services.Services.Cars.DeleteCarCommand.CommandModels
{
    public class DeleteCarModelCommand
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public Initiator Initiator { get; }

        public DeleteCarModelCommand(string code, string message, Initiator initiator)
        {
            Code = code;
            Message = message;
            Initiator = initiator;
        }
    }
}

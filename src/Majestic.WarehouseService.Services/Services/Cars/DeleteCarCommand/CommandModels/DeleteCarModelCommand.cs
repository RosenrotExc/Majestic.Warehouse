using Majestic.WarehouseService.Models.Misc;

namespace Majestic.WarehouseService.Services.Services.Cars.DeleteCarCommand.CommandModels
{
    public class DeleteCarModelCommand
    {
        public string Code { get; set; }
        public Initiator Initiator { get; }

        public DeleteCarModelCommand(string code, Initiator initiator)
        {
            Code = code;
            Initiator = initiator;
        }
    }
}

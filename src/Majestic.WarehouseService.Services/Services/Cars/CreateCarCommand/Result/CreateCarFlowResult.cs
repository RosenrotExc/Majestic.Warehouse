using Majestic.WarehouseService.Models.Misc;

namespace Majestic.WarehouseService.Services.Services.Cars.CreateCarCommand.Result
{
    public class CreateCarFlowResult
    {
        public enum Reasons
        {
            UnexpectedError
        }

        public bool Successful { get; }

        public Reasons? Reason { get; }

        public ServiceResult Result { get; set; }

        public CreateCarFlowResult(bool successful, ServiceResult result, Reasons? reason)
        {
            Successful = successful;
            Result = result;
            Reason = reason;
        }

        public static CreateCarFlowResult UnexpectedError()
        {
            return new CreateCarFlowResult(false, null, Reasons.UnexpectedError);
        }

        public static CreateCarFlowResult Success(ServiceResult result)
        {
            return new CreateCarFlowResult(true, result, null);
        }
    }
}

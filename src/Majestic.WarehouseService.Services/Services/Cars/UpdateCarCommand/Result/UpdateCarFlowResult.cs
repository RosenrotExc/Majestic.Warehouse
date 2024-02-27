using Majestic.WarehouseService.Models.Misc;

namespace Majestic.WarehouseService.Services.Services.Cars.UpdateCarCommand.Result
{
    public class UpdateCarFlowResult
    {
        public enum Reasons
        {
            UnexpectedError,
            ValidationError
        }

        public bool Successful { get; }

        public Reasons? Reason { get; }

        public ServiceResult Result { get; set; }

        public UpdateCarFlowResult(bool successful, ServiceResult result, Reasons? reason)
        {
            Successful = successful;
            Result = result;
            Reason = reason;
        }

        public static UpdateCarFlowResult UnexpectedError()
        {
            return new UpdateCarFlowResult(false, null, Reasons.UnexpectedError);
        }

        public static UpdateCarFlowResult ValidationError(ServiceResult result)
        {
            return new UpdateCarFlowResult(false, result, Reasons.ValidationError);
        }

        public static UpdateCarFlowResult Success(ServiceResult result)
        {
            return new UpdateCarFlowResult(true, result, null);
        }
    }
}

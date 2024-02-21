using Majestic.WarehouseService.Models.Misc;

namespace Majestic.WarehouseService.Services.Services.Cars.DeleteCarCommand.Result
{
    public class DeleteCarFlowResult
    {
        public enum Reasons
        {
            UnexpectedError
        }

        public bool Successful { get; }

        public Reasons? Reason { get; }

        public ServiceResult Result { get; set; }

        public DeleteCarFlowResult(bool successful, ServiceResult result, Reasons? reason)
        {
            Successful = successful;
            Result = result;
            Reason = reason;
        }

        public static DeleteCarFlowResult UnexpectedError()
        {
            return new DeleteCarFlowResult(false, null, Reasons.UnexpectedError);
        }

        public static DeleteCarFlowResult Success(ServiceResult result)
        {
            return new DeleteCarFlowResult(true, result, null);
        }
    }
}

using Majestic.WarehouseService.Models.Misc;

namespace Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarCommand.Result
{
    public class ProcessCarFlowResult
    {
        public enum Reasons
        {
            UnexpectedError
        }

        public bool Successful { get; }

        public Reasons? Reason { get; }

        public ServiceResult Result { get; set; }

        public ProcessCarFlowResult(bool successful, ServiceResult result, Reasons? reason)
        {
            Successful = successful;
            Result = result;
            Reason = reason;
        }

        public static ProcessCarFlowResult UnexpectedError()
        {
            return new ProcessCarFlowResult(false, null, Reasons.UnexpectedError);
        }

        public static ProcessCarFlowResult Success(ServiceResult result)
        {
            return new ProcessCarFlowResult(true, result, null);
        }
    }
}

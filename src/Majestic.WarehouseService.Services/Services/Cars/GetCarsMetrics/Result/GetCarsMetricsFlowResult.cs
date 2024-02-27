using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.GetCarsMetrics.Response;

namespace Majestic.WarehouseService.Services.Services.Cars.GetCarsMetrics.Result
{
    public class GetCarsMetricsFlowResult
    {
        public enum Reasons
        {
            UnexpectedError,
            FailedToGetCarsMetrics,
            NotFound
        }

        public bool Successful { get; }

        public Reasons? Reason { get; }

        public ServiceResultWrapper<GetCarsMetricsResponse> Result { get; set; }

        public GetCarsMetricsFlowResult(bool successful, ServiceResultWrapper<GetCarsMetricsResponse> result, Reasons? reason)
        {
            Successful = successful;
            Result = result;
            Reason = reason;
        }

        public static GetCarsMetricsFlowResult UnexpectedError()
        {
            return new GetCarsMetricsFlowResult(false, null, Reasons.UnexpectedError);
        }

        public static GetCarsMetricsFlowResult FailedToGetCarsMetrics()
        {
            return new GetCarsMetricsFlowResult(false, null, Reasons.FailedToGetCarsMetrics);
        }

        public static GetCarsMetricsFlowResult NotFound()
        {
            return new GetCarsMetricsFlowResult(false, null, Reasons.NotFound);
        }

        public static GetCarsMetricsFlowResult Success(ServiceResultWrapper<GetCarsMetricsResponse> result)
        {
            return new GetCarsMetricsFlowResult(true, result, null);
        }
    }
}

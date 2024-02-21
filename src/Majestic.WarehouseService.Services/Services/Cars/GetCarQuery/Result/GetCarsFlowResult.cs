using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.GetCars.Request;
using Majestic.WarehouseService.Models.v1.GetCars.Response;

namespace Majestic.WarehouseService.Services.Services.Cars.GetCarQuery.Result
{
    public class GetCarsFlowResult
    {
        public enum Reasons
        {
            UnexpectedError,
            FailedToGetCars,
            NotFound
        }

        public bool Successful { get; }

        public Reasons? Reason { get; }

        public PaginatedServiceResultWrapper<IEnumerable<GetCarResponse>, GetCarFilter> Result { get; set; }

        public GetCarsFlowResult(bool successful, PaginatedServiceResultWrapper<IEnumerable<GetCarResponse>, GetCarFilter> result, Reasons? reason)
        {
            Successful = successful;
            Result = result;
            Reason = reason;
        }

        public static GetCarsFlowResult UnexpectedError()
        {
            return new GetCarsFlowResult(false, null, Reasons.UnexpectedError);
        }

        public static GetCarsFlowResult FailedToGetCars()
        {
            return new GetCarsFlowResult(false, null, Reasons.FailedToGetCars);
        }

        public static GetCarsFlowResult NotFound()
        {
            return new GetCarsFlowResult(false, null, Reasons.NotFound);
        }

        public static GetCarsFlowResult Success(PaginatedServiceResultWrapper<IEnumerable<GetCarResponse>, GetCarFilter> result)
        {
            return new GetCarsFlowResult(true, result, null);
        }
    }
}

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
            NotFound
        }

        public bool Successful { get; }

        public Reasons? Reason { get; }

        public PaginatedServiceResultWrapper<GetCarResponse, GetCarFilter> Result { get; set; }

        public GetCarsFlowResult(bool successful, PaginatedServiceResultWrapper<GetCarResponse, GetCarFilter> result, Reasons? reason)
        {
            Successful = successful;
            Result = result;
            Reason = reason;
        }

        public static GetCarsFlowResult UnexpectedError()
        {
            return new GetCarsFlowResult(false, null, Reasons.UnexpectedError);
        }

        public static GetCarsFlowResult NotFound()
        {
            return new GetCarsFlowResult(false, null, Reasons.NotFound);
        }

        public static GetCarsFlowResult Success(PaginatedServiceResultWrapper<GetCarResponse, GetCarFilter> result)
        {
            return new GetCarsFlowResult(true, result, null);
        }
    }
}

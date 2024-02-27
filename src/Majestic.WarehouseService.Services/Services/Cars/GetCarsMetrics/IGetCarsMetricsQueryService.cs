using Majestic.WarehouseService.Services.Services.Cars.GetCarsMetrics.QueryModels;
using Majestic.WarehouseService.Services.Services.Cars.GetCarsMetrics.Result;

namespace Majestic.WarehouseService.Services.Services.Cars.GetCarsMetrics
{
    public interface IGetCarsMetricsQueryService
    {
        Task<GetCarsMetricsFlowResult> HandleAsync(GetCarsMetricsModelQuery query);
    }
}

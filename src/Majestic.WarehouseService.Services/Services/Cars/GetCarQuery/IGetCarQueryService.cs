using Majestic.WarehouseService.Services.Services.Cars.GetCarQuery.QueryModels;
using Majestic.WarehouseService.Services.Services.Cars.GetCarQuery.Result;

namespace Majestic.WarehouseService.Services.Services.Cars.GetCarQuery
{
    public interface IGetCarQueryService
    {
        Task<GetCarsFlowResult> HandleAsync(GetCarsModelQuery query);
    }
}

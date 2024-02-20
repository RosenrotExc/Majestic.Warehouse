using Majestic.WarehouseService.Repository.Repository.Cars;
using Majestic.WarehouseService.Services.Services.Cars.GetCarQuery.QueryModels;
using Majestic.WarehouseService.Services.Services.Cars.GetCarQuery.Result;
using Microsoft.Extensions.Logging;

namespace Majestic.WarehouseService.Services.Services.Cars.GetCarQuery
{
    public class GetCarQueryService : IGetCarQueryService
    {
        private readonly ILogger<GetCarQueryService> _logger;
        private readonly ICarsRepository _carsRepository;

        public GetCarQueryService(
            ILogger<GetCarQueryService> logger, 
            ICarsRepository carsRepository)
        {
            _logger = logger;
            _carsRepository = carsRepository;
        }

        public async Task<GetCarsFlowResult> HandleAsync(GetCarsModelQuery query)
        {
            var result = await _carsRepository.QueryCarsAsync(query.Filter);

            return default;
        }
    }
}

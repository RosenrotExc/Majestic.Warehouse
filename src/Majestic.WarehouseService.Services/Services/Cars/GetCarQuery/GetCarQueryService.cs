using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.GetCars.Request;
using Majestic.WarehouseService.Models.v1.GetCars.Response;
using Majestic.WarehouseService.Repository.Repository.Cars;
using Majestic.WarehouseService.Services.Mappers.Cars;
using Majestic.WarehouseService.Services.Services.Cars.GetCarQuery.QueryModels;
using Majestic.WarehouseService.Services.Services.Cars.GetCarQuery.Result;
using Microsoft.Extensions.Logging;

namespace Majestic.WarehouseService.Services.Services.Cars.GetCarQuery
{
    public class GetCarQueryService : IGetCarQueryService
    {
        private readonly ILogger<GetCarQueryService> _logger;
        private readonly ICarsRepository _carsRepository;
        private readonly ICarMapper _createCarMapper;

        public GetCarQueryService(
            ILogger<GetCarQueryService> logger, 
            ICarsRepository carsRepository,
            ICarMapper createCarMapper)
        {
            _logger = logger;
            _carsRepository = carsRepository;
            _createCarMapper = createCarMapper;
        }

        public async Task<GetCarsFlowResult> HandleAsync(GetCarsModelQuery query)
        {
            _logger.LogInformation("{name} {@query}", nameof(GetCarQueryService), query);

            var result = await _carsRepository.QueryCarsAsync(query.Filter);
            if (!result.IsSuccess)
            {
                _logger.LogError("{name} Failed to query cars {@query}", nameof(GetCarQueryService), query);
                return GetCarsFlowResult.FailedToGetCars();
            }

            var mapperResult = result.Value.Select(x => _createCarMapper.MapCarEntityToCarResponse(x));
            
            var response = new PaginatedServiceResultWrapper<IEnumerable<GetCarResponse>, GetCarFilter>
            {
                IsSuccess = true,
                Next = result.Next,
                Prev = result.Prev,
                TotalCount = result.TotalCount,
                Value = mapperResult
            };

            return GetCarsFlowResult.Success(response);
        }
    }
}

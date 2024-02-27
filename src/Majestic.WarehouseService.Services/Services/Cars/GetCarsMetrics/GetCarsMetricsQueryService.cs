using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.GetCarsMetrics.Response;
using Majestic.WarehouseService.Repository.Repository.Cars;
using Majestic.WarehouseService.Services.Mappers.Cars;
using Majestic.WarehouseService.Services.Services.Cars.GetCarsMetrics.QueryModels;
using Majestic.WarehouseService.Services.Services.Cars.GetCarsMetrics.Result;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Majestic.WarehouseService.Services.Services.Cars.GetCarsMetrics
{
    public class GetCarsMetricsQueryService : IGetCarsMetricsQueryService
    {
        private readonly ILogger<GetCarsMetricsQueryService> _logger;
        private readonly ICarsRepository _carsRepository;
        private readonly IDistributedCache _cache;

        public GetCarsMetricsQueryService(
            ILogger<GetCarsMetricsQueryService> logger,
            ICarsRepository carsRepository,
            ICarMapper createCarMapper,
            IDistributedCache cache)
        {
            _logger = logger;
            _carsRepository = carsRepository;
            _cache = cache;
        }

        public async Task<GetCarsMetricsFlowResult> HandleAsync(GetCarsMetricsModelQuery query)
        {
            _logger.LogInformation("{name} {@query}", nameof(GetCarsMetricsQueryService), query);

            var carsMetrics = await _cache.GetStringAsync(Constants.RedisContants.CarsMetricsKey);
            if (!string.IsNullOrWhiteSpace(carsMetrics) && TryParseMetrics(carsMetrics, out var parsedMetrics))
            {
                return GetCarsMetricsFlowResult.Success(new ServiceResultWrapper<GetCarsMetricsResponse>(parsedMetrics));
            }

            #region Some long running job
            await Task.Delay(1000);
            #endregion

            var result = await _carsRepository.QueryMetricsAsync();
            if (!result.IsSuccess)
            {
                _logger.LogError("{name} Failed to query cars metrics {@result}", nameof(GetCarsMetricsQueryService), result);
                return GetCarsMetricsFlowResult.FailedToGetCarsMetrics();
            }

            var serializedMetrics = JsonConvert.SerializeObject(result.Value);
            await _cache.SetStringAsync(Constants.RedisContants.CarsMetricsKey, serializedMetrics,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });

            return GetCarsMetricsFlowResult.Success(result);
        }

        private static bool TryParseMetrics(string value, out GetCarsMetricsResponse response)
        {
            try
            {
                response = JsonConvert.DeserializeObject<GetCarsMetricsResponse>(value);
                return true;
            }
            catch
            {
                response = null;
                return false;
            }
        }
    }
}

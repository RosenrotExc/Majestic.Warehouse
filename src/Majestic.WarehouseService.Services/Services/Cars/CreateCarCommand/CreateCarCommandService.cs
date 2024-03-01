using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Repository.Repository.Cars;
using Majestic.WarehouseService.Services.Mappers.Cars;
using Majestic.WarehouseService.Services.Services.Cars.CreateCarCommand.CommandModels;
using Majestic.WarehouseService.Services.Services.Cars.CreateCarCommand.Result;
using Majestic.WarehouseService.Services.Validators.Cars.CreateCarValidator;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Majestic.WarehouseService.Services.Services.Cars.CreateCarCommand
{
    public class CreateCarCommandService : ICreateCarCommandService
    {
        private readonly ILogger<CreateCarCommandService> _logger;
        private readonly ICarsRepository _carsRepository;
        private readonly ICarValidator _carValidator;
        private readonly ICarMapper _carMapper;
        private readonly IDistributedCache _cache;

        public CreateCarCommandService(
            ILogger<CreateCarCommandService> logger,
            ICarsRepository carsRepository,
            ICarValidator carValidator,
            ICarMapper carMapper,
            IDistributedCache cache)

        {
            _logger = logger;
            _carsRepository = carsRepository;
            _carValidator = carValidator;
            _carMapper = carMapper;
            _cache = cache;
        }

        public async Task<CreateCarFlowResult> HandleAsync(CreateCarModelCommand command)
        {
            _logger.LogInformation("{name} {@command}", nameof(CreateCarCommandService), command);

            var validationResult = _carValidator.Validate(command.Request.Requests);
            if (!validationResult.IsSuccess)
            {
                _logger.LogError("{name} Validation failed {@validationResult} {@command}", nameof(CreateCarCommandService),
                    validationResult, command);
                return CreateCarFlowResult.ValidationError(validationResult);
            }

            var mappedModels = command.Request.Requests.Select(x => _carMapper.MapCarRequestToCarEntity(x)).ToList();

            var result = await _carsRepository.CreateCarAsync(mappedModels, command.Initiator);
            if (!result.IsSuccess)
            {
                _logger.LogError("{name} Failed to create car {@command}", nameof(CreateCarCommandService),
                    command);
                return CreateCarFlowResult.UnexpectedError();
            }

            await _cache.RemoveAsync(Constants.RedisContants.CarsMetricsKey);

            return CreateCarFlowResult.Success(result);
        }
    }
}

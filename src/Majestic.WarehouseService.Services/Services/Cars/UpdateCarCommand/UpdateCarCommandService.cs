﻿using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Repository.Repository.Cars;
using Majestic.WarehouseService.Services.Mappers.Cars;
using Majestic.WarehouseService.Services.Services.Cars.UpdateCarCommand.CommandModels;
using Majestic.WarehouseService.Services.Services.Cars.UpdateCarCommand.Result;
using Majestic.WarehouseService.Services.Validators.Cars.CreateCarValidator;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Majestic.WarehouseService.Services.Services.Cars.UpdateCarCommand
{
    public class UpdateCarCommandService : IUpdateCarCommandService
    {
        private readonly ILogger<UpdateCarCommandService> _logger;
        private readonly ICarsRepository _carsRepository;
        private readonly ICarValidator _carValidator;
        private readonly ICarMapper _carMapper;
        private readonly IDistributedCache _cache;

        public UpdateCarCommandService(
            ILogger<UpdateCarCommandService> logger,
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

        public async Task<UpdateCarFlowResult> HandleAsync(UpdateCarModelCommand command)
        {
            _logger.LogInformation("{name} {@command}", nameof(UpdateCarCommandService), command);

            var validationResult = _carValidator.Validate(command.Request);
            if (!validationResult.IsSuccess)
            {
                _logger.LogError("{name} Validation failed {@validationResult} {@command}", nameof(UpdateCarCommandService),
                    validationResult, command);
                return UpdateCarFlowResult.ValidationError(validationResult);
            }

            var mappedModel = _carMapper.MapUpdateCarRequestToCarEntity(command.Request);

            var result = await _carsRepository.UpdateCarAsync(command.Code, mappedModel, command.Initiator);
            if (!result.IsSuccess)
            {
                _logger.LogError("{name} Failed to update car {@command}", nameof(UpdateCarCommandService),
                    command);
                return UpdateCarFlowResult.UnexpectedError();
            }

            await _cache.RemoveAsync(Constants.RedisContants.CarsMetricsKey);

            return UpdateCarFlowResult.Success(result);
        }
    }
}

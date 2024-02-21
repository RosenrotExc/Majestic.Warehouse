using Majestic.WarehouseService.Repository.Repository.Cars;
using Majestic.WarehouseService.Services.Mappers.Cars;
using Majestic.WarehouseService.Services.Services.Cars.CreateCarCommand.CommandModels;
using Majestic.WarehouseService.Services.Services.Cars.CreateCarCommand.Result;
using Majestic.WarehouseService.Services.Validators.Cars.CreateCarValidator;
using Microsoft.Extensions.Logging;

namespace Majestic.WarehouseService.Services.Services.Cars.CreateCarCommand
{
    public class CreateCarCommandService : ICreateCarCommandService
    {
        private readonly ILogger<CreateCarCommandService> _logger;
        private readonly ICarsRepository _carsRepository;
        private readonly ICreateCarValidator _createCarValidator;
        private readonly ICreateCarMapper _createCarMapper;

        public CreateCarCommandService(
            ILogger<CreateCarCommandService> logger,
            ICarsRepository carsRepository,
            ICreateCarValidator createCarValidator,
            ICreateCarMapper createCarMapper)
        {
            _logger = logger;
            _carsRepository = carsRepository;
            _createCarValidator = createCarValidator;
            _createCarMapper = createCarMapper;
        }

        public async Task<CreateCarFlowResult> HandleAsync(CreateCarModelCommand command)
        {
            _logger.LogInformation("{name} {@command}", nameof(CreateCarCommandService), command);

            var validationResult = _createCarValidator.Validate(command.Request);
            if (!validationResult.IsSuccess)
            {
                _logger.LogError("{name} Validation failed {@validationResult} {@command}", nameof(CreateCarCommandService),
                    validationResult, command);
                return CreateCarFlowResult.ValidationError();
            }

            var mappedModel = _createCarMapper.MapCarRequestToCarEntity(command.Request);

            var result = await _carsRepository.CreateCarAsync(mappedModel, command.Initiator);
            if (!result.IsSuccess)
            {
                _logger.LogError("{name} Failed to create car {@command}", nameof(CreateCarCommandService),
                    command);
                return CreateCarFlowResult.UnexpectedError();
            }

            return CreateCarFlowResult.Success(result);
        }
    }
}

using Majestic.WarehouseService.Repository.Repository.Cars;
using Majestic.WarehouseService.Services.Mappers.Cars;
using Majestic.WarehouseService.Services.Services.Cars.DeleteCarCommand.CommandModels;
using Majestic.WarehouseService.Services.Services.Cars.DeleteCarCommand.Result;
using Majestic.WarehouseService.Services.Validators.Cars.CreateCarValidator;
using Microsoft.Extensions.Logging;

namespace Majestic.WarehouseService.Services.Services.Cars.DeleteCarCommand
{
    public class DeleteCarCommandService : IDeleteCarCommandService
    {
        private readonly ILogger<DeleteCarCommandService> _logger;
        private readonly ICarsRepository _carsRepository;
        private readonly ICreateCarValidator _createCarValidator;
        private readonly ICarMapper _createCarMapper;

        public DeleteCarCommandService(
            ILogger<DeleteCarCommandService> logger,
            ICarsRepository carsRepository,
            ICreateCarValidator createCarValidator,
            ICarMapper createCarMapper)
        {
            _logger = logger;
            _carsRepository = carsRepository;
            _createCarValidator = createCarValidator;
            _createCarMapper = createCarMapper;
        }

        public async Task<DeleteCarFlowResult> HandleAsync(DeleteCarModelCommand command)
        {
            _logger.LogInformation("{name} {@command}", nameof(DeleteCarCommandService), command);

            var result = await _carsRepository.DeleteCarAsync(command.Code, command.Initiator);
            if (!result.IsSuccess)
            {
                _logger.LogError("{name} Failed to delete car {@command}", nameof(DeleteCarCommandService),
                    command);
                return DeleteCarFlowResult.UnexpectedError();
            }

            return DeleteCarFlowResult.Success(result);
        }
    }
}

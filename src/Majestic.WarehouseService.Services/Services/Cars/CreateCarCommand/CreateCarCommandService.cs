using Majestic.WarehouseService.Repository.Models.Cars;
using Majestic.WarehouseService.Repository.Repository.Cars;
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

        public CreateCarCommandService(
            ILogger<CreateCarCommandService> logger,
            ICarsRepository carsRepository,
            ICreateCarValidator createCarValidator)
        {
            _logger = logger;
            _carsRepository = carsRepository;
            _createCarValidator = createCarValidator;
        }

        public async Task<CreateCarFlowResult> HandleAsync(CreateCarModelCommand command)
        {
            #region Validate
            var validationResult = _createCarValidator.Validate(command.Request);
            #endregion

            // map command to entity
            var entity = new CarEntity();

            var result = await _carsRepository.CreateCarAsync(entity);

            return default;
        }
    }
}

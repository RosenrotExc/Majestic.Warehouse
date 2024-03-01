using System.Reflection.Metadata;
using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Repository.Repository.Cars;
using Majestic.WarehouseService.Services.Mappers.Cars;
using Majestic.WarehouseService.Services.Services.Cars.DeleteCarCommand.CommandModels;
using Majestic.WarehouseService.Services.Services.Cars.DeleteCarCommand.Result;
using Majestic.WarehouseService.Services.Validators.Cars.CreateCarValidator;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Majestic.WarehouseService.Services.Services.Cars.DeleteCarCommand
{
    public class DeleteCarCommandService : IDeleteCarCommandService
    {
        private readonly ILogger<DeleteCarCommandService> _logger;
        private readonly ICarsRepository _carsRepository;
        private readonly ICarValidator _carValidator;
        private readonly ICarMapper _carMapper;
        private readonly IDistributedCache _cache;

        public DeleteCarCommandService(
            ILogger<DeleteCarCommandService> logger,
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

        public async Task<DeleteCarFlowResult> HandleAsync(DeleteCarModelCommand command)
        {
            _logger.LogInformation("{name} {@command}", nameof(DeleteCarCommandService), command);

            const string DeleteMessage = "Record is removed";
            command.Message ??= DeleteMessage;
            var result = await _carsRepository.DeleteCarAsync(command.Code, command.Message, command.Initiator);
            if (!result.IsSuccess)
            {
                _logger.LogError("{name} Failed to delete car {@command}", nameof(DeleteCarCommandService),
                    command);
                return DeleteCarFlowResult.UnexpectedError();
            }

            await _cache.RemoveAsync(Constants.RedisContants.CarsMetricsKey);

            return DeleteCarFlowResult.Success(result);
        }
    }
}

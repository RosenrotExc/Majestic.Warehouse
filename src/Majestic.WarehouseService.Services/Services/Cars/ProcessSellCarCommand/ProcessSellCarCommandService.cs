using Majestic.WarehouseService.Services.Mappers.Cars;
using Majestic.WarehouseService.Services.RabbitMq;
using Majestic.WarehouseService.Services.RabbitMq.Publisher;
using Majestic.WarehouseService.Services.Services.Cars.CreateCarCommand;
using Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarCommand.CommandModels;
using Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarCommand.Result;
using Majestic.WarehouseService.Services.Validators.Cars.CreateCarValidator;
using Microsoft.Extensions.Logging;

namespace Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarCommand
{
    public class ProcessSellCarCommandService : IProcessSellCarCommandService
    {
        private readonly ILogger<ProcessSellCarCommandService> _logger;
        private readonly IMessagePublisher _messagePublisher;
        private readonly ICarMapper _createCarMapper;
        private readonly ICreateCarValidator _createCarValidator;

        public ProcessSellCarCommandService(
            ILogger<ProcessSellCarCommandService> logger,
            IMessagePublisher messagePublisher,
            ICarMapper createCarMapper,
            ICreateCarValidator createCarValidator)
        {
            _logger = logger;
            _messagePublisher = messagePublisher;
            _createCarMapper = createCarMapper;
            _createCarValidator = createCarValidator;
        }

        public Task<ProcessCarFlowResult> HandleAsync(ProcessSellCarModelCommand command)
        {
            var eventModel = _createCarMapper.MapProcessSellCarRequestToEvent(command.Request, command.RequestId);

            var validateResult = _createCarValidator.Validate(eventModel);
            if (!validateResult.IsSuccess)
            {
                _logger.LogError("{name} Validation failed {@validationResult} {@command}", nameof(CreateCarCommandService),
                    validateResult, command);
                return Task.FromResult(ProcessCarFlowResult.ValidationError());
            }

            var result = _messagePublisher.PublishMessage(command.Request, Constants.ProcessSellCarExchangeName);
            if (!result.IsSuccess)
            {
                const string Message = "Failed to process sell car";
                _logger.LogError("{name} {Message}, {@command}", nameof(HandleAsync), Message, command);
                return Task.FromResult(ProcessCarFlowResult.UnexpectedError());
            }

            return Task.FromResult(ProcessCarFlowResult.Success(result));
        }
    }
}

using Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarCommand.CommandModels;
using Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarCommand.Result;
using Microsoft.Extensions.Logging;

namespace Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarCommand
{
    public class ProcessSellCarCommandService : IProcessSellCarCommandService
    {
        private readonly ILogger<ProcessSellCarCommandService> _logger;

        public ProcessSellCarCommandService(
            ILogger<ProcessSellCarCommandService> logger)
        {
            _logger = logger;
        }

        public async Task<ProcessCarFlowResult> HandleAsync(ProcessSellCarModelCommand command)
        {
            await Task.Delay(1000);

            return default;
        }
    }
}

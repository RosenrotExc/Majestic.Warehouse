using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.GetCars.Request;
using Majestic.WarehouseService.Models.v1.ProcessCarSell.Event;
using Majestic.WarehouseService.Models.v1.UpdateCars.Request;
using Majestic.WarehouseService.Services.Services.Cars.GetCarQuery;
using Majestic.WarehouseService.Services.Services.Cars.GetCarQuery.QueryModels;
using Majestic.WarehouseService.Services.Services.Cars.UpdateCarCommand;
using Majestic.WarehouseService.Services.Services.Cars.UpdateCarCommand.CommandModels;
using Microsoft.Extensions.Logging;

namespace Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarHandler
{
    public class ProcessSellCardHandler : IProcessSellCardHandler
    {
        private readonly ILogger<ProcessSellCardHandler> _logger;
        private readonly IGetCarQueryService _getCarQueryService;
        private readonly IUpdateCarCommandService _updateCarCommandService;

        public ProcessSellCardHandler(
            ILogger<ProcessSellCardHandler> logger,
            IGetCarQueryService getCarQueryService,
            IUpdateCarCommandService updateCarCommandService)
        {
            _logger = logger;
            _getCarQueryService = getCarQueryService;
            _updateCarCommandService = updateCarCommandService;
        }

        public async Task HandleAsync(ProcessSellCarEvent request)
        {
            try
            {
                var initiator = new Initiator
                {
                    SubjectId = Guid.NewGuid().ToString(),
                    Email = $"test{Guid.NewGuid():D}@gmail.com"
                };

                var filter = new GetCarFilter
                {
                    Codes = new List<string> { request.Code }
                };

                var query = new GetCarsModelQuery(filter, initiator);
                var result = await _getCarQueryService.HandleAsync(query);
                var existedCar = result?.Result?.Value?.FirstOrDefault();
                if (existedCar == null)
                {
                    _logger.LogError("Failed to get car by id {@request}", request);
                    return;
                }

                var model = new UpdateCarRequest
                {
                    CarName = existedCar.CarName,
                    ModelName = existedCar.ModelName,
                    OwnerName = request.NewOwnerName,
                    DealerNotes = existedCar.DealerNotes,
                    DealersPrice = existedCar.DealersPrice,
                    OwnersPrice = existedCar.OwnersPrice
                };
                var command = new UpdateCarModelCommand(request.Code, model, initiator);
                var updateResult = await _updateCarCommandService.HandleAsync(command);
                if (!updateResult.Successful)
                {
                    _logger.LogError("Failed to get car by id {@request} {@updateResult}", request, updateResult);
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to handle sell car {@request}", request);
            }
        }
    }
}

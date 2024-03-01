using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.GetCars.Request;
using Majestic.WarehouseService.Models.v1.ProcessCarSell.Event;
using Majestic.WarehouseService.Models.v1.UpdateCars.Request;
using Majestic.WarehouseService.Services.Services.Cars.CreateCarCommand;
using Majestic.WarehouseService.Services.Services.Cars.DeleteCarCommand;
using Majestic.WarehouseService.Services.Services.Cars.DeleteCarCommand.CommandModels;
using Majestic.WarehouseService.Services.Services.Cars.GetCarQuery;
using Majestic.WarehouseService.Services.Services.Cars.GetCarQuery.QueryModels;
using Majestic.WarehouseService.Services.Services.Cars.UpdateCarCommand;
using Majestic.WarehouseService.Services.Services.Cars.UpdateCarCommand.CommandModels;
using Majestic.WarehouseService.Services.Validators.Cars.CreateCarValidator;
using Microsoft.Extensions.Logging;

namespace Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarHandler
{
    public class ProcessSellCardHandler : IProcessSellCardHandler
    {
        private readonly ILogger<ProcessSellCardHandler> _logger;
        private readonly IGetCarQueryService _getCarQueryService;
        private readonly IUpdateCarCommandService _updateCarCommandService;
        private readonly IDeleteCarCommandService _deleteCarCommandService;
        private readonly ICarValidator _carValidator;

        public ProcessSellCardHandler(
            ILogger<ProcessSellCardHandler> logger,
            IGetCarQueryService getCarQueryService,
            IUpdateCarCommandService updateCarCommandService,
            IDeleteCarCommandService deleteCarCommandService,
            ICarValidator carValidator)
        {
            _logger = logger;
            _getCarQueryService = getCarQueryService;
            _updateCarCommandService = updateCarCommandService;
            _deleteCarCommandService = deleteCarCommandService;
            _carValidator = carValidator;
        }

        public async Task<ServiceResult> HandleAsync(ProcessSellCarEvent request)
        {
            try
            {
                var initiator = GetStubInitiator();

                var validateResult = _carValidator.Validate(request);
                if (!validateResult.IsSuccess)
                {
                    const string Message = "Validation failed";
                    _logger.LogError("{name} {Message} {@validationResult} {@request}", Message,
                        nameof(CreateCarCommandService), validateResult, request);
                    return new ServiceResult();
                }

                #region Query car
                var filter = new GetCarFilter
                {
                    Codes = new List<string> { request.Code }
                };
                var query = new GetCarsModelQuery(filter, initiator);
                var queryResult = await _getCarQueryService.HandleAsync(query);
                var existedCar = queryResult?.Result?.Value?.FirstOrDefault();
                if (existedCar == null)
                {
                    const string Message = "Failed to get car by id";
                    _logger.LogError("{Message} {@request}", Message, request);
                    return new ServiceResult(Message);
                }
                #endregion

                #region Update car
                var updateModel = new UpdateCarRequest
                {
                    CarName = existedCar.CarName,
                    ModelName = existedCar.ModelName,
                    OwnerName = request.NewOwnerName,
                    DealerNotes = existedCar.DealerNotes,
                    DealersPrice = existedCar.DealersPrice,
                    OwnersPrice = existedCar.OwnersPrice
                };
                var updateCommand = new UpdateCarModelCommand(request.Code, updateModel, initiator);
                var updateResult = await _updateCarCommandService.HandleAsync(updateCommand);
                if (!updateResult.Successful)
                {
                    const string Message = "Failed to update car by id";
                    _logger.LogError("{Message} {@request} {@updateResult}", Message, request, updateResult);
                    return new ServiceResult(Message);
                }
                #endregion

                #region Delete car
                const string DeleteMessage = "Car was sold";
                var deleteCommand = new DeleteCarModelCommand(request.Code, DeleteMessage, initiator);
                var deleteResult = await _deleteCarCommandService.HandleAsync(deleteCommand);
                if (!deleteResult.Successful)
                {
                    const string Message = "Failed to delete car by id";
                    _logger.LogError("{Message} {@request} {@updateResult}", Message, request, updateResult);
                    return new ServiceResult(Message);
                }
                #endregion

                return new ServiceResult(true);
            }
            catch (Exception ex)
            {
                const string Message = "Failed to handle sell car";
                _logger.LogError(ex, "{Message} {@request}", Message, request);
                return new ServiceResult(Message);
            }
        }

        private Models.Misc.Initiator GetStubInitiator()
        {
            return new Models.Misc.Initiator
            {
                SubjectId = Guid.NewGuid().ToString(),
                Email = $"test{Guid.NewGuid():D}@gmail.com"
            };
        }
    }
}

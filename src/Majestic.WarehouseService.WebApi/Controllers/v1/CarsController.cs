using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.CreateCars.Request;
using Majestic.WarehouseService.Models.v1.CreateCars.Response;
using Majestic.WarehouseService.Models.v1.GetCars.Request;
using Majestic.WarehouseService.Models.v1.GetCars.Response;
using Majestic.WarehouseService.Models.v1.ProcessSellCar.Request;
using Majestic.WarehouseService.Services.Services.Cars.CreateCarCommand;
using Majestic.WarehouseService.Services.Services.Cars.CreateCarCommand.CommandModels;
using Majestic.WarehouseService.Services.Services.Cars.CreateCarCommand.Result;
using Majestic.WarehouseService.Services.Services.Cars.GetCarQuery;
using Majestic.WarehouseService.Services.Services.Cars.GetCarQuery.QueryModels;
using Majestic.WarehouseService.Services.Services.Cars.GetCarQuery.Result;
using Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarCommand;
using Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarCommand.CommandModels;
using Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarCommand.Result;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Majestic.WarehouseService.WebApi.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        [HttpPost(Name = "CreateCar")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ServiceResultWrapper<CreateCarResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ServiceResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Summary = "Create car", Description = "Create car request after dealers review", Tags = new[] { "Cars" })]
        public async Task<IActionResult> CreateCarAsync([FromBody] CreateCarRequest request, [FromServices] ICreateCarCommandService command)
        {
            var result = await command.HandleAsync(new CreateCarModelCommand(request));

            if (result.Successful)
            {
                return Ok(result.Result);
            }

            switch (result.Reason)
            {
                case CreateCarFlowResult.Reasons.UnexpectedError:
                    return BadRequest(new ServiceResult("Failed to process card creation"));
                default:
                    throw new ArgumentOutOfRangeException(nameof(result.Reason), result.Reason, null);
            }
        }

        [HttpGet(Name = "QueryCars")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PaginatedServiceResultWrapper<GetCarResponse, GetCarFilter>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ServiceResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Summary = "Query cars", Description = "Query cars from system", Tags = new[] { "Cars" })]
        public async Task<IActionResult> QueryCarsAsync([FromQuery] GetCarFilter filter, [FromServices] IGetCarQueryService query)
        {
            var result = await query.HandleAsync(new GetCarsModelQuery(filter));

            if (result.Successful)
            {
                return Ok(result.Result);
            }

            switch (result.Reason)
            {
                case GetCarsFlowResult.Reasons.UnexpectedError:
                    return BadRequest(new ServiceResult("Failed to process card query"));
                case GetCarsFlowResult.Reasons.NotFound:
                    return BadRequest(new ServiceResult("Cars not found"));
                default:
                    throw new ArgumentOutOfRangeException(nameof(result.Reason), result.Reason, null);
            }
        }

        [HttpPost("process-sell", Name = "ProcessSell")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ServiceResultWrapper<CreateCarResponse>), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType(typeof(ServiceResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Summary = "Process Sell", Description = "Processing cars sell", Tags = new[] { "Cars" })]
        public async Task<IActionResult> ProcessSellCarAsync([FromBody] ProcessSellCarRequest request, [FromServices] IProcessSellCarCommandService command)
        {
            var result = await command.HandleAsync(new ProcessSellCarModelCommand(request));

            if (result.Successful)
            {
                return Accepted(result.Result);
            }

            switch (result.Reason)
            {
                case ProcessCarFlowResult.Reasons.UnexpectedError:
                    return BadRequest(new ServiceResult("Failed to process card sell"));
                default:
                    throw new ArgumentOutOfRangeException(nameof(result.Reason), result.Reason, null);
            }
        }
    }
}

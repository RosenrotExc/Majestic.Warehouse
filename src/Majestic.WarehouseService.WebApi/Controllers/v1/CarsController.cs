using System.Net;
using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.CreateCars.Request;
using Majestic.WarehouseService.Models.v1.GetCars.Request;
using Majestic.WarehouseService.Models.v1.GetCars.Response;
using Majestic.WarehouseService.Models.v1.ProcessSellCar.Request;
using Majestic.WarehouseService.Models.v1.UpdateCars.Request;
using Majestic.WarehouseService.Services.Services.Cars.CreateCarCommand;
using Majestic.WarehouseService.Services.Services.Cars.CreateCarCommand.CommandModels;
using Majestic.WarehouseService.Services.Services.Cars.CreateCarCommand.Result;
using Majestic.WarehouseService.Services.Services.Cars.DeleteCarCommand;
using Majestic.WarehouseService.Services.Services.Cars.DeleteCarCommand.CommandModels;
using Majestic.WarehouseService.Services.Services.Cars.DeleteCarCommand.Result;
using Majestic.WarehouseService.Services.Services.Cars.GetCarQuery;
using Majestic.WarehouseService.Services.Services.Cars.GetCarQuery.QueryModels;
using Majestic.WarehouseService.Services.Services.Cars.GetCarQuery.Result;
using Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarCommand;
using Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarCommand.CommandModels;
using Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarCommand.Result;
using Majestic.WarehouseService.Services.Services.Cars.UpdateCarCommand;
using Majestic.WarehouseService.Services.Services.Cars.UpdateCarCommand.CommandModels;
using Majestic.WarehouseService.Services.Services.Cars.UpdateCarCommand.Result;
using Majestic.WarehouseService.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Majestic.WarehouseService.WebApi.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        #region Create car
        [HttpPost(Name = "CreateCar")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ServiceResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ServiceResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Summary = "Create car", Description = "Create car request after dealers review", Tags = new[] { "Cars" })]
        public async Task<IActionResult> CreateCarAsync([FromBody] CreateCarsRequest request,
            [FromServices] ICreateCarCommandService command)
        {
            var initiator = User.GetStubInitiator();

            var result = await command.HandleAsync(new CreateCarModelCommand(request, initiator));

            if (result.Successful)
            {
                return Ok(result.Result);
            }

            switch (result.Reason)
            {
                case CreateCarFlowResult.Reasons.UnexpectedError:
                    return BadRequest(new ServiceResult("Failed to process car creation"));
                case CreateCarFlowResult.Reasons.ValidationError:
                    return BadRequest(result);
                default:
                    return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region Update car
        [HttpPut("{code}", Name = "UpdateCar")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ServiceResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ServiceResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Summary = "Update car", Description = "Update car request after dealers review", Tags = new[] { "Cars" })]
        public async Task<IActionResult> UpdateCarAsync([FromRoute] string code, [FromBody] UpdateCarRequest request,
            [FromServices] IUpdateCarCommandService command)
        {
            var initiator = User.GetStubInitiator();

            var result = await command.HandleAsync(new UpdateCarModelCommand(code, request, initiator));

            if (result.Successful)
            {
                return Ok(result.Result);
            }

            switch (result.Reason)
            {
                case UpdateCarFlowResult.Reasons.UnexpectedError:
                    return BadRequest(new ServiceResult("Failed to process car update"));
                case UpdateCarFlowResult.Reasons.ValidationError:
                    return BadRequest(result);
                default:
                    return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region Delete car
        [HttpDelete("{code}", Name = "DeleteCar")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ServiceResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ServiceResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Summary = "Delete car", Description = "Delete car request after dealers review", Tags = new[] { "Cars" })]
        public async Task<IActionResult> DeleteCarAsync([FromRoute] string code,
            [FromServices] IDeleteCarCommandService command)
        {
            var initiator = User.GetStubInitiator();

            var result = await command.HandleAsync(new DeleteCarModelCommand(code, initiator));

            if (result.Successful)
            {
                return Ok(result.Result);
            }

            switch (result.Reason)
            {
                case DeleteCarFlowResult.Reasons.UnexpectedError:
                    return BadRequest(new ServiceResult("Failed to process car deletion"));
                default:
                    return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region Query cars
        [HttpGet(Name = "QueryCars")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PaginatedServiceResultWrapper<IEnumerable<GetCarResponse>, GetCarFilter>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ServiceResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Summary = "Query cars", Description = "Query cars from system", Tags = new[] { "Cars" })]
        public async Task<IActionResult> QueryCarsAsync([FromQuery] GetCarFilter filter,
            [FromServices] IGetCarQueryService query)
        {
            var initiator = User.GetStubInitiator();

            var result = await query.HandleAsync(new GetCarsModelQuery(filter, initiator));

            if (result.Successful)
            {
                return Ok(result.Result);
            }

            switch (result.Reason)
            {
                case GetCarsFlowResult.Reasons.UnexpectedError:
                    return BadRequest(new ServiceResult("Failed to process car query"));
                case GetCarsFlowResult.Reasons.NotFound:
                    return BadRequest(new ServiceResult("Cars not found"));
                default:
                    return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region Process cars
        [HttpPost("process-sell", Name = "ProcessSell")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ServiceResult), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType(typeof(ServiceResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Summary = "Process Sell", Description = "Processing cars sell", Tags = new[] { "Cars" })]
        public async Task<IActionResult> ProcessSellCarAsync([FromBody] ProcessSellCarRequest request,
            [FromServices] IProcessSellCarCommandService command)
        {
            var initiator = User.GetStubInitiator();

            var result = await command.HandleAsync(new ProcessSellCarModelCommand(request, initiator, HttpContext.GetRequestId()));
            if (result.Successful)
            {
                return Accepted(result.Result);
            }

            switch (result.Reason)
            {
                case ProcessCarFlowResult.Reasons.UnexpectedError:
                    return BadRequest(new ServiceResult("Failed to process car sell"));
                default:
                    return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        #endregion
    }
}

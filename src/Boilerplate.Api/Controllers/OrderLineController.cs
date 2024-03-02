using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Boilerplate.Api.Application;
using Boilerplate.Domain.Aggregates.OrderLines;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Api.Controllers
{
    [ApiController]
    [Route("orderlines")]
    public class OrderLinesController(CancelOrderLineUseCaseHandler cancelOrderLineUseCaseHandler) : ControllerBase
    {
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel([FromRoute] string id,  [FromBody]CancelOrderLineUseCase useCase)
        {
            useCase.Id = id;
            
            await cancelOrderLineUseCaseHandler.Handle(useCase);

            return this.NoContent();
        }
    }
}
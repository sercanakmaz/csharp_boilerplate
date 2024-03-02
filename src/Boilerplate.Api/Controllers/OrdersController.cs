using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Boilerplate.Api.Application;
using Boilerplate.Domain.Aggregates.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Api.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersController(CreateOrderUseCaseHandler createOrderUseCaseHandler) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderUseCase useCase)
        {
            var createOrderResult = await createOrderUseCaseHandler.Handle(useCase);

            return this.Created($"orders/{createOrderResult.Content.Id}", createOrderResult.Content);
        }
    }
}

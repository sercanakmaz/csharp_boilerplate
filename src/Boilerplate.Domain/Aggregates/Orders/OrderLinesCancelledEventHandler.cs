using System.Threading.Tasks;
using Boilerplate.Domain.Aggregates.OrderLines;
using Boilerplate.Domain.Events.OrderLines;
using Boilerplate.Infrastructure.Domain;
using MongoDB.Bson;
using Created = Boilerplate.Domain.Events.Orders.Created;

namespace Boilerplate.Domain.Aggregates.Orders;

public class OrderLinesCancelledEventHandler(
    IOrderService orderService,
    IOrderLineRepository orderLineRepository): BaseEventHandler<Cancelled>
{
    
    protected override async Task OnHandle(Cancelled @event)
    {
        var orderLine = await orderLineRepository.FindById(ObjectId.Parse(@event.Id));
        
        await orderService.Complete(orderLine.OrderNumber, OrderCompleteReasons.Cancelled);
    }
}
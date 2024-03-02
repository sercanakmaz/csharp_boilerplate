using System.Threading.Tasks;
using Boilerplate.Domain.Events.Orders;
using Boilerplate.Infrastructure.Domain;

namespace Boilerplate.Domain.Aggregates.Orders;

public class OrderLinesCancelledEventHandler: BaseEventHandler<Created>
{
    protected override Task OnHandle(Created @event)
    {
        throw new System.NotImplementedException();
    }
}
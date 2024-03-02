using System.Threading.Tasks;

namespace Boilerplate.Infrastructure.Domain;

public interface IEventHandler
{
    Task Handle(IEvent @event);
}

public abstract class BaseEventHandler<TEvent> : IEventHandler where TEvent : IEvent
{
    protected abstract Task OnHandle(TEvent @event);
    
    public Task Handle(IEvent @event)
    {
        return OnHandle((TEvent)@event);
    }
}
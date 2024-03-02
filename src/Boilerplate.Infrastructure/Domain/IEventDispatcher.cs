using System.Threading.Tasks;

namespace Boilerplate.Infrastructure.Domain;

public interface IEventDispatcher
{
    Task Publish(IEvent @event);
}
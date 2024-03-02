using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Boilerplate.Infrastructure.Domain;

public sealed class EventContext
{
    private readonly List<IEvent> _raisedEvents = new List<IEvent>();
    private readonly List<IEvent> _dispatchedEvents = new List<IEvent>();

    public void AddRaised(IEvent @event)
    {
        lock (this)
        {
            _raisedEvents.Add(@event);
        }
    }

    public void AddDispatched(IEvent @event)
    {
        lock (this)
        {
            _dispatchedEvents.Add(@event);
        }
    }

    public IEvent TakeRaised()
    {
        lock (this)
        {
            var result = _raisedEvents.FirstOrDefault();

            if (result == null)
            {
                return null;
            }
            
            _dispatchedEvents.Remove(result);

            return result;
        }
    }

    public IEvent TakeDispatched()
    {
        lock (this)
        {
            var result = _dispatchedEvents.FirstOrDefault();

            if (result == null)
            {
                return null;
            }

            _dispatchedEvents.Remove(result);

            return result;
        }
    }
}
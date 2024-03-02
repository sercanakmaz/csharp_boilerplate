using System;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.Infrastructure.Domain;

public abstract class BaseUseCaseHandler<TUseCase, TUseCaseResultContent>(
    IServiceProvider serviceProvider,
    EventContext eventContext)
    where TUseCase : BaseUseCase
{
    protected abstract Task<UseCaseResult<TUseCaseResultContent>> OnHandle(TUseCase useCase);

    public async Task<UseCaseResult<TUseCaseResultContent>> Handle(TUseCase useCase)
    {
        var result = await OnHandle(useCase);

        DispatchEvents();

        return result;
    }

    private void DispatchEvents()
    {
        while (true)
        {
            var @event = eventContext.TakeRaised();
            if (@event == null)
            {
                break;
            }

            var baseEventHandlerType = typeof(EventHandler<>).MakeGenericType(@event.GetType());

            var eventHandlers = serviceProvider
                .GetServices(baseEventHandlerType)
                .Cast<IEventHandler>();

            foreach (var eventHandler in eventHandlers)
            {
                eventHandler.Handle(@event);
            }

            eventContext.AddDispatched(@event);
        }
    }
}
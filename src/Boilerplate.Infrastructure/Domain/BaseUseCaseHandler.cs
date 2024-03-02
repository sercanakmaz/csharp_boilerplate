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
        var middlewares = serviceProvider.GetServices<IUseCaseHandlerMiddleware>().ToList();

        foreach (var middleware in middlewares)
        {
            middleware.Before(useCase);
        }
        
        var result = await OnHandle(useCase);

        foreach (var middleware in middlewares)
        {
            middleware.After(useCase, result);
        }
        
        DispatchEvents();

        var eventDispatcher = serviceProvider.GetService<IEventDispatcher>();

        if (eventDispatcher != null)
        {
            while (true)
            {
                var @event = eventContext.TakeDispatched();
                if (@event == null)
                {
                    break;
                }

                await eventDispatcher.Publish(@event);
            }
        }

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
                .Cast<IEventHandler>()
                .ToList();

            foreach (var eventHandler in eventHandlers)
            {
                eventHandler.Handle(@event);
            }

            eventContext.AddDispatched(@event);
        }
    }
}
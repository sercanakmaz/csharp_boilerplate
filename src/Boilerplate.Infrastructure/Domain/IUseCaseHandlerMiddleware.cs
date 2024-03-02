namespace Boilerplate.Infrastructure.Domain;

public interface IUseCaseHandlerMiddleware
{
    void Before(BaseUseCase useCase);
    void After(BaseUseCase useCase, UseCaseResult useCaseResult);
}
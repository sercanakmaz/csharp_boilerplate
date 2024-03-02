using Boilerplate.Infrastructure.Domain;
using Boilerplate.Infrastructure.Exceptions;

namespace Boilerplate.Infrastructure.Persistence;


public class DuplicateEntityException : IdempotencyException
{
    public Entity ExistingEntity { get; set; }

    public DuplicateEntityException(Entity existingEntity)
        : base($"{existingEntity.GetType().Name} exists {existingEntity.Id}", null)
    {
        ExistingEntity = existingEntity;
    }
}
using System;
using System.Net;
using Boilerplate.Infrastructure.Exceptions;

namespace Boilerplate.Infrastructure.Persistence;

public class IdempotencyException : ExceptionBase
{
    public IdempotencyException(string message, Exception innerException)
        : base(message, HttpStatusCode.Conflict, innerException)
    {
    }
}
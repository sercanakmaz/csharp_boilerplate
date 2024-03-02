using System;
using System.Net;
using Boilerplate.Infrastructure.Exceptions;

namespace Boilerplate.Domain.Shared;

public class NotFoundException:ExceptionBase
{
    public NotFoundException() 
        : base(nameof(NotFoundException), HttpStatusCode.NotFound)
    {
    }
}
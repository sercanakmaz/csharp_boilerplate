using System;
using System.Net;
using Boilerplate.Infrastructure.Domain;
using Boilerplate.Infrastructure.Persistence;

namespace Boilerplate.Infrastructure.Exceptions;

public abstract class ExceptionBase : Exception
{
    public HttpStatusCode HttpStatusCode { get; protected set; }
    public string Code { get; set; }

    public ExceptionBase(string message, HttpStatusCode httpStatusCode)
        : base(message)
    {
        HttpStatusCode = httpStatusCode;
    }

    public ExceptionBase(string message, HttpStatusCode httpStatusCode, Exception innerException)
        : base(message, innerException)
    {
        HttpStatusCode = httpStatusCode;
    }

    public ExceptionBase(string code, string message, HttpStatusCode httpStatusCode)
        : base(message)
    {
        Code = code;
        HttpStatusCode = httpStatusCode;
    }

    public ExceptionBase(string code, string message, HttpStatusCode httpStatusCode, Exception innerException)
        : base(message, innerException)
    {
        Code = code;
        HttpStatusCode = httpStatusCode;
    }
}

using System.Collections.Generic;

namespace Boilerplate.Infrastructure.Domain;

public class UseCaseResult
{
    public static UseCaseResult<TContent> FromContent<TContent>(TContent content)
    {
        return new UseCaseResult<TContent> { Content = content };
    }
}

public sealed class UseCaseResult<TContent>: UseCaseResult
{
    internal UseCaseResult()
    {
        
    }
    public TContent Content { get; internal set; }
    public Dictionary<string, object> Meta { get; internal  set; }
}
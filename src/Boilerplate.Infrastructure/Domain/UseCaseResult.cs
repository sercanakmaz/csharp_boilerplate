using System.Collections.Generic;

namespace Boilerplate.Infrastructure.Domain;

public class UseCaseResult
{
    public Dictionary<string, object> Meta { get; internal  set; }
    internal object ContentAsObject { get; set; }
    public static UseCaseResult<TContent> FromContent<TContent>(TContent content)
    {
        return new UseCaseResult<TContent> { ContentAsObject= content, Content = content };
    }
}

public sealed class UseCaseResult<TContent>: UseCaseResult
{
    internal UseCaseResult()
    {
        
    }
    public TContent Content { get; internal set; }
}
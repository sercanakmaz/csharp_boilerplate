using System;

namespace Boilerplate.Infrastructure.Domain;

public interface IEvent
{
    public int Version { get;  set; }
    public string Id { get; set; }
    public DateTime UpdatedDate { get; set; }
}

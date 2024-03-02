using System;
using Boilerplate.Infrastructure.Domain;

namespace Boilerplate.Domain.Events.OrderLines;

public class Cancelled: IEvent
{
    public int Version { get; set; }
    public string Id { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string Status { get;  set; }
    public string Reason { get;  set; }
}
using System;
using Boilerplate.Infrastructure.Domain;

namespace Boilerplate.Domain.Events.Orders;

public class Completed: IEvent
{
    public int Version { get; set; }
    public string Id { get; set; }
    public string Reason { get; set; }
    public DateTime UpdatedDate { get; set; }
}
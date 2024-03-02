using System;
using Boilerplate.Infrastructure.Domain;

namespace Boilerplate.Domain.Events.Orders;

public class Created: IEvent
{
    public int Version { get; set; }
    public string Id { get; set; }
    public string OrderNumber { get; set; }
    public string UserId { get; set; }
    public DateTime UpdatedDate { get; set; }
}
using System;
using Boilerplate.Infrastructure.Domain;

namespace Boilerplate.Domain.Events.OrderLines;

public class Created: IEvent
{
    public int Version { get; set; }
    public string Id { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string Sku { get; set; }
    public decimal Price { get; set; }
    public string OrderNumber { get;  set; }
    public string Status { get;  set; }
}
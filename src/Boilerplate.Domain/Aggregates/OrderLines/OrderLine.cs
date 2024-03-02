using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ObjectiveC;
using System.Security;
using Boilerplate.Domain.Events.OrderLines;
using Boilerplate.Infrastructure.Domain;
using MongoDB.Bson.Serialization.Attributes;

namespace Boilerplate.Domain.Aggregates.OrderLines;

public sealed class OrderLine : Entity
{
    private OrderLine()
    {
    }

    public OrderLine(string sku, decimal price, string orderNumber)
    {
        this.Sku = sku;
        this.Price = price;
        this.OrderNumber = orderNumber;
        this.Status = OrderLineStatuses.Created;

        this.RaiseEvent(new Created
        {
            Id = Id.ToString(),
            Sku = this.Sku,
            Price = this.Price,
            OrderNumber = this.OrderNumber,
            Status = this.Status
        });
    }

    public string Sku { get; private set; }
    public decimal Price { get; private set; }
    public string OrderNumber { get; private set; }
    public string Status { get; private set; }

    public void Cancel()
    {
        Status = OrderLineStatuses.Cancelled;

        this.RaiseEvent(new Cancelled
        {
            Id = Id.ToString(),
            Status = this.Status
        });
    }
}

public class OrderLineStatuses
{
    public static string Created = nameof(Created);
    public static string Cancelled = nameof(Cancelled);
}
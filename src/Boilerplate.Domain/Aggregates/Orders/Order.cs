using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ObjectiveC;
using System.Security;
using Boilerplate.Domain.Events.Orders;
using Boilerplate.Infrastructure.Domain;
using MongoDB.Bson.Serialization.Attributes;

namespace Boilerplate.Domain.Aggregates.Orders;

public sealed class Order : Entity
{
    private Order()
    {
    }

    public Order(string orderNumber, string userId)
    {
        this.OrderNumber = orderNumber;
        this.UserId = userId;
        
        this.RaiseEvent(new Created
        {
            Id = Id.ToString(),
            OrderNumber = OrderNumber,
            UpdatedDate = UpdatedDate,
            UserId = userId
        });
    }
    public string OrderNumber { get;  set; }

    public string UserId { get; set; }
}
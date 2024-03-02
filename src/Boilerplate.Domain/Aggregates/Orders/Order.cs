using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ObjectiveC;
using System.Security;
using Amazon.Runtime.EventStreams;
using Boilerplate.Domain.Events.OrderLines;
using Boilerplate.Domain.Events.Orders;
using Boilerplate.Infrastructure.Domain;
using MongoDB.Bson.Serialization.Attributes;
using Created = Boilerplate.Domain.Events.Orders.Created;

namespace Boilerplate.Domain.Aggregates.Orders;

public sealed class Order : Entity
{
    public Order()
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

    public string OrderNumber { get; private set; }

    public string UserId { get; private set; }
    public string CompleteReason { get; private set; }

    public void Complete(string completeReason)
    {
        this.CompleteReason = completeReason;
        
        this.RaiseEvent(new Completed
        {
            Reason = completeReason
        });
    }
}

public static class OrderCompleteReasons
{
    public static string Cancelled = nameof(Cancelled);
}
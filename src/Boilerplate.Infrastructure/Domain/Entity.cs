namespace Boilerplate.Infrastructure.Domain;

using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public abstract class Entity
{
    public ObjectId Id { get; private set;  } = ObjectId.GenerateNewId();
    public DateTime InsertedDate { get; private set; } = DateTime.Now.ToUniversalTime();
    public DateTime UpdatedDate { get; private set; }
    public int Version { get; private set; }
    
    [BsonIgnore] private IList<IEvent> _uncommittedEvents { get; set; }
    
    public IList<IEvent> GetUncommittedEvents()
    {
        return this._uncommittedEvents;
    }

    protected virtual void RaiseEvent(IEvent @event)
    {
        this.UpdatedDate = DateTime.Now.ToUniversalTime();
        this.Version++;
        @event.Version = this.Version;
            
        if (this._uncommittedEvents == null)
        {
            this._uncommittedEvents = new List<IEvent>();
        }

        this._uncommittedEvents.Add(@event);
    }
}
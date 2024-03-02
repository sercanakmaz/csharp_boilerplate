using MongoDB.Bson;

namespace Boilerplate.Infrastructure.Persistence.Mongo;

public static class MongoExtensions
{
    public static ObjectId ToObjectId(this string obj)
    {
        return ObjectId.Parse(obj);
    }
}
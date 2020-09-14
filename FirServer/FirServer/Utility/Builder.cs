using MongoDB.Bson;
using MongoDB.Driver;

namespace FirServer.Utility
{
    public static class Builder
    {
        public static FilterDefinition<BsonDocument> FilterEq(string field, string value)
        {
            return Builders<BsonDocument>.Filter.Eq(field, value);
        }

        public static FilterDefinition<BsonDocument> FilterEq<T>(string field, T value)
        {
            return Builders<BsonDocument>.Filter.Eq(field, value);
        }

        public static FilterDefinition<BsonDocument> FilterEq(string field, ObjectId id)
        {
            return Builders<BsonDocument>.Filter.Eq(field, id);
        }

        public static UpdateDefinition<BsonDocument> Update<T>(string field, T value)
        {
            return Builders<BsonDocument>.Update.Push(field, value);
        }
    }
}

using MongoDB.Bson;
using MongoDB.Driver;

namespace FirCommon.Utility
{
    public static class Builder
    {
        public static FilterDefinition<BsonDocument> FilterEq(string field, string value)
        {
            return Builders<BsonDocument>.Filter.Eq(field, value);
        }

        public static FilterDefinition<T> FilterEq<T>(string field, object value)
        {
            return Builders<T>.Filter.Eq(field, value);
        }

        public static FilterDefinition<BsonDocument> FilterEq(string field, ObjectId id)
        {
            return Builders<BsonDocument>.Filter.Eq(field, id);
        }

        public static UpdateDefinition<T> Update<T>(string field, object value)
        {
            return Builders<T>.Update.Set(field, value);
        }
    }
}

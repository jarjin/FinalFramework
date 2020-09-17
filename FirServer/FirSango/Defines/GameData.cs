using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GameLibs.FirSango.Defines
{
    public class UserInfo
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public long uid { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public long money { get; set; }
        public string lasttime { get; set; }
    }
}

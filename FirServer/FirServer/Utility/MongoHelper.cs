using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FirServer.Utility
{
    class MongoHelper
    {
        public async void Initialize()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("foo");
            var collection = database.GetCollection<BsonDocument>("bar");

            await collection.InsertOneAsync(new BsonDocument("Name", "Jack"));
            var list = await collection.Find(new BsonDocument("Name", "Jack")).ToListAsync();

            foreach (var document in list)
            {
                Console.WriteLine(document["Name"]);
            }
        }
    }
}

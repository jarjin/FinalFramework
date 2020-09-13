using System;
using System.Collections.Generic;
using System.Linq;
using log4net.Core;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace FirServer.Utility
{
    public class MongoHelper : IDisposable
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;
        protected static ILogger _logger;

        public MongoHelper(string dbname, string connectUrl)
        {
            _client = new MongoClient(connectUrl);
            _database = _client.GetDatabase(dbname);
        }

        public MongoHelper(string dbname, ILogger log)
        {
            _client = new MongoClient();
            _database = _client.GetDatabase(dbname);
            _logger = log;
        }

        public long? Count<T>(string collectionName)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                return collection.CountDocuments(collectionName);
            }
            catch (Exception ex)
            {
                WriteError("Count", "Count(string collectionName)", ex.Message);
            }
            return null;
        }

        public long Count(string collectionName, FilterDefinition<BsonDocument> filter)
        {
            var collection = _database.GetCollection<BsonDocument>(collectionName);
            return collection.Find(filter).Count();
        }

        public long Count(string collectionName, string field, string value)
        {
            var collection = _database.GetCollection<BsonDocument>(collectionName);
            return collection.Find(Builders<BsonDocument>.Filter.Eq(field, value)).Count();
        }

        public long Count(string collectionName, string field, ObjectId id)
        {
            var collection = _database.GetCollection<BsonDocument>(collectionName);
            return collection.Find(Builders<BsonDocument>.Filter.Eq(field, id)).Count();
        }

        public long Count<T>(string collectionName, string field, T value)
        {
            var collection = _database.GetCollection<BsonDocument>(collectionName);
            return collection.Find(Builders<BsonDocument>.Filter.Eq<T>(field, value)).Count();
        }

        public List<T> Select<T>(string collectionName, FilterDefinition<BsonDocument> filter)
        {
            var collection = _database.GetCollection<BsonDocument>(collectionName);
            var result = collection.Find(filter).ToList();
            List<T> returnList = new List<T>();
            foreach (var l in result)
            {
                returnList.Add(BsonSerializer.Deserialize<T>(l));
            }
            return returnList;
        }

        public T SelectOne<T>(string collectionName, FilterDefinition<BsonDocument> filter)
        {
            var collection = _database.GetCollection<BsonDocument>(collectionName);
            var result = collection.Find(filter).ToList();
            if (result.Count > 1)
            {
                throw new Exception("To many results");
            }
            return BsonSerializer.Deserialize<T>(result.ElementAt(0));
        }

        public bool Insert(string collectionName, BsonDocument doc)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                collection.InsertOne(doc);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Insert<T>(string collectionName, T doc)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                collection.InsertOne(doc.ToBsonDocument());
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool InsertMany<T>(string collectionName, IEnumerable<T> documents)
        {
            try
            {
                List<BsonDocument> docs = new List<BsonDocument>();
                for (int i = 0; i < documents.Count(); i++)
                {
                    docs[i] = documents.ElementAt(i).ToBsonDocument();
                }
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                collection.InsertMany(docs);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateOne(string collectionName, FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                collection.UpdateOne(filter, update);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateOne(string collectionName, string fieldName, string value, UpdateDefinition<BsonDocument> update)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq(fieldName, value);
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                collection.UpdateOne(filter, update);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateArray<T>(string collectionName, string arrayField, List<T> list, FilterDefinition<BsonDocument> filter)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                var update = Builders<BsonDocument>.Update.PushEach(arrayField, list);
                collection.FindOneAndUpdate<BsonDocument>(filter, update);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(string collectionName, FilterDefinition<BsonDocument> filter)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string collectionName, string fieldName, string value)
        {
            var filter = Builders<BsonDocument>.Filter.Eq(fieldName, value);
            throw new NotImplementedException();
        }

        private void WriteError(string title, string function, string message)
        {
            if (_logger != null)
            {
                //_logger.Log(title, function, message);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

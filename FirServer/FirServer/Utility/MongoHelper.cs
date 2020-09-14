using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        
        #region Count Function
        public long Count(string collectionName)
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
            return 0;
        }

        public async Task<long> CountAsync(string collectionName)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                return await collection.CountDocumentsAsync(collectionName);
            }
            catch (Exception ex)
            {
                WriteError("Count", "Count(string collectionName)", ex.Message);
            }
            return 0;
        }

        public long Count(string collectionName, FilterDefinition<BsonDocument> filter)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                return collection.CountDocuments(filter);
            }
            catch (Exception ex)
            {
                WriteError("Count", "Count(string collectionName)", ex.Message);
            }
            return 0;
        }

        public long Count(string collectionName, string field, string value)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                return collection.CountDocuments(Builder.FilterEq(field, value));
            }
            catch (Exception ex)
            {
                WriteError("Count", "Count(string collectionName)", ex.Message);
            }
            return 0;
        }

        public long Count(string collectionName, string field, ObjectId id)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                return collection.CountDocuments(Builder.FilterEq(field, id));
            }
            catch (Exception ex)
            {
                WriteError("Count", "Count(string collectionName)", ex.Message);
            }
            return 0;
        }

        public long Count<T>(string collectionName, string field, T value)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                return collection.CountDocuments(Builder.FilterEq(field, value));
            }
            catch (Exception ex)
            {
                WriteError("Count", "Count(string collectionName)", ex.Message);
            }
            return 0;
        }
        #endregion


        #region Select Function
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

        public async Task<List<T>> SelectAsync<T>(string collectionName, FilterDefinition<BsonDocument> filter)
        {
            var collection = _database.GetCollection<BsonDocument>(collectionName);
            var result = await collection.Find(filter).ToListAsync();
            List<T> returnList = new List<T>();
            foreach (var l in result)
            {
                returnList.Add(BsonSerializer.Deserialize<T>(l));
            }
            return returnList;
        }
        #endregion


        #region Insert Function
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

        public async Task<bool> InsertAsync(string collectionName, BsonDocument doc)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                await collection.InsertOneAsync(doc);
                return true;
            }
            catch (Exception ex)
            {
                WriteError("InsertAsync", "InsertAsync(string collectionName)", ex.Message);
            }
            return false;
        }

        public bool InsertMany<T>(string collectionName, IEnumerable<T> documents)
        {
            try
            {
                var docs = new List<BsonDocument>();
                var count = documents.Count();
                for (int i = 0; i < count; i++)
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

        public async Task<bool> InsertManyAsync<T>(string collectionName, IEnumerable<T> documents)
        {
            try
            {
                var docs = new List<BsonDocument>();
                var count = documents.Count();
                for (int i = 0; i < count; i++)
                {
                    docs[i] = documents.ElementAt(i).ToBsonDocument();
                }
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                await collection.InsertManyAsync(docs);
                return true;
            }
            catch (Exception ex)
            {
                WriteError("InsertManyAsync", "InsertManyAsync(collectionName, documents)", ex.Message);
            }
            return false;
        }
        #endregion


        #region Update Function
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
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                collection.UpdateOne(Builder.FilterEq(fieldName, value), update);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateOneAsync(string collectionName, string fieldName, string value, UpdateDefinition<BsonDocument> update)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                await collection.UpdateOneAsync(Builder.FilterEq(fieldName, value), update);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public long UpdateMany<T>(string collectionName, string arrayField, List<T> list, FilterDefinition<BsonDocument> filter)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                var update = Builders<BsonDocument>.Update.PushEach(arrayField, list);
                var result = collection.UpdateMany(filter, update);
                if (result.IsModifiedCountAvailable)
                {
                    return result.ModifiedCount;
                }
            }
            catch { }
            return 0;
        }

        public async Task<long> UpdateManyAsync<T>(string collectionName, string arrayField, List<T> list, FilterDefinition<BsonDocument> filter)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                var update = Builders<BsonDocument>.Update.PushEach(arrayField, list);
                var result = await collection.UpdateManyAsync(filter, update);
                if (result.IsModifiedCountAvailable)
                {
                    return result.ModifiedCount;
                }
            }
            catch { }
            return 0;
        }
        #endregion


        #region Delete Function
        public bool Delete(string collectionName, FilterDefinition<BsonDocument> filter)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                collection.DeleteOne(filter);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(string collectionName, string fieldName, string value)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                collection.DeleteOne(Builder.FilterEq(fieldName, value));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string collectionName, string fieldName, string value)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                await collection.DeleteOneAsync(Builder.FilterEq(fieldName, value));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public long DeleteMany(string collectionName, FilterDefinition<BsonDocument> filter)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                var result = collection.DeleteMany(filter);
                return result.DeletedCount;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<long> DeleteManyAsync(string collectionName, string fieldName, string value)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                var result = await collection.DeleteManyAsync(Builder.FilterEq(fieldName, value));
                return result.DeletedCount;
            }
            catch
            {
                return 0;
            }
        }

        #endregion

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

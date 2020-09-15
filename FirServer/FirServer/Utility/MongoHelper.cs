using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using log4net;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace FirServer.Utility
{
    public class MongoHelper : IDisposable
    {
        private static IMongoClient mClient;
        private static IMongoDatabase mDatabase;
        private static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(MongoHelper));

        public MongoHelper(string dbname, string connectUrl)
        {
            mClient = new MongoClient(connectUrl);
            mDatabase = mClient.GetDatabase(dbname);
        }
        
        #region Count Function
        public long Count(string collectionName)
        {
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            try
            {
                var collection = mDatabase.GetCollection<BsonDocument>(collectionName);
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
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            try
            {
                var collection = mDatabase.GetCollection<BsonDocument>(collectionName);
                return await collection.CountDocumentsAsync(collectionName);
            }
            catch (Exception ex)
            {
                WriteError("Count", "Count(string collectionName)", ex.Message);
            }
            return 0;
        }

        public long Count<T>(string collectionName, Expression<Func<T, bool>> filter) where T : BsonDocument
        {
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            try
            {
                var collection = mDatabase.GetCollection<T>(collectionName);
                return collection.CountDocuments(filter);
            }
            catch (Exception ex)
            {
                WriteError("Count", "Count(string collectionName)", ex.Message);
            }
            return 0;
        }

        public async Task<long> CountAsync<T>(string collectionName, Expression<Func<T, bool>> filter) where T : BsonDocument
        {
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            try
            {
                var collection = mDatabase.GetCollection<T>(collectionName);
                return await collection.CountDocumentsAsync(filter);
            }
            catch (Exception ex)
            {
                WriteError("Count", "Count(string collectionName)", ex.Message);
            }
            return 0;
        }
        #endregion


        #region Select Function 
        public List<T> Select<T>(string collectionName, Expression<Func<T, bool>> filter) where T : BsonDocument
        {
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            var collection = mDatabase.GetCollection<T>(collectionName);
            var result = collection.Find(filter).ToList();
            var returnList = new List<T>();
            foreach (var l in result)
            {
                returnList.Add(BsonSerializer.Deserialize<T>(l));
            }
            return returnList;
        }

        public T SelectOne<T>(string collectionName, Expression<Func<T, bool>> filter) where T : BsonDocument
        {
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            var collection = mDatabase.GetCollection<T>(collectionName);
            var result = collection.Find(filter).FirstOrDefault();
            return BsonSerializer.Deserialize<T>(result);
        }

        public T SelectOne<T>(string collectionName, ProjectionDefinition<T> projection, Expression<Func<T, bool>> filter) where T : BsonDocument
        {
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            var collection = mDatabase.GetCollection<T>(collectionName);
            var result = collection.Find<T>(filter).Project(projection).FirstOrDefault();
            return BsonSerializer.Deserialize<T>(result);
        }

        public async Task<List<T>> SelectAsync<T>(string collectionName, Expression<Func<T, bool>> filter) where T : BsonDocument
        {
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            var collection = mDatabase.GetCollection<T>(collectionName);
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
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            try
            {
                var collection = mDatabase.GetCollection<BsonDocument>(collectionName);
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
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            try
            {
                var collection = mDatabase.GetCollection<BsonDocument>(collectionName);
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
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            try
            {
                var collection = mDatabase.GetCollection<BsonDocument>(collectionName);
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
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            try
            {
                var docs = new List<BsonDocument>();
                var count = documents.Count();
                for (int i = 0; i < count; i++)
                {
                    docs[i] = documents.ElementAt(i).ToBsonDocument();
                }
                var collection = mDatabase.GetCollection<BsonDocument>(collectionName);
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
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            try
            {
                var docs = new List<BsonDocument>();
                var count = documents.Count();
                for (int i = 0; i < count; i++)
                {
                    docs[i] = documents.ElementAt(i).ToBsonDocument();
                }
                var collection = mDatabase.GetCollection<BsonDocument>(collectionName);
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
        public bool UpdateOne<T>(string collectionName, UpdateDefinition<T> update, Expression<Func<T, bool>> filter) where T : BsonDocument
        {
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            try
            {
                var collection = mDatabase.GetCollection<T>(collectionName);
                collection.UpdateOne(filter, update);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateOneAsync<T>(string collectionName, UpdateDefinition<T> update, Expression<Func<T, bool>> filter) where T : BsonDocument
        {
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            try
            {
                var collection = mDatabase.GetCollection<T>(collectionName);
                await collection.UpdateOneAsync(filter, update);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public long UpdateMany<T>(string collectionName, string arrayField, List<T> list, Expression<Func<T, bool>> filter) where T : BsonDocument
        {
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            try
            {
                var collection = mDatabase.GetCollection<T>(collectionName);
                var update = Builders<T>.Update.PushEach(arrayField, list);
                var result = collection.UpdateMany(filter, update);
                if (result.IsModifiedCountAvailable)
                {
                    return result.ModifiedCount;
                }
            }
            catch (Exception ex)
            {
                WriteError("UpdateMany", "UpdateMany", ex.Message);
            }
            return 0;
        }

        public async Task<long> UpdateManyAsync<T>(string collectionName, string arrayField, List<T> list, Expression<Func<T, bool>> filter) where T : BsonDocument
        {
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            try
            {
                var collection = mDatabase.GetCollection<T>(collectionName);
                var update = Builders<T>.Update.PushEach(arrayField, list);
                var result = await collection.UpdateManyAsync(filter, update);
                if (result.IsModifiedCountAvailable)
                {
                    return result.ModifiedCount;
                }
            }
            catch (Exception ex)
            {
                WriteError("UpdateManyAsync", "UpdateManyAsync", ex.Message);
            }
            return 0;
        }
        #endregion


        #region Delete Function
        public bool Delete<T>(string collectionName, Expression<Func<T, bool>> filter) where T : BsonDocument
        {
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            try
            {
                var collection = mDatabase.GetCollection<T>(collectionName);
                collection.DeleteOne(filter);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync<T>(string collectionName, Expression<Func<T, bool>> filter) where T : BsonDocument
        {
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            try
            {
                var collection = mDatabase.GetCollection<T>(collectionName);
                var result = await collection.DeleteOneAsync(filter);
                return result.DeletedCount > 0;
            }
            catch
            {
                return false;
            }
        }

        public long DeleteMany<T>(string collectionName, Expression<Func<T, bool>> filter) where T : BsonDocument
        {
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            try
            {
                var collection = mDatabase.GetCollection<T>(collectionName);
                var result = collection.DeleteMany(filter);
                return result.DeletedCount;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<long> DeleteManyAsync<T>(string collectionName, Expression<Func<T, bool>> filter) where T : BsonDocument
        {
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            try
            {
                var collection = mDatabase.GetCollection<T>(collectionName);
                var result = await collection.DeleteManyAsync(filter);
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
            if (logger != null)
            {
                logger.Error(function + message);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

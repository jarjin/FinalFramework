using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace FirCommon.DataBase
{
    public class MongoHelper : IDisposable
    {
        private static IMongoClient mClient;
        private static IMongoDatabase mDatabase;

        public MongoHelper(string connUrl)
        {
            mClient = new MongoClient(connUrl);
        }

        public void OpenDB(string dbName)
        {
            mDatabase = mClient.GetDatabase(dbName);
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
                WriteError(ex.Message);
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
                WriteError(ex.Message);
            }
            return 0;
        }

        public long Count<T>(string collectionName, Expression<Func<T, bool>> filter) 
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
                WriteError(ex.Message);
            }
            return 0;
        }

        public async Task<long> CountAsync<T>(string collectionName, Expression<Func<T, bool>> filter)
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
                WriteError(ex.Message);
            }
            return 0;
        }
        #endregion


        #region Select Function 
        public List<T> Select<T>(string collectionName, Expression<Func<T, bool>> filter = null) 
        {
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            var collection = mDatabase.GetCollection<T>(collectionName);
            if (filter == null)
            {
                return collection.Find(new BsonDocument()).ToList<T>();
            }         
            return collection.Find(filter).ToList<T>();
        }

        public T SelectOne<T>(string collectionName, Expression<Func<T, bool>> filter)
        {
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            var collection = mDatabase.GetCollection<T>(collectionName);
            return collection.Find(filter).FirstOrDefault();
        }

        public T SelectOne<T>(string collectionName, ProjectionDefinition<T> projection, Expression<Func<T, bool>> filter)
        {
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            var collection = mDatabase.GetCollection<T>(collectionName);
            var result = collection.Find(filter).Project(projection).FirstOrDefault();
            if (result == null)
            {
                return default(T);
            }
            return BsonSerializer.Deserialize<T>(result);
        }

        public async Task<List<T>> SelectAsync<T>(string collectionName, Expression<Func<T, bool>> filter)
        {
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            var collection = mDatabase.GetCollection<T>(collectionName);
            return await collection.Find(filter).ToListAsync<T>();
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
            catch (Exception ex)
            {
                WriteError(ex.Message);
            }
            return false;
        }

        public bool Insert<T>(string collectionName, T doc)
        {
            if (mDatabase == null)
            {
                throw new Exception("MongoDB database was null!!!");
            }
            try
            {
                var collection = mDatabase.GetCollection<T>(collectionName);
                collection.InsertOne(doc);
                return true;
            }
            catch (Exception ex)
            {
                WriteError(ex.Message);
            }
            return false;
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
                WriteError(ex.Message);
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
            catch (Exception ex)
            {
                WriteError(ex.Message);
            }
            return false;
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
                WriteError(ex.Message);
            }
            return false;
        }
        #endregion


        #region Update Function
        public bool UpdateOne<T>(string collectionName, UpdateDefinition<T> update, Expression<Func<T, bool>> filter)
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
            catch (Exception ex)
            {
                WriteError(ex.Message);
            }
            return false;
        }

        public async Task<bool> UpdateOneAsync<T>(string collectionName, UpdateDefinition<T> update, Expression<Func<T, bool>> filter)
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
            catch (Exception ex)
            {
                WriteError(ex.Message);
            }
            return false;
        }

        public long UpdateMany<T>(string collectionName, string arrayField, List<T> list, Expression<Func<T, bool>> filter) 
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
                WriteError(ex.Message);
            }
            return 0;
        }

        public async Task<long> UpdateManyAsync<T>(string collectionName, string arrayField, List<T> list, Expression<Func<T, bool>> filter) 
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
                WriteError(ex.Message);
            }
            return 0;
        }
        #endregion


        #region Delete Function
        public bool Delete<T>(string collectionName, Expression<Func<T, bool>> filter)
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
            catch (Exception ex)
            {
                WriteError(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteAsync<T>(string collectionName, Expression<Func<T, bool>> filter) 
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
            catch (Exception ex)
            {
                WriteError(ex.Message);
                return false;
            }
        }

        public long DeleteMany<T>(string collectionName, Expression<Func<T, bool>> filter) 
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
            catch (Exception ex)
            {
                WriteError(ex.Message);
                return 0;
            }
        }

        public async Task<long> DeleteManyAsync<T>(string collectionName, Expression<Func<T, bool>> filter)
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
            catch (Exception ex)
            {
                WriteError(ex.Message);
                return 0;
            }
        }
        #endregion

        private void WriteError(string message)
        {
            Console.WriteLine(message);
        }

        public void DropDB(string dbName)
        {
            if (mClient != null)
            {
                mClient.DropDatabase(dbName);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

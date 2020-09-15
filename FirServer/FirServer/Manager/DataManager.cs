using System.Collections.Generic;
using log4net;
using FirServer.Interface;
using FirServer.Utility;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using System;

namespace FirServer.Manager
{
    public class DataManager : BaseBehaviour, IManager
    {
        private static MongoHelper mongoHelper = null;
        private static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(DataManager));

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            dataMgr = this;
            mongoHelper = new MongoHelper("sango", "mongodb://localhost:27017");
        }

        /// <summary>
        /// 添加一行
        /// </summary>
        public bool Add(string tabName, Dictionary<string, object> values)
        {
            var doc = new BsonDocument(values);
            return mongoHelper.Insert<BsonDocument>(tabName, doc);
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        public void Set<T>(string tabName, UpdateDefinition<T> update, Expression<Func<T, bool>> filter) where T : BsonDocument
        {
            mongoHelper.UpdateOne<T>(tabName, update, filter);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        public BsonValue Get<T>(string tabName, string strKey, Expression<Func<T, bool>> filter) where T : BsonDocument
        {
            var projection = Builders<T>.Projection.Include(strKey);
            var doc = mongoHelper.SelectOne<T>(tabName, projection, filter);
            if (doc != null)
            {
                return doc.GetValue(strKey);
            }
            return null;
        }

        /// <summary>
        /// 获取一行
        /// </summary>
        public BsonDocument GetDoc<T>(string tabName, Expression<Func<T, bool>> filter) where T : BsonDocument
        {
            return mongoHelper.SelectOne<T>(tabName, filter);
        }

        /// <summary>
        /// 组合查询
        /// </summary>
        public List<T> Query<T>(string tabName, Expression<Func<T, bool>> filter) where T : BsonDocument
        {
            return mongoHelper.Select<T>(tabName, filter);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            mongoHelper = null;
        }

        public void OnDispose()
        {
            dataMgr = null;
        }
    }
}

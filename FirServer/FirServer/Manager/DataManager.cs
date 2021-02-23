using System.Collections.Generic;
using log4net;
using MongoDB.Driver;
using System.Linq.Expressions;
using System;
using FirCommon.DataBase;

namespace FirServer.Manager
{
    public class DataManager : BaseManager
    {
        private static MongoHelper mongoHelper = null;
        private static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(DataManager));

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        public void Connect(string url)
        {
            mongoHelper = new MongoHelper(url);
        }

        /// <summary>
        /// 打开数据库
        /// </summary>
        public void OpenDB(string dbName)
        {
            mongoHelper.OpenDB(dbName);
        }

        /// <summary>
        /// 添加一行
        /// </summary>
        public bool Add<T>(string tabName, T doc)
        {
            return mongoHelper.Insert<T>(tabName, doc);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        public T Get<T>(string tabName, string strKey, Expression<Func<T, bool>> filter)
        {
            var projection = Builders<T>.Projection.Include(strKey);
            return mongoHelper.SelectOne<T>(tabName, projection, filter);
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        public void Set<T>(string tabName, UpdateDefinition<T> update, Expression<Func<T, bool>> filter)
        {
            mongoHelper.UpdateOne<T>(tabName, update, filter);
        }

        /// <summary>
        /// 获取一行
        /// </summary>
        public T GetDoc<T>(string tabName, Expression<Func<T, bool>> filter) 
        {
            return mongoHelper.SelectOne<T>(tabName, filter);
        }

        /// <summary>
        /// 组合查询
        /// </summary>
        public List<T> Query<T>(string tabName, Expression<Func<T, bool>> filter = null) 
        {
            return mongoHelper.Select<T>(tabName, filter);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        public T Exist<T>(string tabName, Expression<Func<T, bool>> filter)
        {
            return Get<T>(tabName, "uid", filter);
        }

        /// <summary>
        /// 删除数据库
        /// </summary>
        public void DropDB(string dbName)
        {
            mongoHelper.DropDB(dbName);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            mongoHelper = null;
        }
    }
}

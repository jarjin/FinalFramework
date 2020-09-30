using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FirServer.Model
{
    public class BaseModel : BaseBehaviour
    {
        protected string tableName;

        public string TableName
        {
            get { return tableName; }
        }

        public BaseModel(string tabName)
        {
            this.tableName = tabName;
        }

        /// <summary>
        /// 添加
        /// </summary>
        protected bool Add<T>(T doc)
        {
            if (string.IsNullOrEmpty(tableName) || dataMgr == null)
            {
                throw new Exception();
            }
            return dataMgr.Add(tableName, doc);
        }

        /// <summary>
        /// 获取
        /// </summary>
        protected T Get<T>(string strKey, Expression<Func<T, bool>> filter) 
        {
            if (string.IsNullOrEmpty(tableName) || dataMgr == null)
            {
                throw new Exception();
            }
            return dataMgr.Get<T>(tableName, strKey, filter);
        }

        /// <summary>
        /// 设置
        /// </summary>
        protected void Set<T>(UpdateDefinition<T> update, Expression<Func<T, bool>> filter) 
        {
            if (string.IsNullOrEmpty(tableName) || dataMgr == null)
            {
                throw new Exception();
            }
            dataMgr.Set(tableName, update, filter);
        }

        /// <summary>
        /// 获取文档
        /// </summary>
        public T GetDoc<T>(Expression<Func<T, bool>> filter)
        {
            return dataMgr.GetDoc(tableName, filter);
        }

        /// <summary>
        /// 查询结果集
        /// </summary>
        public List<T> Query<T>(Expression<Func<T, bool>> filter = null) 
        {
            if (string.IsNullOrEmpty(tableName) || dataMgr == null)
            {
                throw new Exception();
            }
            return dataMgr.Query(tableName, filter);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        public T Exist<T>(Expression<Func<T, bool>> filter) 
        {
            if (string.IsNullOrEmpty(tableName) || dataMgr == null)
            {
                throw new Exception();
            }
            return dataMgr.Exist<T>(tableName, filter);
        }
    }
}

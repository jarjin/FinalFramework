using System;
using System.Collections.Generic;
using System.Data;

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

        protected string Get(string uid, string strKey)
        {
            if (string.IsNullOrEmpty(tableName) || dataMgr == null)
            {
                throw new Exception();
            }
            return dataMgr.Get(tableName, uid, strKey);
        }

        protected void Set(string uid, string strKey, string value)
        {
            if (string.IsNullOrEmpty(tableName) || dataMgr == null)
            {
                throw new Exception();
            }
            dataMgr.Set(tableName, uid, strKey, value);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        protected long Add(List<string> values)
        {
            if (string.IsNullOrEmpty(tableName) || dataMgr == null)
            {
                throw new Exception();
            }
            return dataMgr.Add(tableName, values);
        }

        /// <summary>
        /// 获取一行
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public DataRow GetRow(List<string> values = null)
        {
            if (string.IsNullOrEmpty(tableName) || dataMgr == null)
            {
                throw new Exception();
            }
            return dataMgr.GetRow(tableName, values);
        }

        /// <summary>
        /// 查询结果集
        /// </summary>
        /// <param name="values"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public DataSet Query(List<string> values = null, int rowCount = 0)
        {
            if (string.IsNullOrEmpty(tableName) || dataMgr == null)
            {
                throw new Exception();
            }
            return dataMgr.Query(tableName, values, rowCount);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool Exist(string uid)
        {
            if (string.IsNullOrEmpty(tableName) || dataMgr == null)
            {
                throw new Exception();
            }
            return dataMgr.Exist(tableName, uid);
        }
    }
}

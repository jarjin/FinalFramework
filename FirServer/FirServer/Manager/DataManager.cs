using System.Collections.Generic;
using System.Data;
using log4net;
using FirServer.Interface;
using FirServer.Utility;

namespace FirServer.Manager
{
    public class DataManager : BaseBehaviour, IManager
    {
        const int expireTime = 86400 * 3;
        private static MongoHelper mongoHelper = null;
        private static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(DataManager));

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            dataMgr = this;
            mongoHelper = new MongoHelper("foo", "mongodb://localhost:27017");
        }

        /// <summary>
        /// 添加一行
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public long Add(string tabName, List<string> values)
        {
            //string valuestr = string.Empty;
            //var sqlParams = new MySqlParameter[values.Count];
            //for(int i = 0; i < values.Count; i++)
            //{
            //    var strKey = "@value" + i;
            //    var strs = values[i].Split(':');
            //    var dbType = GetDbType(strs[0]);
            //    sqlParams[i] = new MySqlParameter(strKey, dbType) { Value = strs[1] };

            //    if (!string.IsNullOrEmpty(valuestr))
            //    {
            //        valuestr += ", ";
            //    }
            //    valuestr += strKey;
            //}
            //string strsql = "insert into " + tabName + " values (" + valuestr + ");";
            //return MysqlUtility.ExecuteSql(strsql, sqlParams);
            return 0;
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="uid"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string tabName, string uid, string key, string value)
        {
            //var strs = value.Split(':');
            //var dbType = GetDbType(strs[0]);
            //var sqlParams = new MySqlParameter[]
            //{
            //    new MySqlParameter("@value", dbType) { Value = strs[1] },
            //    new MySqlParameter("@openid", MySqlDbType.VarChar) { Value = uid },
            //};
            //string strKey = tabName + "_" + uid + "_" + key;
            ////RedisUtility.StringSet(strKey, strs[1], expireTime);

            //var strsql = "update " + tabName + " set " + key + "=@value where openid=@openid";
            //MysqlUtility.ExecuteSql(strsql, sqlParams);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="uid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string tabName, string uid, string key)
        {
            string strKey = tabName + "_" + uid + "_" + key;
            //if (RedisUtility.KeyExist(strKey))
            //{
            //    return RedisUtility.StringGet(strKey);
            //}
            //var sqlParams = new MySqlParameter[]
            //{
            //    new MySqlParameter("@openid", MySqlDbType.VarChar) { Value = uid },
            //};
            //var strsql = "select " + key + " from " + tabName + " where uid=@uid limit 1";
            //var dataset = MysqlUtility.ExecuteQuery(strsql, sqlParams);
            //var obj = dataset.Tables[0].Rows[0][key].ToString();
            //if (obj != null)
            //{
            //    logger.Warn("strKey:  " + strKey + " obj: " + obj);
            //    RedisUtility.StringSet(strKey, obj, expireTime);
            //}
            //return obj;
            return null;
        }

        /// <summary>
        /// 获取一行
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public DataRow GetRow(string tabName, List<string> values = null)
        {
            var dataset = Query(tabName, values, 1);
            if (dataset == null || dataset.Tables == null || dataset.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            return dataset.Tables[0].Rows[0];
        }

        /// <summary>
        /// 组合查询   Key:Type:Value    nick:str:jarjin
        /// </summary>
        /// <returns></returns>
        public DataSet Query(string tabName, List<string> values = null, int rowCount = 0)
        {
            //MySqlParameter[] sqlParams = null;
            //var limitStr = string.Empty;
            //if (rowCount > 0) {
            //    limitStr = " limit " + rowCount;
            //}
            //var strsql = "select * from " + tabName;
            //if (values != null)
            //{
            //    string valuestr = string.Empty;
            //    sqlParams = new MySqlParameter[values.Count];
            //    for (int i = 0; i < values.Count; i++)
            //    {
            //        var strs = values[i].Split(':');
            //        var valKey = "@value" + i;
            //        var dateType = GetDbType(strs[0]);
            //        var keyValues = strs[1].Split('=');
            //        sqlParams[i] = new MySqlParameter(valKey, dateType) { 
            //            Value = keyValues[1] 
            //        };
            //        if (!string.IsNullOrEmpty(valuestr))
            //        {
            //            valuestr += " and ";
            //        }
            //        valuestr += keyValues[0] + "=" + valKey;
            //    }
            //    strsql += " where " + valuestr;
            //}
            //strsql += limitStr;
            //DataSet dataset = null;
            //try
            //{
            //    //dataset = MysqlUtility.ExecuteQuery(strsql, sqlParams);
            //}
            //finally
            //{
            //    logger.Info("strsql:" + strsql);
            //}
            //return dataset;
            return null;
        }

        /// <summary>
        /// 存在一条记录
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool Exist(string tabName, string uid)
        {
            //var sqlParams = new MySqlParameter[]
            //{
            //    new MySqlParameter("@uid", MySqlDbType.VarChar) { Value = uid },
            //};
            //var strsql = "select * from " + tabName + " where uid=@uid";
            //DataSet dataset = null;
            //try
            //{
            //    //dataset = MysqlUtility.ExecuteQuery(strsql, sqlParams);
            //}
            //finally
            //{
            //    logger.Info("strsql:" + strsql);
            //}
            //return dataset;
            return false;
        }

        /// <summary>
        /// 移除某个字段
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="uid"></param>
        /// <param name="key"></param>
        public void Remove(string tabName, string uid, string key)
        {
            //if (RedisUtility.KeyExist(key))
            //{
            //    RedisUtility.KeyDelete(key);
            //}
            //var sqlParams = new MySqlParameter[]
            //{
            //    new MySqlParameter("@openid", MySqlDbType.VarChar) { Value = uid },
            //};
            //var strsql = "update " + tabName + " set "+ key + "='' where openid =@openid";
            //MysqlUtility.ExecuteSql(strsql, sqlParams);
        }

        /// <summary>
        /// 关闭链接
        /// </summary>
        public void Close()
        {
            //MysqlUtility.Close();
            //RedisUtility.Close();
        }

        public void OnDispose()
        {
            dataMgr = null;
        }
    }
}

using System;
using System.Collections.Generic;
using FirServer.Defines;
using FirServer.Utility;
using MySql.Data.MySqlClient;

namespace FirServer.Managers
{
     public class MySqlServer
    {
        #region Instance

        private MySqlServer() { }
        static readonly MySqlServer _instance = new MySqlServer();

        public static MySqlServer Instance()
        {
            return _instance;
        }

        #endregion

        string _connStr;

        /// <summary>
        /// 初始化服务器
        /// </summary>
        public void Initialize(DatabaseConfig config)
        {
            if (config != null)
                this._connStr = GetConnStr(config);
        }

        #region Get

        public object GetScalarResult(ZinSQL sql)
        {
            using (var conn = GetConnection(sql))
            {
                conn.Open();
                var list = conn.GetScalarResult(sql, sql.ParamList);
                conn.Close();

                return list;
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        public object[] Get(ZinSQL sql, List<MySqlParameter> paramList = null)
        {
            var list = GetList(sql, paramList ?? new List<MySqlParameter>());

            if (list.Count > 0)
                return list[0];

            return null;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        public List<object[]> GetList(ZinSQL sql, List<MySqlParameter> paramList = null)
        {
            using (var conn = GetConnection(sql))
            {
                conn.Open();
                var list = conn.Get(sql, paramList ?? new List<MySqlParameter>());
                conn.Close();

                return list;
            }
        }

        #endregion

        #region Insert

        /// <summary>
        /// 插入数据
        /// </summary>
        public void Insert(ZinSQL sql, List<MySqlParameter> paramList)
        {
#if NoneWriteSQL
            return;
#endif
            using (var conn = GetConnection(sql))
            {
                conn.Open();
                conn.Insert(sql, paramList);
                conn.Close();
            }
        }

        public void InsertOrUpdate(ZinSQL sql, List<MySqlParameter> paramList)
        {
#if NoneWriteSQL
            return;
#endif
            sql.With(paramList);
            BatchInsertOrUpdate(new List<ZinSQL>() { sql }, sql.Config);
        }

        /// <summary>
        /// 追加数据
        /// </summary>
        public void Append(ZinSQL sql, List<MySqlParameter> paramList)
        {
#if NoneWriteSQL
            return;
#endif
            using (var conn = GetConnection(sql))
            {
                conn.Open();
                if (conn.Get(sql, paramList).Count == 0)
                    conn.Insert(sql, paramList);
                else
                    conn.Append(sql, paramList);
                conn.Close();
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// 更新数据
        /// </summary>
        public void Update(ZinSQL sql, List<MySqlParameter> paramList)
        {
#if NoneWriteSQL
            return;
#endif

            using (var conn = GetConnection(sql))
            {
                conn.Open();
                conn.Update(sql, paramList);
                conn.Close();
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// 删除数据
        /// </summary>
        public void Delete(ZinSQL sql, List<MySqlParameter> paramList)
        {
#if NoneWriteSQL
            return;
#endif

            using (var conn = GetConnection(sql))
            {
                conn.Open();
                conn.Delete(sql, paramList);
                conn.Close();
            }
        }

        #endregion

        #region Count

        /// <summary>
        /// 计数
        /// </summary>
        public int Count(ZinSQL sql, List<MySqlParameter> paramList = null)
        {
            using (var conn = GetConnection(sql))
            {
                conn.Open();
                var count = conn.Count(sql, paramList ?? new List<MySqlParameter>());
                conn.Close();

                return count;
            }
        }

        #endregion

        #region Batch

        public void BatchInsert(List<ZinSQL> sqlList, DatabaseConfig config = null)
        {
#if NoneWriteSQL
            return;
#endif
            using (var conn = GetConnection(config))
            {
                conn.Open();

                foreach (var zinSQL in sqlList)
                    conn.Insert(zinSQL, zinSQL.ParamList);

                conn.Close();
            }
        }

        public void BatchInsertOrUpdate(List<ZinSQL> sqlList, DatabaseConfig config = null)
        {
#if NoneWriteSQL
            return;
#endif
            using (var conn = GetConnection(config))
            {
                conn.Open();

                foreach (var zinSQL in sqlList)
                {
                    if (conn.Get(zinSQL, zinSQL.ParamList).Count == 0)
                        conn.Insert(zinSQL, zinSQL.ParamList);
                    else
                        conn.Update(zinSQL, zinSQL.ParamList);
                }

                conn.Close();
            }
        }

        public void BatchDelete(List<ZinSQL> sqlList, DatabaseConfig config = null)
        {
#if NoneWriteSQL
            return;
#endif
            using (var conn = GetConnection(config))
            {
                conn.Open();

                foreach (var zinSQL in sqlList)
                    conn.Delete(zinSQL, zinSQL.ParamList);

                conn.Close();
            }
        }

        #endregion

        /// <summary>
        /// 直接执行SQL
        /// </summary>
        public object Execute(ZinSQL sql, List<MySqlParameter> paramList)
        {
            using (var conn = GetConnection(sql))
            {
                conn.Open();
                var obj = conn.Execute(sql, paramList);
                conn.Close();

                return obj;
            }
        }

        public void ExecuteNonQuery(ZinSQL sql)
        {
            using (var conn = GetConnection(sql))
            {
                conn.Open();
                conn.ExecuteNonQuery(sql);
                conn.Close();
            }
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        public List<string> GetTableName(DatabaseConfig config, string database)
        {
            var sql = new ZinSQL();
            sql.Config = config;
            sql.ExecuteSql = string.Format("SELECT table_name FROM information_schema.tables WHERE table_schema = '{0}'", database);

            using (var conn = GetConnection(sql))
            {
                conn.Open();
                var list = conn.Get(sql, new List<MySqlParameter>());
                conn.Close();

                var tableList = new List<string>();
                foreach (var data in list)
                    tableList.Add(data[0].ToString());

                return tableList;
            }
        }

        #region Save/Load DataList

        public List<string> LoadDataTypeList(DatabaseConfig config = null)
        {
            var typeList = new List<string>();

            // 生成SQL
            var sql = new ZinSQL("game_data");
            sql.AddField("DataType");
            sql.Config = config;

            var dataList = MySqlServer.Instance().GetList(sql);
            foreach (var data in dataList)
            {
                typeList.Add((string)data[0]);
            }
            return typeList;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public T LoadData<T>(string type, DatabaseConfig config = null)
            where T : new()
        {
            // 生成SQL
            var sql = new ZinSQL("game_data");
            sql.AddField("DataValue");
            sql.SetQuery("DataType", type);
            sql.Config = config;

            // 获取数据
            var data = Get(sql);
            if (data != null && data[0] != DBNull.Value)
            {
                var value = SerializationHelper.Deserialize<T>((byte[])data[0]);
                if (value != null)
                    return value;
            }

            return new T();
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public void SaveData<T>(string type, T data, DatabaseConfig config = null)
            where T : new()
        {
#if NoneWriteSQL
            return;
#endif
            try
            {
                var sql = new ZinSQL("game_data");
                var paramList = new List<MySqlParameter>();
                paramList.Add(sql.AddParam("DataValue", SerializationHelper.Serialize<T>(data)));
                paramList.Add(sql.AddParam("DataType", type));
                sql.SetQuery("DataType", type);
                sql.Config = config;

                InsertOrUpdate(sql, paramList);
            }
            catch (Exception ex)
            {
                // LogHelper.WriteErrorLog(ex);
                // LogHelper.WriteErrorLog(string.Format("Save Data {0} Error", type));
            }
        }

        public void ResetData(string type)
        {
            try
            {
                var sql = new ZinSQL("game_data");
                var paramList = new List<MySqlParameter>();
                sql.SetQuery("DataType", type);

                Delete(sql, paramList);
            }
            catch (Exception ex)
            {
                // LogHelper.WriteErrorLog(ex);
                // LogHelper.WriteErrorLog(string.Format("Delete Data {0} Error", type));
            }
        }

        #endregion

        #region Helpers

        public MySqlConnection GetConnection(ZinSQL sql)
        {
            return GetConnection(sql.Config);
        }

        public MySqlConnection GetConnection(DatabaseConfig config)
        {
            var connStr = (config != null) ? MySqlServer.Instance().GetConnStr(config) : _connStr;
            return new MySqlConnection(connStr);
        }

        public MySqlConnection GetConnection(string connStr = "")
        {
            return new MySqlConnection((!string.IsNullOrEmpty(connStr)) ? connStr : _connStr);
        }

        // connectDB: 是否需要连接指定数据库进行查询 - 在建库的时候, 为false
        public string GetConnStr(DatabaseConfig config, bool connectDB = true)
        {
            if (connectDB)
                return
                    $"server={config.IP};port={config.Port};user id={config.Username};password={config.Password};database={config.Database};charset={config.CharSet};SslMode={(config.DefaultCommandTimeout > 0 ? "default command timeout=" + config.DefaultCommandTimeout : "")};";
            else
                return
                    $"server={config.IP};port={config.Port};user id={config.Username};password={config.Password};charset={config.CharSet};SslMode={(config.DefaultCommandTimeout > 0 ? "default command timeout=" + config.DefaultCommandTimeout : "")};";
        }

        #endregion
    }
}
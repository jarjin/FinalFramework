using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Utility;

namespace FirServer.Managers
{
    public static class MySqlHelper
    {
        #region Get

        public static object GetScalarResult(this MySqlConnection conn, ZinSQL sql, List<MySqlParameter> paramList)
        {
            if (string.IsNullOrEmpty(sql.Where))
                return conn.ExecuteScalar(string.Format("select {1} from {0}", sql.TableName, sql.GetFields()), paramList);
            else
                return conn.ExecuteScalar(string.Format("select {1} from {0} where {2}", sql.TableName, sql.GetFields(), sql.Where), paramList);
        }

        /// <summary>
        /// 获取
        /// </summary>
        public static List<object[]> Get(this MySqlConnection conn, ZinSQL sql, List<MySqlParameter> paramList)
        {
            var sqlStr = string.Empty;

            if (string.IsNullOrEmpty(sql.Where))
                sqlStr = string.Format("select {1} from {0}", sql.TableName, sql.GetFields());
            else
                sqlStr = string.Format("select {1} from {0} where {2}", sql.TableName, sql.GetFields(), sql.Where);

            return conn.ExecuteQuery(sqlStr, paramList);
        }

        #endregion

        /// <summary>
        /// 判断是否存在
        /// </summary>
        public static bool Has(this MySqlConnection conn, ZinSQL sql)
        {
            var fields = sql.GetFields();
            var count = conn.ExecuteScalar(string.Format("select COUNT({1}) from {0} where {2} = {3}", sql.TableName, fields, fields, sql.GetValues()));

            return (Convert.ToInt32(count) > 0);
        }

        /// <summary>
        /// 插入信息
        /// </summary>
        public static void Insert(this MySqlConnection conn, ZinSQL sql, List<MySqlParameter> paramList)
        {
            conn.ExecuteNonQuery(string.Format("insert into {0}({1}) values({2})", sql.TableName, sql.GetFields(), sql.GetValues()), paramList);
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        public static void Update(this MySqlConnection conn, ZinSQL sql, List<MySqlParameter> paramList)
        {
            conn.ExecuteNonQuery(string.Format("update {0} set {1} where {2}", sql.TableName, sql.GetUpdateValues(), sql.Where), paramList);
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="whereStr">ID = 1</param>
        public static void Delete(this MySqlConnection conn, ZinSQL sql, List<MySqlParameter> paramList)
        {
            conn.ExecuteNonQuery(string.Format("delete from {0} where {1}", sql.TableName, sql.Where), paramList);
        }

        /// <summary>
        /// 追加信息
        /// </summary>
        public static void Append(this MySqlConnection conn, ZinSQL sql, List<MySqlParameter> paramList)
        {
            var field = sql.GetAppendFiled();
            var value = sql.GetAppendValue();
            var sqlStr = string.Format("update {0} set {1} = concat({2}, {3}) where {4}", sql.TableName, field, field, value, sql.Where);
            conn.ExecuteNonQuery(sqlStr, paramList);
        }

        /// <summary>
        /// 计数
        /// </summary>
        public static int Count(this MySqlConnection conn, ZinSQL sql, List<MySqlParameter> paramList)
        {
            var sqlStr = string.Empty;

            if (string.IsNullOrEmpty(sql.Where))
                sqlStr = string.Format("select count({1}) from {0}", sql.TableName, sql.GetFields());
            else
                sqlStr = string.Format("select count({1}) from {0} where {2}", sql.TableName, sql.GetFields(), sql.Where);

            return conn.ExecuteScalar(sqlStr, paramList).ToInt();
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        public static object Execute(this MySqlConnection conn, ZinSQL sql, List<MySqlParameter> paramList)
        {
            return conn.ExecuteScalar(string.Format(sql.ExecuteSql, sql.TableName), paramList);
        }

        public static void ExecuteNonQuery(this MySqlConnection conn, ZinSQL sql)
        {
            conn.ExecuteNonQuery(sql.ExecuteSql, sql.ParamList);
        }

        #region Helpers

        public static MySqlParameter AddParam(this ZinSQL sql, string field, object value)
        {
            sql.AddParam(field);
            return new MySqlParameter(string.Format("@{0}", field), value);
        }

        public static MySqlParameter AddQueryParam(this ZinSQL sql, string field, object value, bool addParam = false, string op = "=")
        {
            sql.SetParamQuery(field, op);

            if (addParam)
                sql.AddParam(field);

            return new MySqlParameter(string.Format("@{0}", field), value);
        }

        /// <summary>
        /// 应对出现where条件参数也是要被修改参数
        /// </summary>
        public static MySqlParameter AddKeyQueryParam(this ZinSQL sql, string field, object value)
        {
            sql.SetKeyQuery(field);
            return new MySqlParameter(string.Format("@{0}1", field), value);
        }

        public static List<MySqlParameter> AddMultiQueryParam<T>(this ZinSQL sql, string field, List<T> values)
        {
            sql.SetMultiQuery(field, values.Count);

            var list = new List<MySqlParameter>();

            for (int i = 0, n = values.Count; i < n; i++)
                list.Add(new MySqlParameter(string.Format("@{0}{1}", field, i), values[i]));

            return list;
        }

        private static List<object[]> ExecuteQuery(this MySqlConnection conn, string sql, List<MySqlParameter> paramList)
        {
            using (var cmd = new MySqlCommand(sql, conn))
            {
                foreach (var param in paramList)
                    cmd.Parameters.Add(param);

                var list = new List<object[]>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var values = new object[reader.FieldCount];
                        reader.GetValues(values);
                        list.Add(values);
                    }

                    reader.Close();
                }

                return list;
            }
        }

        private static void ExecuteNonQuery(this MySqlConnection conn, string sql, List<MySqlParameter> paramList)
        {
            using (var cmd = new MySqlCommand(sql, conn))
            {
                foreach (var param in paramList)
                    cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();
            }
        }

        private static object ExecuteScalar(this MySqlConnection conn, string sql, List<MySqlParameter> paramList = null)
        {
            using (var cmd = new MySqlCommand(sql, conn))
            {
                if (paramList != null)
                {
                    foreach (var param in paramList)
                        cmd.Parameters.Add(param);
                }

                return cmd.ExecuteScalar();
            }
        }

        #endregion
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirServer.Defines;
using MySql.Data.MySqlClient;
using Utility;

namespace FirServer.Managers
{
   public class ZinSQL
    {
        #region Members

        string _tableName;
        List<string> fields;
        List<string> values;
        string _where;
        List<MySqlParameter> paramList;

        #endregion

        public ZinSQL(string tableName = "")
        {
            _tableName = tableName;
            fields = new List<string>();
            values = new List<string>();
            paramList = new List<MySqlParameter>();
        }

        /// <summary>
        /// 连接串（用于连接其他MySQL）
        /// </summary>
        public DatabaseConfig Config
        {
            get;
            set;
        }

        public string TableName
        {
            get { return _tableName; }
        }

        public string Where
        {
            get { return _where; }
        }

        public string ExecuteSql
        {
            get;
            set;
        }

        /// <summary>
        /// 添加值
        /// </summary>
        public void AddField(string fieldName)
        {
            fields.Add(fieldName);
        }

        public void AddParam(string fieldName)
        {
            fields.Add(fieldName);
            values.Add(string.Format("@{0}", fieldName));
        }

        public void Add(string fieldName, string value)
        {
            fields.Add(fieldName);
            values.Add(string.Format("'{0}'", value));
        }

        public void Add(string fieldName, int value)
        {
            fields.Add(fieldName);
            values.Add(value.ToString());
        }

        public void SetParamQuery(string field, string op = "=")
        {
            CombineQuery(string.Format("{0} {1} @{2}", field, op, field));
        }

        /// <summary>
        /// 应对出现where条件参数也是要被修改参数
        /// </summary>
        public void SetKeyQuery(string field)
        {
            CombineQuery(string.Format("{0} = @{1}1", field, field));
        }

        public void SetQuery(string field)
        {
            CombineQuery(string.Format("{0} = @{1}", field, field));
        }

        public void SetQuery(string field, string value)
        {
            CombineQuery(string.Format("{0} = '{1}'", field, value));
        }

        public void SetQuery(string field, int value, string op = "=")
        {
            CombineQuery(string.Format("{0} {1} {2}", field, op, value));
        }

        public void SetMultiQuery(string field, string[] values)
        {
            CombineQuery(string.Format("{0} in ({1})", field, string.Join(",", values)));
        }

        public void SetMultiQuery(string field, int count)
        {
            var values = new string[count];

            for (var i = 0; i < count; i++)
            {
                values[i] = string.Format("@{0}{1}", field, i);
            }

            CombineQuery(string.Format("{0} in ({1})", field, string.Join(",", values)));
        }

        private void CombineQuery(string query)
        {
            if (string.IsNullOrEmpty(_where))
                _where = query;
            else
                _where = string.Format("{0} AND {1}", _where, query);
        }

        /// <summary>
        /// 附带参数列表
        /// </summary>
        public ZinSQL With(List<MySqlParameter> sqlParamList)
        {
            this.paramList = sqlParamList;
            return this;
        }

        public List<MySqlParameter> ParamList
        {
            get { return this.paramList; }
        }

        /// <summary>
        /// 获取所有字段
        /// </summary>
        public string GetFields()
        {
            return string.Join(",", this.fields.ToArray());
        }

        /// <summary>
        /// 获取所有值
        /// </summary>
        public string GetValues()
        {
            return string.Join(",", this.values.ToArray());
        }

        /// <summary>
        /// 获取更新值
        /// </summary>
        public string GetUpdateValues()
        {
            var _sb = new StringBuilder();
            var fieldCount = fields.Count;

            for (var i = 0; i < fieldCount; i++)
            {
                _sb.Append(fields[i]).Append("=").Append(values[i]).Append(",");
            }

            return _sb.Cut();
        }

        public string GetAppendFiled()
        {
            return this.fields.LastOrDefault();
        }

        public string GetAppendValue()
        {
            return this.values.LastOrDefault();
        }
    }
}
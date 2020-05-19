
using System;
using System.Collections.Generic;
using System.Data;
using log4net;
using Utility;

namespace FirServer.Models
{
    public class UserModel : BaseModel
    {

        public UserModel() : base("user")
        {
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public long AddUser(List<string> values)
        {
            var uid = AppUtil.NewGuidId();
            values.Insert(0, "long:" + uid);
            if (base.Add(values) == 0)
            {
                uid = 0L;
            }
            return uid;
        }

        /// <summary>
        /// 用户是否存在
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public long ExistUser(List<string> values = null)
        {
            var row = GetRow(values);
            if (row == null)
            {
                return 0L;
            }
            var uid = Convert.ToInt64(row["uid"]);
            return uid;
        }

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public DataRowCollection GetAll(List<string> values = null, int rowCount = 0)
        {
            var dataset = Query(values, rowCount);
            if (dataset == null || dataset.Tables == null || dataset.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            return dataset.Tables[0].Rows;
        }

        public string GetUserName(string uid)
        {
            return base.Get(uid, "username");
        }

        public void SetUserName(string uid, string value)
        {
            base.Set(uid, "username", "str:" + value);
        }

        public long GetMoney(string uid)
        {
            return long.Parse(base.Get(uid, "money"));
        }

        public void SetMoney(string uid, long value)
        {
            base.Set(uid, "money", "str:" + value);
        }

        public int GetCount(string uid)
        {
            return int.Parse(base.Get(uid, "count"));
        }

        public void SetCount(string uid, int value)
        {
            base.Set(uid, "count", "int:" + value);
        }

        public string GetLastTime(string uid)
        {
            return base.Get(uid, "lasttime");
        }

        public void SetLastTime(string uid, string value)
        {
            base.Set(uid, "lasttime", "str:" + value);
        }
    }
}

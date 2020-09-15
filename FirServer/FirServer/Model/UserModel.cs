using FirServer.Define;
using FirServer.Utility;
using System.Collections.Generic;
using Utility;

namespace FirServer.Model
{
    public class UserModel : BaseModel
    {

        public UserModel() : base("user")
        {
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        public long AddUser(Dictionary<string, object> values)
        {
            var uid = AppUtil.NewGuidId();
            values.Add("uid", uid);
            if (!base.Add(values))
            {
                uid = 0L;
            }
            return uid;
        }

        /// <summary>
        /// 用户是否存在
        /// </summary>
        public long ExistUser(string username, string password)
        {
            var row = GetDoc<UserInfo>(u => u.username == username && u.password == password);
            if (row == null)
            {
                return 0L;
            }
            return row["uid"].AsInt64;
        }

        public string GetUserName(long uid)
        {
            var result = Get<UserInfo>("username", u => u.uid == uid);
            if (result != null)
            {
                return result.AsString;
            }
            return null;
        }

        public void SetUserName(long uid, string value)
        {
            var filter = Builder.Update<UserInfo>("username", value);
            Set<UserInfo>(filter, u => u.uid == uid);
        }

        public long GetMoney(long uid)
        {
            var result = Get<UserInfo>("money", u => u.uid == uid);
            if (result != null)
            {
                return result.AsInt64;
            }
            return 0;
        }

        public void SetMoney(long uid, long value)
        {
            var filter = Builder.Update<UserInfo>("money", value);
            Set<UserInfo>(filter, u => u.uid == uid);
        }

        public int GetCount(long uid)
        {
            var result = Get<UserInfo>("count", u => u.uid == uid);
            if (result != null)
            {
                return result.AsInt32;
            }
            return 0;
        }

        public void SetCount(long uid, int value)
        {
            var filter = Builder.Update<UserInfo>("count", value);
            Set<UserInfo>(filter, u => u.uid == uid);
        }

        public string GetLastTime(long uid)
        {
            var result = Get<UserInfo>("lasttime", u => u.uid == uid);
            if (result != null)
            {
                return result.AsString;
            }
            return null;
        }

        public void SetLastTime(long uid, string value)
        {
            var filter = Builder.Update<UserInfo>("lasttime", value);
            Set<UserInfo>(filter, u => u.uid == uid);
        }
    }
}

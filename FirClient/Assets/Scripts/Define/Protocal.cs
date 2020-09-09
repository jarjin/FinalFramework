
namespace FirClient.Define
{
    public enum ProtoType : byte
    {
        CSProtoMsg = 0,
        LuaProtoMsg = 1,     
    }

    /// <summary>
    /// 结果码
    /// </summary>
    public enum ResultCode : ushort
    {
        Success = 0,             //操作成功
        Failed = 1,              //操作失败
        ExistUser = 2,           //用户已注册
    }

    public static class Protocal
    {
        public const string Disconnect = "Disconnect";             //异常掉线
        public const string Register = "Register";                 //注册账号
        public const string Login = "Login";                       //用户登录
        public const string Logout = "Logout";                     //退出游戏
        public const string ReqUserInfo = "ReqUserInfo";           //用户信息
    }
}

namespace FirServer.Define
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

    /// <summary>
    /// 网络协议文件
    /// </summary>
    static class Protocal
    {
        public const string Default = "Default";                 //缺省消息
        public const string Disconnect = "Disconnect";              //异常掉线
    }
}
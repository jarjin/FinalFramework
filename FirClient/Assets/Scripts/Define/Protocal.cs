
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
        public const string Default = "Default";             //异常掉线
    }
}
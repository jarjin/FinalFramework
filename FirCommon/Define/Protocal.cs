namespace FirCommon.Define
{
    public enum ProtoType : byte
    {
        CSProtoMsg = 0,
        LuaProtoMsg = 1,
    }

    public static class Protocal
    {
        public const string Default = "Default";                                        //缺省消息
        public const string Connected = "Connected";                                    //链接完成
        public const string Disconnect = "Disconnect";                                  //异常掉线

        public const string ReqLogin = "pb_user.ReqLogin";                      //请求用户登录
        public const string ResLogin = "pb_user.ResLogin";                      //返回用户登录
    }
}

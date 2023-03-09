using FirCommon.Define;
using FirServer.Define;
using FirServer.Handler;
using Google.Protobuf;
using msg1;

namespace GameLibs.Handler
{
    internal class LoginHandler : BaseHandler
    {
        public override async Task OnMessage(MsgChannel channel, ByteString bytes)
        {
            var reqMsg = ReqMsg.Parser.ParseFrom(bytes);
            var resMsg = new ResMsg { Message = "HelloLogin!!!" };
            await SendMessage(channel, Protocal.ResLogin, resMsg);
        }
    }
}

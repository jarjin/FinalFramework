using FirServer.Define;
using Google.Protobuf;

namespace FirServer.Interface
{
    public interface IHandler
    {
        Task OnMessage(MsgChannel channel, ByteString bytes);
    }
}

using FirClient.Define;
using LiteNetLib.Utils;

namespace FirClient.Network
{
    public class PacketData
    {
        public ProtoType protocal;
        public NetDataWriter writer;

        public PacketData() { }

        public void Reset()
        {
            writer = null;
            protocal = ProtoType.CSProtoMsg;
        }
    }
}


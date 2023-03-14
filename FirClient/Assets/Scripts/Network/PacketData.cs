using FirClient.Define;

namespace FirClient.Network
{
    public class PacketData
    {
        public ProtoType protocal;

        public PacketData() { }

        public void Reset()
        {
            protocal = ProtoType.CSProtoMsg;
        }
    }
}


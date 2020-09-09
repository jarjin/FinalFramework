using System;
using LiteNetLib;
using LiteNetLib.Utils;

public interface IHandler
{
    void OnMessage(NetPeer peer, NetDataReader reader);
}
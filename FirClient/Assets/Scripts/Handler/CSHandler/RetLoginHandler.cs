using Tutorial;
using UnityEngine;
using Google.Protobuf;
using System.IO;

namespace FirClient.Handler
{
    public class RetLoginHandler : BaseHandler
    {
        public override void OnMessage(byte[] bytes)
        {
            //解包读取
            var person = Person.Parser.ParseFrom(bytes);
            GLogger.Log(person.Name);

            ///封包发送
            using (MemoryStream stream = new MemoryStream())
            {
                // Save the person to a stream
                person.WriteTo(stream);
                bytes = stream.ToArray();
                networkMgr.SendData("Person", bytes);
            }
        }
    }
}
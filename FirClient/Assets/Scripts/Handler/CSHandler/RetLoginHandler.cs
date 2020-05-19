
using tutorial;
using UnityEngine;

namespace FirClient.Handler
{
    public class RetLoginHandler : BaseHandler
    {
        public override void OnMessage(byte[] bytes)
        {
            //解包读取
            var person = DeSerialize<Person>(bytes);
            GLogger.Log(person.name);

            ///封包发送
            networkMgr.SendData<Person>("Person", person);
        }
    }
}
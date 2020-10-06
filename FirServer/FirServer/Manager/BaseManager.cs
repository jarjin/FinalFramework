using FirServer.Interface;
using System;

namespace FirServer.Manager
{
    public class BaseManager : BaseBehaviour, IManager
    {
        public virtual void Initialize()
        {
            throw new NotImplementedException();
        }

        public virtual void OnDispose()
        {
            throw new NotImplementedException();
        }
    }
}

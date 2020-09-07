using System;
using System.Collections.Generic;
using System.Threading;

namespace FirServer.Managers
{
    public interface ITaskModule
    {
        Guid ThreadKey
        {
            get;
            set;
        }

        List<Thread> ThreadList
        {
            get;
            set;
        }
    }
}
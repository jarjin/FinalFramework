using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirServer.Interface
{
    public interface IManager : IObject
    {
        void Initialize();
        void OnDispose();
    }
}

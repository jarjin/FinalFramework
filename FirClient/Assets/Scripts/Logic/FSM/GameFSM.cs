using FirClient.Component.FSM;
using FirClient.Logic.Manager;

namespace FirClient.Logic.FSM
{
    public class GameFSM : BaseFSM
    {
        protected static bool bRunning = false;
        protected static readonly object mLock = new object();

        protected static NPCDataManager npcDataMgr
        {
            get
            {
                return LogicBehaviour.npcDataMgr;
            }
        }
    }
}


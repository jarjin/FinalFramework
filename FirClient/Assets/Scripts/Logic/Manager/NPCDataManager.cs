using FirClient.Data;
using System.Linq;
using System.Collections.Generic;

namespace FirClient.Logic.Manager
{
    public class NPCDataManager : LogicBehaviour
    {
        private uint npcIndex = 0;
        private Dictionary<long, NPCData> mNpcDatas = new Dictionary<long, NPCData>();
        private Dictionary<NpcType, List<NPCData>> cacheNpcDatas = new Dictionary<NpcType, List<NPCData>>();

        public Dictionary<long, NPCData> NpcDatas
        {
            get { return mNpcDatas; } 
        }

        public override void Initialize()
        {
        }

        public uint MakeNpcId()
        {
            return ++npcIndex;
        }

        public void AddNpcData(NPCData data)
        {
            if (data == null) return;
            if (!mNpcDatas.ContainsKey(data.npcid))
            {
                mNpcDatas.Add(data.npcid, data);
            }
        }

        public NPCData GetNpcData(long npcid)
        {
            NPCData npcData = null;
            mNpcDatas.TryGetValue(npcid, out npcData);
            return npcData;
        }

        internal List<NPCData> GetNpcDatas(NpcType npcType)
        {
            var query = from m in mNpcDatas.Values
                        where m.npcType == npcType
                        select m;
            return query.ToList<NPCData>();
        }

        public void RemoveNpcData(long npcid)
        {
            mNpcDatas.Remove(npcid);
        }

        public void ClearNpcData()
        {
            mNpcDatas.Clear();
        }

        public void ClearNpcData(NpcType npcType)
        {
            var array = GetNpcDatas(npcType);
            if (array != null && array.Count > 0)
            {
                foreach(var de in array)
                {
                    mNpcDatas.Remove(de.npcid);
                }
            }
        }

        public NPCData NewNpcData(uint roleid, NpcType npcType)
        {
            var npcid = MakeNpcId();
            var npcData = new NPCData(npcid);
            npcData.roleid = roleid;
            npcData.npcType = npcType;

            var roleData = configMgr.GetRoleData(roleid);
            npcData.jobType = roleData.job;
            return npcData;
        }

        public bool IsAllNpcStateOK(NpcType type, NpcState state)
        {
            var Npcs = GetNpcDatas(type);
            foreach (var de in Npcs)
            {
                if (de.npcState == state)
                {
                    return false;
                }
                if (state == NpcState.Move && de.npcState != NpcState.Ready)
                {
                    return false;
                }
            }
            return true;
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            foreach(var de in mNpcDatas)
            {
                if (de.Value.fsm != null)
                {
                    de.Value.fsm.OnUpdate(deltaTime);
                }
            }
        }

        public override void OnDispose()
        {
        }
    }
}


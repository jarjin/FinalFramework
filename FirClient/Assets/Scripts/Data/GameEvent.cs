using System;
using UnityEngine;

namespace FirClient.Data
{
    public enum GameEventType : byte
    {
        NpcSpawn,
        NpcMove,
        NpcShow,
        Battle,
        EnterDungeon,
        BattleEvent,
        EnterScene,
    }

    public class GameEventData
    {
        private static long evIndex = 0;

        public long eventId;
        public GameEventType evType;
        public GameEventBase evParam;

        long MakeEventId()
        {
            return ++evIndex;
        }
        public GameEventData(GameEventType type, GameEventBase gameEvent)
        {
            this.eventId = MakeEventId();
            this.evType = type;
            this.evParam = gameEvent;
        }
    }

    public class GameEventBase 
    {
        public Action<object> callback;
    }

    /// <summary>
    /// 进入副本事件
    /// </summary>
    public class EnterDungeonEvent : GameEventBase
    {
        public uint chapterid;
        public uint dungeonid;
        public Action action;
    }

    public class NpcSpawnEvent : GameEventBase
    {
        public bool isShowHUD;
        public NPCData npcData;
    }

    /// <summary>
    /// 攻击NPC事件
    /// </summary>
    public class NpcSkillAttackEvent : GameEventBase
    {
        public long attackerid;
        public long defenderid;
        public long currHp;
        public long maxHp;
        public int amount;
        public bool bMoveToTarget;
        public bool bPlaySound;
        public bool bUseSkill;
    }

    public class NpcMoveEvent : GameEventBase
    {
        public long npcid;
        public Vector3 movePos;
        public float moveTime;
    }

    public class NpcShowEvent : GameEventBase
    {
        public long npcid;
        public float showTime;
    }

    public class BattleStartEvent : GameEventBase
    {
        public BattleType type;
    }

    public class BattleEndEvent : GameEventBase
    {
        public bool result;
        public BattleType type;
    }
}
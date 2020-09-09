using FirClient.Logic.FSM;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FirClient.Data
{
    public enum NpcType : byte
    {
        Hero = 0,
        Enemy = 1,
    }

    public enum FaceDir : byte
    {
        Left = 1,
        Right = 2,
    }

    public enum SceneType : byte
    {
        BigScene,
        Dungeon,
    }

    public enum ItemType : int
    {
        Head = 0,       //头像图标
        Item = 1,       //道具图标
        Skill = 2,      //技能图标
    }

    public enum LevelType : int
    {
        Init = 0,
        Login = 1,
        Loader = 2,
        Main = 3,
        Battle = 4,
    }

    public enum BattleType : byte
    {
        TurnBase,       //回合制
        FreeBattle,     //自由战斗
    }

    public enum JobType : byte
    {
        Warrior = 1,    //战士
        Mage = 2,       //魔法师
        Archer = 3,     //弓箭手
    }

    public enum NpcState : byte
    {
        Idle = 0,
        Search = 1,
        Move = 2,
        LockTarget = 3,
        Attack = 4,
        Death = 5,
        Ready = 6
    }

    public class NPCDataBase
    {
        public long hp = 0;                 //血值
        public long hpMax = 0;              //最大血值
        public uint hpInc = 0;              //自增血量
        public long mp = 0;                 //魔法值
        public long mpMax = 0;              //最大魔法值
        public uint mpInc = 0;              //自增魔法值
        public long attack = 0;             //攻击力
        public long defense = 0;            //防御力
        public long skillConsume = 0;       //技能消耗魔法值
        public long exp = 0;                //经验值
    }

    public class NPCData : NPCDataBase
    {
        public NPCData(long npcid)
        {
            this.npcid = npcid;
        }
        public long npcid = 0;
        public uint roleid = 0;
        public uint index = 0;
        public NpcType npcType;
        public Vector3 position;
        public FaceDir faceDir;
        public NpcState npcState;
        public JobType jobType;
        public ushort level = 0;            //等级
        public NpcFSM fsm = null;          //角色状态机

        public override string ToString()
        {
            return string.Format("npcid:{0} npcType:{1} npcState:{2} fsm:{3}", npcid, npcType, npcState, fsm);
        }
    }

    public enum FrameActionType
    {
        Active,     //主动行为
        Passive,    //被动行为
    }

    public class FrameActionData
    {
        public long currHp;
        public long maxHp;
        public int amount;
        public bool bPlaySound;
        public FrameActionType type;
        public SkillParamData skillParams;
        public string animClipName;
        public object target;
    }

    public enum SkillFrameType : byte
    {
        Anim,
        Effect,
        Bullet,
        Sound,
        Health,
        DamageNum,
        BeAttack,
    }

    public class SkillParamData
    {
        public uint bulletId = 0;
        public uint effectId = 0;
        public uint beAttackEffectId = 0;
    }

    public class SkillData
    {
        public string name;
        public SkillParamData skillParams;
        public Dictionary<uint, uint> frameDatas;
    }

    public class RoleData
    {
        public uint id;
        public string name;
        public string nick;
        public Vector3 scale;
        public JobType job;
        public string desc;
        public string[] clips;
        public Dictionary<string, SkillData> skills;
    }

    public class BulletData
    {
        public string name;
        public string resource;
        public string animName;
        public Vector3 scale;
        public string sound;
    }

    public enum EffectType : byte
    {
        Sprite = 1,     //序列帧特效
        Flash = 2,      //FLASH特效
    }

    public class EffectData
    {
        public string name;
        public EffectType type;
        public string resource;
        public string animName;
        public Vector3 scale;
        public string sound;
    }

    [Serializable]
    public class EventData
    {
        public EventsType type;
        public string value;
    }

    public class SceneEvent
    {
        public string name;
        public Vector2? pos;
        public List<EventData> eventObjs;
    }

    public class MapData
    {
        public uint id;
        public uint type;
        public uint eventid;
        public string atlas;
        public string sound;
        public List<SceneEvent> events;
    }

    public enum EventsType : byte
    {
        None = 0,
        SpawnHero = 1,          //出生英雄
        SpawnEnemy = 2,         //出生敌人
        EnterDungeon = 3,       //副本事件
        ShowDialog = 4,         //对话事件
        MoveCamera = 5,         //移动相机
        MoveNpc = 6,            //移动NPC
        LoadScene = 7,          //进入场景
        StartFight = 8,         //战斗开始
    }

    public enum MoveObjectType : byte
    {
        CurrentPos = 1,
        NextPos = 2,
        SpecifiedPos = 3,
    }

    public class TeamNpcData : NPCDataBase
    {
        public uint roleid;
        public long money;
        public List<uint> drops;

        public override string ToString()
        {
            return string.Format("id={0} hp={1} attack={2} defense={3} exp={4} money={5}",
                                roleid, hp, attack, defense, exp, money);
        }
    }

    public class TeamData
    {
        public uint id;
        public List<TeamNpcData> teamNpcs;
    }

    public class ChapterData
    {
        public uint id;
        public string name;
        public Dictionary<uint, DungeonData> dungeonDatas;
    }

    public class DungeonData
    {
        public uint id;
        public string name;
        public string atlas;
        public uint star;
        public uint eventid;
        public List<uint> drop;
        public List<SceneEvent> events;
    }

    public enum EmbattleType : byte
    {
        Left = 1,
        Right = 2,
        Center = 3,
        BothSides = 4,
    }

    public class EmbattlePos
    {
        public uint id;
        public Vector3 pos;
        public FaceDir faceDir;
        public bool isUsing = false;
    }

    public class DialogData
    {
        public uint id;
        public uint roleid;
        public uint posid;
        public string txtContent;
    }

    public class PageData
    {
        public uint id;
        public string title;
        public Dictionary<uint, DialogData> dialogDatas;
    }

    public class StoryData
    {
        public uint id;
        public string name;
        public Dictionary<uint, PageData> pageDatas;
    }
}

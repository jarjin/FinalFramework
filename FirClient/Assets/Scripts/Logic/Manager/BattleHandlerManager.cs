using FirClient.Data;
using FirClient.Extensions;
using FirClient.Logic.FSM;
using FirClient.Logic.Handler;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FirClient.Logic.Manager
{
    public class BattleHandlerManager : LogicBehaviour
    {
        private Action execOK;
        private Vector2 currPos;
        private Dictionary<BattleType, BaseBattleHandler> battleHandlers = new Dictionary<BattleType, BaseBattleHandler>();

        public BaseBattleHandler CurrHandler
        {
            get { return battleHandlers[LogicConst.BattleType]; }
        }

        public override void Initialize()
        {
            battleHandlers.Add(BattleType.TurnBase, new TurnBaseBattleHandler());
            battleHandlers.Add(BattleType.FreeBattle, new FreeBattleHandler());
        }

        public void Initialize(Vector2 pos)
        {
            currPos = pos;
            GLogger.Yellow("Initialize Battle Pos====================:>>" + pos);
            embattlePosMgr.Initialize(pos);
        }

        /// <summary>
        /// 添加测试数据
        /// </summary>
        private void AddTestData()
        {
            uint[] roleids = { 11000 };
            npcDataMgr.ClearNpcData();
            for(int i = 0; i < roleids.Length; i++)
            {
                var npcData = npcDataMgr.NewNpcData(roleids[i], NpcType.Hero);
                npcData.position = currPos;
                npcData.hp = npcData.hpMax = 100;
                npcData.mp = LogicUtil.Random(0, 10);
                npcData.mpMax = 100;
                npcData.attack = 10;
                npcData.defense = 10;
                npcData.mpInc = 10u;
                npcData.skillConsume = 0;
                npcDataMgr.AddNpcData(npcData);
            }
        }

        /// <summary>
        /// 根据服务器数据，生成英雄
        /// </summary>
        public void SpawnHeros(Action execOK)
        {
            AddTestData();
            var index = 0u;
            var npcDatas = npcDataMgr.NpcDatas;
            var emType = LogicConst.BattleType == BattleType.FreeBattle ? EmbattleType.Center : EmbattleType.Left;

            foreach (var de in npcDatas)
            {
                if (de.Value.npcType == NpcType.Hero)
                {
                    var item = embattlePosMgr.GetItem(emType, ++index);
                    if (item != null)
                    {
                        de.Value.index = index;
                        de.Value.faceDir = item.faceDir;
                        de.Value.position = item.pos;
                        de.Value.fsm = new NpcFSM();
                        de.Value.fsm.Initialize(de.Value.npcid);
                        battleLogicMgr.NpcSpawn(de.Value, OnNpcSpawnOK);
                    }
                }
            }
            if (execOK != null) execOK();
        }

        internal void OnNpcSpawnOK(object param)
        {
            var npcId = param.ToLong();
            Debug.Log("OnNpcSpawnOK:>" + npcId);
            var npcData = npcDataMgr.GetNpcData(npcId);
            if (npcData != null)
            {
                var showTime = LogicUtil.Random(0.2f, 0.5f);
                if (LogicConst.BattleType == BattleType.FreeBattle && npcData.npcType == NpcType.Enemy)
                {
                    showTime = 0;
                }
                battleLogicMgr.NpcShow(npcId, showTime, delegate (object obj)
                {
                    var currData = npcDataMgr.GetNpcData(obj.ToLong());
                    if (npcData != null)
                    {
                        npcData.npcState = NpcState.Ready;
                    }
                    Debug.Log("OnNpcShowOK:>" + obj);
                });
                battleLogicMgr.SetNpcFaceDir(npcId, npcData.faceDir);
            }
        }

        /// <summary>
        /// 初始化NPC
        /// </summary>
        public void InitNpcTeams(List<string> teamDatas, Action execOK)
        {
            CurrHandler?.InitNpcTeams(teamDatas);
            if (execOK != null) execOK();
        }

        /// <summary>
        /// 开始战斗
        /// </summary>
        public void StartFight(Action execOK)
        {
            this.execOK = execOK;
            MoveNextTurn();
            battleLogicMgr.BattleStart();
            CurrHandler?.StartFight();
        }



        /// <summary>
        /// 下一波敌人
        /// </summary>
        public void MoveNextTurn()
        {
            var teamCount = CurrHandler?.GetTeamCount();
            GLogger.Gray("MoveNextTurn--------->>>" + teamCount);
            if (teamCount == 0)
            {
                if (npcDataMgr.IsAllNpcStateOK(NpcType.Hero, NpcState.Attack))
                {
                    timerMgr.AddTimer(0.5f, 0, (obj) =>
                    {
                        if (execOK != null) execOK();
                    });
                }
            }
            else
            {
                this.SpawnNpcTeam();
                CurrHandler?.MoveNextTurn();
            }
        }

        /// <summary>
        /// 出生一组NPC
        /// </summary>
        private void SpawnNpcTeam()
        {
            var currTeam = CurrHandler?.NextTeamData();
            if (currTeam != null)
            {
                List<uint> randomPos = null;
                var emType = LogicConst.BattleType == BattleType.FreeBattle ? EmbattleType.BothSides : EmbattleType.Right;
                if (emType == EmbattleType.BothSides)
                {
                    randomPos = new List<uint>();
                    while (randomPos.Count < currTeam.teamNpcs.Count)
                    {
                        var newindex = LogicUtil.Random(0u, 4u);
                        if (!randomPos.Contains(newindex))
                        {
                            randomPos.Add(newindex);
                        }
                    }
                }
                for (var i = 0; i < currTeam.teamNpcs.Count; i++)
                {
                    var newindex = 0;
                    var teamNpc = currTeam.teamNpcs[i];
                    if (emType == EmbattleType.BothSides)
                    {
                        newindex = (int)randomPos[i] + 1;
                    }
                    else
                    {
                        newindex = i + 1;
                    }
                    var item = embattlePosMgr.GetItem(emType, (uint)newindex);
                    if (item != null)
                    {
                        var npcData = npcDataMgr.NewNpcData(teamNpc.roleid, NpcType.Enemy);
                        npcData.index = (uint)i;
                        npcData.hp = teamNpc.hp;
                        npcData.hpInc = teamNpc.hpInc;
                        npcData.hpMax = teamNpc.hpMax;

                        npcData.mp = teamNpc.mp;
                        npcData.mpInc = teamNpc.mpInc;
                        npcData.mpMax = teamNpc.mpMax;

                        npcData.attack = teamNpc.attack;
                        npcData.defense = teamNpc.defense;
                        npcData.faceDir = item.faceDir;
                        npcData.position = item.pos;
                        npcDataMgr.AddNpcData(npcData);

                        npcData.fsm = new NpcFSM();
                        npcData.fsm.Initialize(npcData.npcid);
                        battleLogicMgr.NpcSpawn(npcData, OnNpcSpawnOK);
                    }
                }
            }
        }
    }
}


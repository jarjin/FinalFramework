using System;
using DG.Tweening;
using FirClient.Component;
using FirClient.HUD;
using UnityEngine;
using FTRuntime;
using FirClient.Data;
using FirClient.Utility;
using FirClient.Extensions;
using LuaInterface;

namespace FirClient.View
{
    public class RoleView : NPCView
    {
        private CSwf swf;
        private int oldSortingOrder = 0;
        private Vector3 oldPos;
        private RoleData roleData;
        private HUDObject mhudObj;
        private SwfClip swfClip;
        private MeshRenderer render;
        private bool bRunning = false;
        private Tweener moveTweener = null;

        [NoToLua]
        public override void Initialize(NPCData npcData, Vector3 pos, Action initOK = null) 
        {
            base.Initialize(npcData, pos, initOK);
            roleData = configMgr.GetRoleData(npcData.roleid);

            var id = npcData.roleid.ToString();
            base.CreateNpcObject(id, pos, roleData.scale, delegate(GameObject prefab) 
            {
                swf = roleObject.GetComponent<CSwf>();
                swf.onStopPlayingEvent += OnPlayingClipOK;

                render = roleObject.GetComponent<MeshRenderer>();

                swfClip = roleObject.GetComponent<SwfClip>();

                gameObject.SetActive(false);
                gameObject.transform.position = pos;

                if (initOK != null) initOK();
            });
        }

        /// <summary>
        /// 动作剪辑播放完成回调函数
        /// </summary>
        /// <param name="obj"></param>
        private void OnPlayingClipOK(string clip)
        {
            switch(clip)
            {
                case AnimNames.Attack:      //战斗完成
                case AnimNames.Skill:
                    OnNpcSkillAttackOK();
                    PlayRoleAnim(AnimNames.Idle, true);
                    break;
                case AnimNames.BeAttacked:      //被击完成
                    if (!bRunning)
                    {
                        PlayRoleAnim(AnimNames.Idle, true);
                    }
                    break;
            }
            GLogger.Log("OnPlayingClipOK::>>" + gameObject.name + " " + clip);
        }

        /// <summary>
        /// 出生NPC对象
        /// </summary>
        [NoToLua]
        public void SpawnNpcObject(bool initHUD = false, string layerName = Layers.Default)
        {
            if (layerName != Layers.Default)
            {
                gameObject.SetLayer(layerName);
            }
            PlayRoleAnim(AnimNames.Idle, true);

            if (initHUD)
            {
                mhudObj = Util.AddHudObject(gameObject);
                if (mhudObj != null)
                {
                    mhudObj.InitHud(1.2f, roleData.nick, NpcData.npcType);
                }
            }
        }

        /// <summary>
        /// NPC显示
        /// </summary>
        public void ShowNpc(float time, Action initOK = null)
        {
            gameObject.SetActive(true);
            var pos = gameObject.transform.position;

            if (swfClip != null)
            {
                swfClip.sortingOrder = (int)pos.z + 1;
            }
            if (time > 0f)
            {
                gameObject.transform.position = new Vector3(pos.x, pos.y + 3, pos.z);
                gameObject.transform.DOMoveY(pos.y, time).SetEase(Ease.InExpo).OnComplete(() => OnShowNpcOK(initOK));
            }
            else
            {
                if (initOK != null) initOK();
            }
        }

        /// <summary>
        /// 出生完成事件
        /// </summary>
        private void OnShowNpcOK(Action initOK)
        {
            PlayNpcSpawnSound("20000");
            if (initOK != null) initOK();
        }

        /// <summary>
        /// 播放角色动画
        /// </summary>
        public void PlayRoleAnim(string clip, bool isLoopPlay = false) 
        {
            if (swf != null)
            {
                swf.PlaySwfClip(clip, isLoopPlay);
            }
        }

        /// <summary>
        /// 移动到一个点
        /// </summary>
        internal void MoveTo(long npcid, Vector3 value, float time, Action<long> moveOK)
        {
            bRunning = true;
            long myNpcId = npcid;
            Vector3 movePos = value;
            float moveTime = time;
            PlayRoleAnim(AnimNames.Run, true);

            if (moveTweener != null)
            {
                moveTweener.Kill();
            }
            moveTweener = gameObject.transform.DOMove(movePos, moveTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                bRunning = false;
                moveTweener = null;
                PlayRoleAnim(AnimNames.Idle, true);
                if (moveOK != null) moveOK(npcid);
            });
        }

        /// <summary>
        /// 盯着某一位置
        /// </summary>
        /// <param name="pos"></param>
        public void LookAt(Vector3Int pos)
        {
            var trans = gameObject.GetChild<Transform>("roleObject");
            var scale = trans.localScale;
            var x = NpcData.position.x <= pos.x ? Math.Abs(scale.x) * -1 : Math.Abs(scale.x);
            trans.localScale = new Vector3(x, scale.y, scale.z);
        }

        /// <summary>
        /// 设置朝向
        /// </summary>
        /// <param name="dir"></param>
        public void SetFaceDir(FaceDir dir)
        {
            var trans = gameObject.GetChild<Transform>("roleObject");
            var scale = trans.localScale;
            var x = dir == FaceDir.Left ? Math.Abs(scale.x): Math.Abs(scale.x) * -1;
            trans.localScale = new Vector3(x, scale.y, scale.z);
        }

        /// <summary>
        /// 普通攻击
        /// </summary>
        public void NpcSkillAttack(RoleView targetValue, NpcSkillAttackEvent data, Action execOK)
        {
            RoleView target = targetValue;
            NpcSkillAttackEvent evData = data;

            oldSortingOrder = swfClip.sortingOrder;

            actActions[AnimNames.Attack] = execOK;
            oldPos = gameObject.transform.position;

            if (evData.bMoveToTarget)
            {
                swfClip.sortingOrder = AppConst.BattleTempSortingOrder;

                var newPos = (Vector3)target.NpcData.position;
                if (newPos.x > gameObject.transform.position.x)
                {
                    newPos.x -= 0.7f;
                }
                else
                {
                    newPos.x += 0.7f;
                }
                gameObject.transform.DOMove(newPos, 0.2f).OnComplete(delegate ()
                {
                    NpcSkillAttackInternal(target, evData);
                });
            }
            else
            {
                NpcSkillAttackInternal(target, evData);
            }
        }

        private void NpcSkillAttackInternal(RoleView target, NpcSkillAttackEvent evData)
        {
            var skillKey = evData.bUseSkill ? AnimNames.Skill : AnimNames.Attack;
            SkillData skillData = null;
            roleData.skills.TryGetValue(skillKey, out skillData);
            if (skillData != null)
            {
                var targetFrameAction = new FrameActionData();  //被击者
                targetFrameAction.currHp = evData.currHp;
                targetFrameAction.maxHp = evData.maxHp;
                targetFrameAction.amount = evData.amount;
                targetFrameAction.bPlaySound = evData.bPlaySound;
                targetFrameAction.type = FrameActionType.Passive;
                targetFrameAction.skillParams = skillData.skillParams;
                target.OnTakeDamage(targetFrameAction);

                var myFrameAction = new FrameActionData();      //攻击者
                myFrameAction.type = FrameActionType.Active;
                myFrameAction.skillParams = skillData.skillParams;
                myFrameAction.bPlaySound = evData.bPlaySound;
                myFrameAction.target = target;
                myFrameAction.animClipName = evData.bUseSkill ? AnimNames.Skill : AnimNames.Attack;
                timerMgr.CreateTicker(skillData.frameDatas, myFrameAction, OnFrameAction);
            }
            else
            {
                GLogger.Red("!!!NpcSkillAttackInternal:>>" + evData.attackerid + " " + evData.defenderid + " " + evData.bUseSkill);
            }
        }

        /// <summary>
        /// 攻击完成
        /// </summary>
        private void OnNpcSkillAttackOK()
        {
            gameObject.transform.DOMove(oldPos, 0.2f).OnComplete(delegate ()
            {
                if (actActions.ContainsKey(AnimNames.Attack))
                {
                    actActions[AnimNames.Attack]();
                    actActions.Remove(AnimNames.Attack);
                }
                swfClip.sortingOrder = oldSortingOrder;
            });
        }

        /// <summary>
        /// 受到伤害
        /// </summary>
        void OnTakeDamage(FrameActionData frameActionData)
        {
            var skillData = roleData.skills[AnimNames.Attack];
            timerMgr.CreateTicker(skillData.frameDatas, frameActionData, OnFrameAction);
        }

        /// <summary>
        /// 帧行为
        /// </summary>
        /// <param name="actId"></param>
        private void OnFrameAction(uint actId, object param)
        {
            var frameData = param as FrameActionData;
            var frameType = (SkillFrameType)actId;
            if (frameData.type == FrameActionType.Active)
            {
                switch (frameType)
                {
                    case SkillFrameType.Anim:       //攻击动画
                        OnPlayAnim(frameData);
                        break;
                    case SkillFrameType.Effect:     //播放特效
                        OnPlayEffect(frameData);
                        break;
                    case SkillFrameType.Bullet:     //发射子弹
                        OnFireBullet(frameData);
                        break;
                    case SkillFrameType.Sound:      //播放声音
                        OnPlaySound(frameData);
                        break;
                }
            }
            else
            {
                switch (frameType)
                {
                    case SkillFrameType.Health:     //血条
                        OnUpdateHealthBar(frameData.currHp, frameData.maxHp);
                        break;
                    case SkillFrameType.DamageNum:  //伤害数字
                        OnPopupFloatingText(frameData.amount);
                        break;
                    case SkillFrameType.BeAttack:   //被击特效
                        OnBeAttack(frameData);
                        break;
                }
            }
        }

        /// <summary>
        /// 发射子弹
        /// </summary>
        private void OnFireBullet(FrameActionData frameData)
        {
            var bulletid = frameData.skillParams.bulletId;
            if (bulletid > 0)
            {
                var roleObj = frameData.target as RoleView;
                var startPos = gameObject.transform.position;
                var offsetPos = Vector3.up * render.bounds.size.y / 2;
                var destPos = roleObj.gameObject.transform.position + offsetPos;
                bulletMgr.Create(bulletid, startPos, destPos, 0.2f);
            }
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        void OnPlayAnim(FrameActionData frameData)
        {
            PlayRoleAnim(frameData.animClipName);
        }

        /// <summary>
        /// 播放特效
        /// </summary>
        void OnPlayEffect(FrameActionData frameData)
        {
            var effectid = frameData.skillParams.effectId;
            if (effectid > 0)
            {
                var roleObj = frameData.target as RoleView;
                var offsetPos = Vector3.up * render.bounds.size.y / 2;
                var spawnPos = roleObj.gameObject.transform.position + offsetPos;
                effectMgr.Create(effectid, spawnPos, frameData.bPlaySound);
            }
        }

        /// <summary>
        /// 播放声音
        /// </summary>
        void OnPlaySound(FrameActionData frameData)
        {
            if (frameData.bPlaySound)
            {
                soundMgr.Play("Audios/Hit_Attack1");
            }
        }

        /// <summary>
        /// 被击ACTION
        /// </summary>
        void OnBeAttack(FrameActionData frameData)
        {
            if (!bRunning)
            {
                PlayRoleAnim(AnimNames.BeAttacked);
            }
            var effectid = frameData.skillParams.beAttackEffectId;
            if (effectid != 0)
            {
                var pos = gameObject.transform.position;
                effectMgr.Create(effectid, pos, frameData.bPlaySound);
            }
        }

        /// <summary>
        /// 更新血条
        /// </summary>
        void OnUpdateHealthBar(long currhp, long maxhp)
        {
            if (mhudObj != null)
            {
                float curr = currhp.ToFloat();
                float max = maxhp.ToFloat();
                mhudObj.SetHealthBarValue((float)curr / (float)max);
            }
        }

        /// <summary>
        /// 显示飘字
        /// </summary>
        /// <param name="amount"></param>
        private void OnPopupFloatingText(int amount)
        {
            var pos = gameObject.transform.position;
            FloatingText.Create(battleScene, pos, amount);
        }

        /// <summary>
        /// NPC死亡
        /// </summary>
        public void OnNpcDeath(Action execOK)
        {
            var parentName = gameObject.name;
            if (swfClip != null)
            {
                var color = swfClip.tint; color.a = 0;
                DOTween.To(() => swfClip.tint, x => swfClip.tint = x, color, 1).OnComplete(delegate ()
                {
                    base.OnNpcDeath();
                    Util.RemoveHudObject(parentName);
                    if (execOK != null) execOK();
                });
            }
            else
            {
                base.OnNpcDeath();
                Util.RemoveHudObject(parentName);
                if (execOK != null) execOK();
            }
        }

        /// <summary>
        /// 销毁自身
        /// </summary>
        [NoToLua]
        public override void OnDispose()
        {
            base.OnDispose();
            swf.onStopPlayingEvent -= OnPlayingClipOK;
        }
    }
}


namespace FirClient.Define
{
    public class EventNames
    {
        public const string EvLogicUpdate = "LogicUpdate";                  //逻辑层UPDATE
        public const string EvNpcSpawn = "NpcSpawn";                        //生成角色
        public const string EvNpcSpawnOK = "NpcSpawnOK";                    //出生角色OK
        public const string EvEnterScene = "EnterScene";                    //进入场景
        public const string EvBeginPlay = "BeginPlay";                      //进入场景OK
        public const string EvMoveCamera = "MoveCamera";                    //相机跟随
        public const string EvNpcSkillAttack = "NpcSkillAttack";            //攻击NPC
        public const string EvNpcSkillAttackOK = "NpcSkillAttackOK";        //攻击NPC完成
        public const string EvNpcDeath = "NpcDeath";                        //NPC死亡
        public const string EvEnterDungeon = "EnterDungeon";                //进入副本
        public const string EvEnterDungeonOK = "EnterDungeonOK";            //进入副本OK
        public const string EvBattleStart = "BattleStart";                  //战斗开始
        public const string EvBattleEnd = "BattleEnd";                      //战斗结束
        public const string EvChooseNpc = "ChooseNpc";                      //选择NPC
        public const string EvNpcMove = "NpcMove";                          //NPC移动
        public const string EvNpcMoveOK = "NpcMoveOK";                      //移动完成
        public const string EvNpcFaceDir = "NpcFaceDir";                    //NPC朝向
        public const string EvNpcShow = "NpcShow";                          //NPC展示
        public const string EvNpcShowOK = "NpcShowOK";                      //NPC展示
        public const string EvShowDialog = "ShowDialog";                    //显示对话
    }
}
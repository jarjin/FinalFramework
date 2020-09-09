using FirClient.Data;

namespace FirClient.Logic
{
    public class LogicConst
    {
        public static float Viewport_Margin = 0;

        private static SceneType _sceneType;
        public static SceneType SceneType
        {
            get { return _sceneType; }
            set { _sceneType = value; }
        }

        public static BattleType BattleType
        {
            get
            {
                return SceneType == SceneType.BigScene ? BattleType.FreeBattle : BattleType.TurnBase;
            }
        }
    }
}
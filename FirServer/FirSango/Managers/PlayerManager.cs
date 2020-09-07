using System.Collections.Concurrent;
using System.Collections.Generic;
using FirSanguo;
using FirServer;
using FirServer.Defines;
using FirServer.Interface;
using FirServer.Models;
using Utility;

namespace GameLibs.FirSango.Managers
{
    // 玩家管理器
    public class PlayerManager : BaseBehaviour, IManager
    {
        /// <summary>
        /// 在线角色列表
        /// - 键: 角色ID
        /// - 值: 角色数据
        /// </summary>
        private static ConcurrentDictionary<ulong, PlayerCore> playerTable = new ConcurrentDictionary<ulong, PlayerCore>();
        /// <summary>
        /// 角色简要信息表
        /// - 键: 角色ID
        /// - 值: 角色简要信息
        /// </summary>
        private static ConcurrentDictionary<ulong, PlayerDispaly> playerDisplayTable = new ConcurrentDictionary<ulong, PlayerDispaly>();
        private static int playerCount;

        public PlayerManager()
        {
        }

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public void OnDispose()
        {
            throw new System.NotImplementedException();
        }

        #region Load
        
         /// <summary>
        /// 读取玩家角色简要信息列表
        /// </summary>
        public static void LoadPlayerDisplayTable()
        {
            // gamerDisplayTable = GamerFactory.GetGamerDisplayList();
            //
            // foreach (var gamerDisplay in gamerDisplayTable)
            // {
            //     gamerDisplayByCodeTable.Renew(gamerDisplay.Value.UniqueCode, gamerDisplay.Value);
            //     RenewGamerName(gamerDisplay.Value.GamerName);
            // }
            var userModel = modelMgr.GetModel(ModelNames.Player) as PlayerModel;
            if (userModel != null)
            {
                var list = new List<string>();
                // todo: 
            }

            playerCount = playerDisplayTable.Count;
        }

        #endregion

        #region Save

        
        #endregion

        #region Add

        /// 增加新玩家
        public static ulong AddPlayer(string strNickName, string strRole)
        {
            var uid = (ulong)AppUtil.NewGuidId();
            var player = new PlayerCore();
            player.playerbase = new PlayerBase()
            {
                playerid = uid,
                nickname = strNickName,
                role = strRole
            };

            playerTable.Renew(uid, player);
            playerDisplayTable.Renew(uid,new PlayerDispaly(){
                playerbase = player.playerbase
            });

            return uid;
        }

        #endregion

        #region Renew

        public static bool UpdatePlayer(PlayerCore player)
        {   
            if (player == null)
            {
                return false;
            }
            playerTable.Renew(player.playerbase.playerid, player);
            return true;
        }

        #endregion

        #region Remove
        // TODO：原则上不可以移除玩家
        #endregion

    }
}
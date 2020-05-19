using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirSanguo;

namespace GameLibs.FirSango
{
    public static class GamerCache
    {
        /// <summary>
        /// 在线角色列表
        /// - 键: 角色ID
        /// - 值: 角色数据
        /// </summary>
        private static ConcurrentDictionary<ulong, PlayerCore> gamerTable = new ConcurrentDictionary<ulong, PlayerCore>();

        /// <summary>
        /// 角色简要信息表
        /// - 键: 角色ID
        /// - 值: 角色简要信息
        /// </summary>
        private static ConcurrentDictionary<ulong, PlayerDispaly> gamerDisplayTable = new ConcurrentDictionary<ulong, PlayerDispaly>();
        private static int gamerCount;
        
        
        /// <summary>
        /// 读取玩家角色简要信息列表
        /// </summary>
        public static void LoadGamerDisplayTable()
        {
            // gamerDisplayTable = GamerFactory.GetGamerDisplayList();
            //
            // foreach (var gamerDisplay in gamerDisplayTable)
            // {
            //     gamerDisplayByCodeTable.Renew(gamerDisplay.Value.UniqueCode, gamerDisplay.Value);
            //     RenewGamerName(gamerDisplay.Value.GamerName);
            // }

            gamerCount = gamerDisplayTable.Count;
        }

        /// <summary>
        /// 登录时初始化玩家信息
        /// </summary>
        public static PlayerCore LoadPlayerCore(string userName, string sdkTag = "")
        {
            if (string.IsNullOrEmpty(userName))
                return null;

            if (!string.IsNullOrEmpty(sdkTag))
                userName = string.Format("{0}:{1}", userName, sdkTag);

            // var gamer = GamerFactory.GetGamer(userName);
            // if (gamer != null)
            //     gamer = gamerTable.AddOrUpdate(gamer.GamerID, gamer, (k, v) => v); // 如果有数据存在，则使用已有数据
            //
            // return gamer;
            return null;
        }

        #region Get GamerDisplay

        public static List<PlayerDispaly> GetGamerDisplayList()
        {
            return gamerDisplayTable.Values.ToList();
        }

        public static List<PlayerDispaly> GetGamerDisplayList(List<ulong> gamerIDList)
        {
            var displayList = new List<PlayerDispaly>();
            foreach (var gamerID in gamerIDList)
            {
                var display = GamerCache.GetGamerDisplay(gamerID);
                if (display != null)
                    displayList.Add(display);
            }

            return displayList;
        }
        

        /// <summary>
        /// 获取角色显示信息
        /// </summary>
        public static PlayerDispaly GetGamerDisplay(ulong playerId)
        {
            if (!gamerDisplayTable.ContainsKey(playerId))
                return null;

            return gamerDisplayTable[playerId];
        }

        public static void RenewGamerDisplay(PlayerDispaly gamerDisplay)
        {
            // todo:
        }

        #endregion

        #region Get Gamer

        /// <summary>
        /// 获取玩家列表
        /// </summary>
        public static List<PlayerCore> GetPlayerCores()
        {
            return gamerTable.Values.ToList();
        }

        public static List<PlayerCore> GetPlayerCoresByIds(List<ulong> gamerIDList)
        {
            var gamerList = new List<PlayerCore>();
            foreach (var gamerID in gamerIDList)
            {
                var gamer = GamerCache.GetPlayer(gamerID);
                if (gamer != null)
                    gamerList.Add(gamer);
            }

            return gamerList;
        }

        /// <summary>
        /// 获取在线角色
        /// </summary>
        public static PlayerCore GetPlayer(ulong playerId)
        {
            if (!gamerTable.ContainsKey(playerId))
                return null;

            return gamerTable[playerId];
        }

        public static PlayerCore GetPlayer_GM(ulong palyerId)
        {
            var gamerDisplay = GamerCache.GetGamerDisplay(palyerId);
            if (gamerDisplay == null || gamerDisplay.playerbase.playerid > 0)
                return null;

            return GamerCache.GetPlayer(palyerId) ?? GamerCache.LoadPlayer(palyerId);
        }
        

        #endregion

        /// <summary>
        /// 创建角色
        /// </summary>
        public static void CreatePlayer(PlayerCore player)
        {
            // 生成唯一标识
            gamerCount++;
            // gamer.CoreData.UniqueCode = (gamer.GamerID.ToString().SplitTrim("-")[1] + EncryptHelper.TinyEncrypt(gamerCount)).ToUpper();

            // RenewGamer(gamer);
            // RenewGamerName(gamer.Name);
            //
            // var task = new Task(() => GamerFactory.CreateGamer(gamer));
            // task.Start();
        }
        

        // /// <summary>
        // /// 更新角色
        // /// </summary>
        // public static void UpdateGamer(Gamer gamer, PriorityType priorityType = PriorityType.Later)
        // {
        //     gamerTable.Renew(gamer.GamerID, gamer);
        //
        //     switch (priorityType)
        //     {
        //         case PriorityType.Immediately:
        //             // 新建线程
        //             var task = new Task(() => GamerFactory.UpdateGamer(gamer));
        //             task.Start();
        //             return;
        //         case PriorityType.Prior:
        //             if (!batchUpdateList.Any(u => u.Key == gamer.GamerID))
        //                 batchUpdateList.Add(new UpdateItem() { Key = gamer.GamerID, UpdateTime = DateTime.Now });
        //             break;
        //         case PriorityType.Later:
        //             if (!batchUpdateList.Any(u => u.Key == gamer.GamerID))
        //                 batchUpdateList.Add(new UpdateItem() { Key = gamer.GamerID, UpdateTime = DateTime.Now.AddSeconds(GlobalVar.BatchUpdateTime) });
        //             break;
        //     }
        // }

        // /// <summary>
        // /// 批量更新角色
        // /// </summary>
        // public static void BatchUpdateGamer(int limitNumber = int.MaxValue)
        // {
        //     var count = Math.Min(batchUpdateList.Count, limitNumber);
        //     UpdateItem updateItem;
        //
        //     while (batchUpdateList.TryTake(out updateItem))
        //     {
        //         try
        //         {
        //             var gamer = GetPlayer(updateItem.Key);
        //             if (gamer != null)
        //                 GamerFactory.UpdateGamer(gamer);
        //         }
        //         catch (Exception ex)
        //         {
        //             LogHelper.WriteErrorLog(ex);
        //         }
        //     }
        //     
        //     LogHelper.WriteDebugLog(string.Format("Update Gamer Pre:{0} Now:{1}", count, batchUpdateList.Count));
        // }

        /// <summary>
        /// 移除角色在线缓存
        /// </summary>
        public static void RemovePlayer(ulong playerId)
        {
            // gamerTable.Remove(playerId);
        }

        /// <summary>
        /// 验证角色名
        /// </summary>
        public static bool ValidateGamerNameAvailability(string gamerName)
        {
            if (string.IsNullOrEmpty(gamerName))
                return false;

            // if (gamerNameList.Contains(gamerName.ToLower()))
            //     return false;

            return true;
        }

        /// <summary>
        /// 获取在线人数
        /// </summary>
        public static int GetOnlineCount()
        {
            return gamerTable.Count;
        }

        public static List<PlayerDispaly> GetGamerDisplayList(string username)
        {
            return gamerDisplayTable.Values.Where(g => g.playerbase.nickname == username).ToList();
        }

        /// <summary>
        /// 登录时初始化玩家信息
        /// </summary>
        public static PlayerCore LoadPlayer(ulong playerId)
        {
            if (playerId == null)
                return null;

            // var gamer = GamerFactory.GetGamerByGamerID(gamerID);
            // if (gamer != null)
            //     gamer = gamerTable.AddOrUpdate(gamer.GamerID, gamer, (k, v) => v); // 如果有数据存在，则使用已有数据
            //
            // return gamer;
            return null;
        }
    }
}
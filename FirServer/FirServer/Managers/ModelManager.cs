using System;
using System.Collections.Generic;
using log4net;
using FirServer.Defines;
using FirServer.Models;
using FirServer.Interface;

namespace FirServer.Managers
{
    public class ModelManager : BaseBehaviour, IManager
    {
        private static Dictionary<string, BaseModel> models = new Dictionary<string, BaseModel>();
        private static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(ModelManager));

        public ModelManager()
        {
        }

        public void Initialize()
        {
            modelMgr = this;

            models.Clear();
            AddModel(ModelNames.User, new UserModel());
            AddModel(ModelNames.Battle, new BattleModel());
            AddModel(ModelNames.Player,new PlayerModel());
        }

        public BaseModel GetModel(string strKey)
        {
            if (models.ContainsKey(strKey))
            {
                return models[strKey];
            }
            return null;
        }

        public void AddModel(string strKey, BaseModel model)
        {
            if (models.ContainsKey(strKey))
            {
                throw new Exception();
            }
            models.Add(strKey, model);
        }

        public void RemoveModel(string strKey)
        {
            if (models.ContainsKey(strKey))
            {
                models.Remove(strKey);
            }
        }

        public void OnDispose()
        {
            modelMgr = null;
        }
    }
}

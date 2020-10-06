using System;
using System.Collections.Generic;
using log4net;
using FirServer.Model;

namespace FirServer.Manager
{
    public class ModelManager : BaseManager
    {
        private static Dictionary<string, BaseModel> models = new Dictionary<string, BaseModel>();
        private static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(ModelManager));

        public ModelManager()
        {
        }

        public override void Initialize()
        {
            models.Clear();
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
    }
}

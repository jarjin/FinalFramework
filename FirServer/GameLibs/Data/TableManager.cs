using ProtoBuf.Meta;
using UnityEngine;

namespace FirCommon.Data
{
	public class TableManager : BaseObject
	{
		private static TableManager instance;
    	public GlobalConfigTable globalConfigTable;
    	public ItemTable itemTable;
    	public NpcTable npcTable;
    	public ObjectPoolTable objectPoolTable;
    	public QualityTable qualityTable;
///[APPEND_VAR]

		public static TableManager Create()
		{
			if (instance == null)
			{
				instance = new TableManager();
			}
			return instance;
		}

		public override void Initialize()
		{
			this.RegTypes();
			this.LoadTables();
		}

		public void RegTypes()
		{
            RuntimeTypeModel.Default.Add<SurrogateVector2>();
            RuntimeTypeModel.Default.Add(typeof(Vector2), false).SetSurrogate(typeof(SurrogateVector2));

            RuntimeTypeModel.Default.Add<SurrogateVector3>();
            RuntimeTypeModel.Default.Add(typeof(Vector3), false).SetSurrogate(typeof(SurrogateVector3));

            RuntimeTypeModel.Default.Add<SurrogateColor>();
            RuntimeTypeModel.Default.Add(typeof(Color), false).SetSurrogate(typeof(SurrogateColor));

            RuntimeTypeModel.Default.Add<SurrogateColor32>();
            RuntimeTypeModel.Default.Add(typeof(Color32), false).SetSurrogate(typeof(SurrogateColor32));
        }
			
		public T LoadData<T>(string path) where T : class
		{
            var fullPath = base.DataPath + path;
            return FirCommon.Utility.ProtoUtil.Deserialize<T>(fullPath);
		}

		public void LoadTables() 
		{
        	globalConfigTable = LoadData<GlobalConfigTable>("Tables/GlobalConfigTable.bytes");
        	globalConfigTable.Initialize();
        	itemTable = LoadData<ItemTable>("Tables/ItemTable.bytes");
        	itemTable.Initialize();
        	npcTable = LoadData<NpcTable>("Tables/NpcTable.bytes");
        	npcTable.Initialize();
        	objectPoolTable = LoadData<ObjectPoolTable>("Tables/ObjectPoolTable.bytes");
        	objectPoolTable.Initialize();
        	qualityTable = LoadData<QualityTable>("Tables/QualityTable.bytes");
        	qualityTable.Initialize();
///[APPEND_TABLE]
		}

		public override void OnUpdate(float deltaTime)
		{
		}

		public override void OnDispose()
		{
		}
	}
}

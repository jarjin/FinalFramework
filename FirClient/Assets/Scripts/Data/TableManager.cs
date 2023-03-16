namespace FirCommon.Data
{
	public class TableManager : BaseObject
	{
		private static TableManager instance;
    	public NpcTable npcTable;
    	public ObjectPoolTable objectPoolTable;
    	public GlobalConfigTable globalConfigTable;
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
			this.LoadTables();
		}
			
		public T LoadData<T>(string path) where T : class
		{
            var fullPath = base.DataPath + path;
            return FirCommon.Utility.ProtoUtil.Deserialize<T>(fullPath);
		}

		public void LoadTables() 
		{
        	npcTable = LoadData<NpcTable>("Tables/NpcTable.bytes");
        	npcTable.Initialize();
        	objectPoolTable = LoadData<ObjectPoolTable>("Tables/ObjectPoolTable.bytes");
        	objectPoolTable.Initialize();
        	globalConfigTable = LoadData<GlobalConfigTable>("Tables/GlobalConfigTable.bytes");
        	globalConfigTable.Initialize();
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

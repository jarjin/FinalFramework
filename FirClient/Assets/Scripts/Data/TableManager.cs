namespace FirCommon.Data
{
	public class TableManager : BaseObject
	{
		private static TableManager instance;
    	public NpcTable npcTable;
    	public ObjectPoolTable objectPoolTable;
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
            return SerializeUtil.Deserialize<T>(fullPath);
		}

		public void LoadTables() 
		{
        	npcTable = LoadData<NpcTable>("Tables/NpcTable.bytes");
        	npcTable.Initialize();
        	objectPoolTable = LoadData<ObjectPoolTable>("Tables/ObjectPoolTable.bytes");
        	objectPoolTable.Initialize();
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

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class TableManager : BaseObject
{
	private static TableManager instance;
    public GlobalConfigTable globalConfigTable;
    public ItemTable itemTable;
    public NpcTable npcTable;
    public QualityTable qualityTable;
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
        IFormatter serializer = new BinaryFormatter();
        using (var loadFile = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
        {
            return serializer.Deserialize(loadFile) as T;
        }
    }

    public void LoadTables() 
    {
        globalConfigTable = LoadData<GlobalConfigTable>("Tables/GlobalConfigTable.bytes");
        globalConfigTable.Initialize();
        itemTable = LoadData<ItemTable>("Tables/ItemTable.bytes");
        itemTable.Initialize();
        npcTable = LoadData<NpcTable>("Tables/NpcTable.bytes");
        npcTable.Initialize();
        qualityTable = LoadData<QualityTable>("Tables/QualityTable.bytes");
        qualityTable.Initialize();
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

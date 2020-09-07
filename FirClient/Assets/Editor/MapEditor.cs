using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditorInternal;
using FirClient.Component;
using FirClient.Extensions;

[CustomEditor(typeof(MapInfo))]
public class MapEditor : BaseEditor
{
    public static string CurrMapName;

    private MapInfo _instance;
    void OnEnable()
    {
        _instance = (MapInfo)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        this.DrawBottomUI();
    }

    void OnSceneGUI()
    {
        //var brushPos = grid.CellToWorld(new Vector3Int(position.x, position.y, position.z));
        //Handles.Label(brushPos, brushPos.ToString());
    }

    [MenuItem("Assets/Create/Game/EventTile", false, 79)]
    static void CreateEventTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Event Object", "New Event Object", "asset", "Save Event Object", "Assets");
        if (string.IsNullOrEmpty(path)) return;
        //var obj = ScriptableObject.CreateInstance<CEventObject>();
        //AssetDatabase.CreateAsset(obj, path);
    }
     
    void DrawBottomUI()
    {
        GUI.color = Color.gray;
        if (GUILayout.Button("Save MapData to Xml!!!"))
        {
            this.SaveMap();
        }
        GUI.color = Color.white;
    }

    void SaveGridData() 
    {
        var spawn_point = _instance.transform.Find("event_points");
        var content = string.Empty;
        for(int i = 0; i < spawn_point.childCount; i++)
        {
            var spawnNode = spawn_point.GetChild(i);
            var mapPos = spawnNode.transform.position;
            var eventobj = spawnNode.GetComponent<CEventObject>();
            var eventObjs = eventobj.SerializeEvents();
            content += "    <item name='" + spawnNode.name +
                               "' pos='" + mapPos.ToStr("_") +
                               "' eventids='" + eventObjs + "' />\n";
        }
        if (content.EndsWith("\n")) {
            content = content.Remove(content.Length - 1);
        }
        var template = File.ReadAllText(AppDataPath + "/Templates/Map.txt");
        template = template.Replace("{#content}", content);
        var path = AppDataPath + "/res/Datas/Events/" + _instance.name + ".xml";
        File.WriteAllText(path, template);
        AssetDatabase.Refresh();
    }

    void CopyPasteComponentData(Component src, Component dest)
    {
        ComponentUtility.CopyComponent(src);
        ComponentUtility.PasteComponentValues(dest);
    }

    /// <summary>
    /// 保存地图
    /// </summary>
    void SaveMap()
    {
        if (EditorUtility.DisplayDialog("Warning", "Are you sure save current map?", "Yes", "No"))
        {
            this.SaveGridData(); 
            // this.ExportServerMapData();
            // var prefab = map.gameObject;
            // var srcPrefab = PrefabUtility.GetPrefabParent(prefab);
            // PrefabUtility.ReplacePrefab(prefab, srcPrefab, ReplacePrefabOptions.ConnectToPrefab);
            // EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }
    }

    /// <summary>
    /// 导出服务器地图数据
    /// </summary>
    void ExportServerMapData()
    {
    }
}

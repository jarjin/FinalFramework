using UnityEngine;
using UnityEditor;
using FirClient.Utility;
using System.IO;
using FirClient.Define;

public class VersionEditor : EditorWindow
{
    VersionInfo currVersion = null;

    [MenuItem("GameAsset/Open Version Editor")]//在unity菜单Window下有MyWindow选项
    static void Init()
    {
        var myWindow = (VersionEditor)EditorWindow.GetWindow(typeof(VersionEditor), false, "VersionEditor", true);//创建窗口
        myWindow.Show();//展示
    }

    void OnGUI()
    {
        GUILayout.BeginVertical();
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Full Version:"), GUILayout.Width(100));
                GUILayout.Label(new GUIContent(currVersion.ToString()), GUILayout.Width(500));
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Main Version:"), GUILayout.Width(100));
                currVersion.mainVersion = GUILayout.TextField(currVersion.mainVersion);
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Primary Version:"), GUILayout.Width(100));
                currVersion.primaryVersion = GUILayout.TextField(currVersion.primaryVersion);
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Patch Version:"), GUILayout.Width(100));
                currVersion.patchVersion = GUILayout.TextField(currVersion.patchVersion);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
                if (GUILayout.Button("Make New PatchVersion!"))
                {
                    currVersion.patchVersion = MakePatchVersion();
                }
                if (GUILayout.Button("Save Current Version!"))
                {
                    SaveVersion(currVersion);
                }
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
    }

    private void OnEnable() {
        LoadVersion();    
    }

    /// <summary>
    /// 1.1.10001
    /// </summary>
    void LoadVersion() 
    {
        currVersion = new VersionInfo();
        var verPath = Application.dataPath + "/version.txt";
        if (File.Exists(verPath))
        {
            var content = File.ReadAllText(verPath);
            var strs = content.Split('.');
            currVersion.mainVersion = strs[0];
            currVersion.primaryVersion = strs[1];
            currVersion.patchVersion = strs[2];
        }
    }

    string MakePatchVersion() 
    {
        return Util.RandomTime();
    }

    void SaveVersion(VersionInfo info)
    {
        var str = string.Format("{0}.{1}.{2}", info.mainVersion, info.primaryVersion, info.patchVersion);
        File.WriteAllText(Application.dataPath + "/version.txt", str);
        Debug.Log("Save Version:" + info.ToString());
    }

    void OnInspectorUpdate()
    {
        this.Repaint();
    }
}

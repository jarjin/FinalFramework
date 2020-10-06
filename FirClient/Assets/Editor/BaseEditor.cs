using UnityEngine;
using UnityEditor;
using System.IO;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System;
using Debug = UnityEngine.Debug;
using static UnityEditorInternal.ReorderableList;
using UnityEditorInternal;
using FirClient.Utility;

public class BaseEditor : Editor
{
    private static GameSettings currSettings;
    private static bool isInitialize = false;

    public static GameSettings gameSettings
    {
        get
        {
            if (currSettings == null)
            {
                currSettings = Util.LoadGameSettings();
            }
            return currSettings;
        }
    }

    /// <summary>
    /// 数据目录
    /// </summary>
    public static string AppDataPath
    {
        get
        {
            return Application.dataPath;
        }
    }

    public static string AppDataWithoutAssetPath
    {
        get
        {
            return AppDataPath.Replace("Assets", string.Empty);
        }
    }

    public static string StreamDir
    {
        get
        {
            return Application.streamingAssetsPath;
        }
    }

    private int FolderContentsCount(string path)
    {
        int result = Directory.GetFiles(path).Length;
        string[] subFolders = Directory.GetDirectories(path);
        foreach (string subFolder in subFolders)
        {
            result += FolderContentsCount(subFolder);
        }
        return result;
    }

    public static void ExecuteProc(string proc, string args = null, bool useShell = false)
    {
        Debug.Log(proc + " " + args);

        ProcessStartInfo info = new ProcessStartInfo();
        info.FileName = proc;
        info.Arguments = args;
        info.WindowStyle = ProcessWindowStyle.Hidden;
        info.UseShellExecute = useShell;
        info.RedirectStandardError = !useShell;

        Process pro = Process.Start(info);
        pro.WaitForExit();

        if (!useShell)
        {
            string msg = pro.StandardError.ReadToEnd();
            if (!string.IsNullOrEmpty(msg))
            {
                Debug.LogError(msg);
            }
        }
    }

    /// <summary>
    /// 编译
    /// </summary>
    public static void CompileWithExecuteCode(string className, string classCode, string[] assemblys = null)
    {
        CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
        CompilerParameters paras = new CompilerParameters();
        if (assemblys != null)
        {
            foreach (string assembly in assemblys)
            {
                paras.ReferencedAssemblies.Add(assembly + ".dll");
            }
        }
        paras.GenerateExecutable = false;
        paras.GenerateInMemory = true;

        CompilerResults result = provider.CompileAssemblyFromSource(paras, classCode);
        if (result.Errors.HasErrors)
        {
            string ErrorMessage = "";
            foreach (CompilerError err in result.Errors)
            {
                ErrorMessage += err.ErrorText;
            }
            UnityEngine.Debug.LogError(ErrorMessage);
        }
        else
        {
            object instance = result.CompiledAssembly.CreateInstance(className);
            Type classType = result.CompiledAssembly.GetType(className);
            try
            {
                classType.GetMethod("Execute").Invoke(instance, null);
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("Execute Error!");
            }
        }
    }

    protected static bool TryCreateDir(string dirName)
    {
        if (!Directory.Exists(dirName))
        {
            Directory.CreateDirectory(dirName);
            return true;
        }
        return false;
    }

    protected static bool RetryCreateDir(string dirName)
    {
        if (Directory.Exists(dirName))
        {
            Directory.Delete(dirName, true);
        }
        return TryCreateDir(dirName);
    }

    protected static bool TryCreateDirWithoutFileExt(string dirName)
    {
        var dir = dirName.Substring(0, dirName.LastIndexOf('/'));
        return TryCreateDir(dir);
    }

    public static long FileSize(string filePath)
    {
        if (!File.Exists(filePath)) return 0;
        return new FileInfo(filePath).Length;
    }

    public static string GetSelectedPathOrFallback()
    {
        string path = "Assets";
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        return path;
    }

    public static string GetSelectedObjectPath()
    {
        return AssetDatabase.GetAssetPath(Selection.activeObject);
    }

    public ReorderableList CreateRecordList(SerializedObject serializedObject, string varName, string title,
               ReorderCallbackDelegate onreorder, SelectCallbackDelegate onselect, AddCallbackDelegate onadd, RemoveCallbackDelegate onremove)
    {
        var reordList = new ReorderableList(serializedObject, serializedObject.FindProperty(varName), true, true, true, true);
        reordList.displayAdd = reordList.displayRemove = true;
        reordList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, title, EditorStyles.boldLabel);
            Texture2D btnTexture = reordList.elementHeight == 0f ? EditorGUIUtility.FindTexture("winbtn_win_max_h") : EditorGUIUtility.FindTexture("winbtn_win_min_h");
            if (GUI.Button(new Rect(rect.width + 4, rect.y + 2, rect.height, rect.height), btnTexture, EditorStyles.label))
            {
                reordList.elementHeight = reordList.elementHeight == 0f ? 21f : 0f;
                reordList.draggable = reordList.elementHeight > 0f;
            }
        };
        reordList.onReorderCallback = onreorder;
        reordList.onSelectCallback = onselect;
        reordList.onAddCallback = onadd;
        reordList.onRemoveCallback = onremove;
        return reordList;
    }

    public static void ShowMessage(string content)
    {
        EditorUtility.DisplayDialog("信息", content, "确定");
    }

    public static string GetUnityAssetPath(string path)
    {
        return path.Replace(AppDataPath, "Assets");
    }

    public static void MergeMesh(string srcpath, string destpath)
    {
        GameObject target = AssetDatabase.LoadAssetAtPath<GameObject>(srcpath);
        if (target != null)
        {
            MeshRenderer[] meshRenderers = target.GetComponentsInChildren<MeshRenderer>();
            Material[] mats = new Material[meshRenderers.Length];
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                mats[i] = meshRenderers[i].sharedMaterial;
            }
            GameObject instance = new GameObject();
            var meshFilters = target.GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combineInstances = new CombineInstance[meshFilters.Length];
            for (int i = 0; i < meshFilters.Length; i++)
            {
                combineInstances[i].mesh = meshFilters[i].sharedMesh;
                combineInstances[i].transform = meshFilters[i].transform.localToWorldMatrix;
            }
            Mesh newMesh = new Mesh();
            newMesh.CombineMeshes(combineInstances);
            instance.AddComponent<MeshFilter>().sharedMesh = newMesh;
            instance.GetComponent<MeshRenderer>().sharedMaterials = mats;

            PrefabUtility.SaveAsPrefabAsset(instance, destpath);
            GameObject.DestroyImmediate(instance);

            AssetDatabase.Refresh();
        }
    }

    public static string GetGameObjectPath(GameObject obj)
    {
        string path = "/" + obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = "/" + obj.name + path;
        }
        return path;
    }
}

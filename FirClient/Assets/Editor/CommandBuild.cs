using UnityEditor;
using UnityEngine;

public class CommandBuild : BaseEditor
{
    public static void PerformBuild()
    {
    }

    [MenuItem("Assets/Open PersistentPath")]
    static void OpenPersistentPath()
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            System.Diagnostics.Process.Start(Application.persistentDataPath);
        }
        else if(Application.platform == RuntimePlatform.WindowsEditor)
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
    }
}

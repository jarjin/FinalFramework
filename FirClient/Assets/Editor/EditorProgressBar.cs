using UnityEditor;

public class EditorProgressBar : BaseEditor
{
    public static void OnUpdate(string title, string info, float progress, float max)
    {
        EditorUtility.DisplayProgressBar(title, info, (float)(progress / max));
    }

    public static void CloseBar() 
    {
        EditorUtility.ClearProgressBar();
    }
}

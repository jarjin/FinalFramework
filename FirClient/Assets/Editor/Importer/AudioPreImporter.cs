using UnityEditor;

public static class AudioPreImporter
{
    public static void ProcAudio(string assetPath, ref AudioImporter importer)
    {
        if (assetPath.Contains("mono"))
        {
            importer.forceToMono = true;
        }
    }
}

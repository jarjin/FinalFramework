using UnityEditor;

public static class TexturePreImporter
{
    static BuildTarget activePlatform = EditorUserBuildSettings.activeBuildTarget;

    public static void ProcTexture(string assetPath, ref TextureImporter importer)
    {
        importer.mipmapEnabled = false;
        importer.compressionQuality = 50;
        importer.textureType = TextureImporterType.Sprite;

        if (assetPath.StartsWith("Assets/res/Atlas"))
        {
            importer.spriteImportMode = SpriteImportMode.Multiple;
        }
        if (assetPath.StartsWith("Assets/res/Fonts"))
        {
            importer.textureType = TextureImporterType.Default;
        }
        var settings = new TextureImporterPlatformSettings();
        settings.compressionQuality = 50;
        var info = GetTexCompressInfo(assetPath);
        if (info == null)
        {
            switch (activePlatform)
            {
                case BuildTarget.Android:
                    settings.format = TextureImporterFormat.ETC2_RGBA8;
                    settings.androidETC2FallbackOverride = AndroidETC2FallbackOverride.Quality32Bit;
                break;
                case BuildTarget.iOS:
                    settings.format = TextureImporterFormat.ASTC_6x6;
                break;
            }
            settings.maxTextureSize = 1024;
        }
        else
        {
            switch (activePlatform)
            {
                case BuildTarget.Android:
                    settings.format = info.androidFormat;
                break;
                case BuildTarget.iOS:
                    settings.format = info.iosFormat;
                break;
            }
            settings.maxTextureSize = GetTextureSize(info.textureSize);
        }
        importer.SetPlatformTextureSettings(settings);
    }

    static int GetTextureSize(TextureSize type)
    {
        switch (type)
        {
            case TextureSize.MAX_1024: return 1024;
            case TextureSize.MAX_2048: return 2048;
            case TextureSize.MAX_4096: return 4096;
        }
        return 1024;
    }

    static TextureCompressInfo GetTexCompressInfo(string assetPath)
    {
        TextureCompressInfo info = null;
        var list = BaseEditor.gameSettings.atlasSettings;
        if (list != null)
        {
            foreach (var item in list)
            {
                var path = "Assets/" + item.assetPath;
                if (assetPath.Contains(path))
                {
                    return item;
                }
            }
        }
        return info;
    }
}

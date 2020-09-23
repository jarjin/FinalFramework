using UnityEditor;

public static class TexturePreImporter
{
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
        var andSettings = new TextureImporterPlatformSettings();
        var iosSettings = new TextureImporterPlatformSettings();
        andSettings.name = "Android";
        iosSettings.name = "iPhone";
        andSettings.overridden = true;
        iosSettings.overridden = true;
        andSettings.compressionQuality = iosSettings.compressionQuality = 50;
        var info = GetTexCompressInfo(assetPath);
        if (info == null)
        {
            andSettings.format = TextureImporterFormat.ETC2_RGBA8;
            andSettings.androidETC2FallbackOverride = AndroidETC2FallbackOverride.Quality32Bit;
            iosSettings.format=TextureImporterFormat.ASTC_6x6;
            andSettings.maxTextureSize = iosSettings.maxTextureSize = 1024;
        }
        else
        {
            andSettings.format = info.androidFormat;
            iosSettings.format = info.iosFormat;
            andSettings.maxTextureSize =  iosSettings.maxTextureSize= GetTextureSize(info.textureSize);
        }
        importer.SetPlatformTextureSettings(andSettings);
        importer.SetPlatformTextureSettings(iosSettings);
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
        if (BaseEditor.gameSettings != null)
        {
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
        }
        return info;
    }
}

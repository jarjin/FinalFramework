using System.IO;
using UnityEditor;
using UnityEngine;

public class AtlasBuilder : BaseEditor
{
    public enum AtlasType
    {
        UI,
        Sprite
    }
    public static int texWidth = 1024;
    public static int texHeight = 1024;

    [MenuItem("Assets/Create Or Update Atlas", false, 504)]
    static void CreateOrUpdateAtlas()
    {
        string texPrefix = gameSettings.uiTexturePath;
        string atlasPrefix = gameSettings.uiAtlasPath;
        string fullTexPath = AppDataWithoutAssetPath + texPrefix;
        if (!Directory.Exists(fullTexPath) || fullTexPath.Contains("."))
        {
            Debug.LogError("Texture Path not found!!!");
            return;
        }
        string tpath = gameSettings.texturePackerPath + "\\TexturePacker.exe";
        if (!File.Exists(tpath))
        {
            Debug.LogError("TexturePacker.exe not found!!!");
            return;
        }
        string texturePath = GetSelectedPathOrFallback();
        if (!texturePath.Contains(texPrefix) || texturePath.EndsWith(texPrefix))
        {
            Debug.LogError("Texture Asset Path not found!!!");
            return;
        }
        string atlasName = Path.GetFileNameWithoutExtension(texturePath);
        string fullAtlasPath = AppDataWithoutAssetPath + atlasPrefix + "/" + atlasName;
        string tpsfile = fullAtlasPath + "/" + atlasName + ".tps";

        if (Directory.Exists(fullAtlasPath))
        {
            PublishAtlas(tpsfile);
            AssetDatabase.Refresh();
            return;
        }
        Directory.CreateDirectory(fullAtlasPath);

        string templatePath = AppDataPath + "/Templates/TPAtlas.tps";
        if (!File.Exists(templatePath))
        {
            return;
        }
        ParseTPSFile(templatePath, tpsfile, atlasName, texturePath);     //分析TPS文件
        PublishAtlas(tpsfile);
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 分析处理tps文件
    /// </summary>
    static void ParseTPSFile(string templatePath, string tpsfile, string atlasName, string texturePath)
    {
        var size = GetAtlasSize(atlasName);
        var content = File.ReadAllText(templatePath);
        content = content.Replace("[TEX_WIDTH]", size.x.ToString());
        content = content.Replace("[TEX_HEIGHT]", size.y.ToString());
        content = content.Replace("[TPS_FILE]", tpsfile);
        content = content.Replace("[DATA_PATH]", atlasName + ".tpsheet");
        content = content.Replace("[FILE_LIST]", texturePath.Replace("Assets", AppDataPath));
        File.WriteAllText(tpsfile, content);
    }

    static Vector2Int GetAtlasSize(string atlasName)
    {
        var size = new Vector2Int(texWidth, texHeight);
        var list = gameSettings.atlasSettings;
        if (list != null)
        {
            foreach(var item in list)
            {
                var itemName = Path.GetFileNameWithoutExtension(item.assetPath);
                if (itemName == atlasName)
                {
                    switch(item.textureSize)
                    {
                        case TextureSize.MAX_1024:
                            size.x = size.y = 1024;
                        break;
                        case TextureSize.MAX_2048:
                            size.x = size.y = 2048;
                        break;
                        case TextureSize.MAX_4096:
                            size.x = size.y = 4096;
                        break;
                    }
                }
            }
        }
        return size;
    }

    static void PublishAtlas(string tpsfile)
    {
        string tpath = string.Empty;
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            tpath = gameSettings.texturePackerPath + "\\TexturePacker.exe";
        }
        else  if (Application.platform == RuntimePlatform.OSXEditor)
        {
            //Mac 下的默认安装目录是 /Applications
            tpath = gameSettings.texturePackerPath + "/TexturePacker.app/Contents/MacOS/TexturePacker";
        }
       
        try
        {
            ExecuteProc(tpath, tpsfile);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    /// <summary>
    /// 创建材质
    /// </summary>
    /// <param name="tpsPath"></param>
    static void CreateMaterial(string tpsPath, AtlasType type)
    {
        string typeName = type.ToString();
        var materialPath = tpsPath.Replace(".tps", ".mat");
        string path = AppDataPath + "/Templates/Materials/" + typeName + "Material.mat";
        string name = Path.GetFileNameWithoutExtension(materialPath);
        File.Copy(path, materialPath, true);
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 压缩纹理
    /// </summary>
    /// <param name="tpsPath"></param>
    static void CompressTexture(string tpsPath)
    {
        string aPath = tpsPath.Replace(AppDataPath, "Assets").Replace(".tps", ".png");
        TextureImporter ti = UnityEditor.AssetImporter.GetAtPath(aPath) as TextureImporter;
        if (ti == null)
        {
            return;
        }
        ti.textureType = TextureImporterType.Default;

        TextureImporterSettings settings = new TextureImporterSettings();
        ti.ReadTextureSettings(settings);

        settings.readable = false;
        settings.mipmapEnabled = false;
        settings.maxTextureSize = 1024;
        settings.wrapMode = TextureWrapMode.Clamp;
        settings.npotScale = TextureImporterNPOTScale.None;

        settings.textureFormat = TextureImporterFormat.ETC2_RGBA8;
        settings.filterMode = FilterMode.Trilinear;
        settings.spriteMode = (int)SpriteImportMode.Multiple;

        settings.aniso = 4;
        settings.alphaIsTransparency = false;
        ti.SetTextureSettings(settings);
        AssetDatabase.ImportAsset(aPath, ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 创建图集跟透明通道
    /// </summary>
    /// <param name="atlasName"></param>
    /// <param name="packedPath"></param>
    /// <param name="texturePath"></param>
    static void CreateAtlasAndAlpha(string atlasName, string packedPath, string texturePath)
    {
        var tpexe = gameSettings.texturePackerPath;
        string main = "--format unity-texture2d " +
                    "--max-width 1024 " +
                    "--max-height 1024 " +
                    "--multipack " +
                    "--trim-mode None " +
                    "--disable-rotation " +
                    "--size-constraints POT " +
                    "--opt RGB888 " +
                    "--sheet " + packedPath + "/" + atlasName + ".png " +
                    "--data " + packedPath + "/" + atlasName + ".tpsheet " + texturePath;
        //ExecCommand(tpexe, main, packedPath);

        string mask = "--format unity-texture2d " +
                    "--max-width 1024 " +
                    "--max-height 1024 " +
                    "--multipack " +
                    "--trim-mode None " +
                    "--disable-rotation " +
                    "--size-constraints POT " +
                    "--opt ALPHA " +
                    "--sheet " + packedPath + "/" + atlasName + "_A.png " +
                    "--data " + packedPath + "/" + atlasName + "_A.tpsheet " + texturePath;
        //ExecCommand(tpexe, mask, packedPath);
        AssetDatabase.Refresh();
    }
}

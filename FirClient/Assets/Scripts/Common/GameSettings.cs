using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public enum TextureSize
{
    MAX_1024,
    MAX_2048,
    MAX_4096,
}

[Serializable]
public class TextureCompressInfo
{
    public string assetPath;
#if UNITY_EDITOR
    public TextureImporterFormat iosFormat;
    public TextureImporterFormat androidFormat;
#endif
    public TextureSize textureSize = TextureSize.MAX_1024;
    public bool isDynamic = false;
}

public enum PackType
{
    PackSingleFile,             //单个打包
    PackMultiFile,              //多个打包
    PackMultiDirectory,         //打包多目录
}

public enum PackCompressType
{
    PackCompressDirectory,      //压缩打包
}

[Serializable]
public class AssetBundlePackInfo
{
    public string assetPath;
    public PackType packType;
    public string fileExtName;
}

[Serializable]
public class DataBundlePackInfo
{
    public string dataPath;
    public PackCompressType packType;
    public string fileExtName;
}

public class GameSettings : ScriptableObject
{
    [Tooltip("游戏的调试模式")]
    public bool debugMode;

    [Tooltip("游戏的日志模式")]
    public bool logMode;

    [Tooltip("Lua的字节码模式")]
    public bool luaByteMode;

    [Tooltip("AStar的调试模式")]
    public bool aStarDebugMode;

    [Tooltip("UI图集的路径")]
    public string uiAtlasPath;

    [Tooltip("UI图集的纹理路径")]
    public string uiTexturePath;

    [Tooltip("TexturePacker的exe安装路径")]
    public string texturePackerPath;

    [Tooltip("游戏的运行帧频，默认30帧")]
    public int GameFrameRate;                        //游戏帧频

    [SerializeField][HideInInspector]
    private int selectedAtlasIndex = -1;
    [SerializeField][HideInInspector]
    public List<TextureCompressInfo> atlasSettings;

    [SerializeField][HideInInspector]
    private int selectedABIndex = -1;
    [SerializeField][HideInInspector]
    public List<AssetBundlePackInfo> assetBundlePackSetting;

    [SerializeField][HideInInspector]
    private int selectedDatasIndex = -1;
    [SerializeField][HideInInspector]
    public List<DataBundlePackInfo> datasBundlePackSetting;
}

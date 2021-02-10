using FirClient.Define;
using UnityEngine;

public class AppConst
{
    public static bool DebugMode = false;                       //调试模式-用于内部测试
    public static bool LogMode = false;                         //日志模式
    public static bool UpdateMode = false;                      //更新模式
    public static bool NetworkMode = false;                     //网络模式
    public static bool LuaByteMode = false;                     //Lua字节码模式-默认关闭 
    public static bool ShowFps = false;                         //显示帧频
    public static AppState AppState = AppState.None;            //APP的状态

    public static int GameFrameRate = 30;                       //帧频数
    public const uint BatchProcCount = 5;                       //ZIP批处理次数
    public const int NetMessagePoolMax = 100;

    public const int DefaultSortLayer = 0;                      //默认层渲染排序值
    public const int RoleSortLayer = 3;                         //角色层渲染排序值
    public const int BattleTempSortingOrder = 100;              //战斗时临时排序

    public const string AppName = "FirSango";                   //应用程序名称
    public const string AppPrefix = AppName + "_";              //应用程序前缀
    public const string ExtName = ".unity3d";                   //素材扩展名
    public const string LuaTempDir = "LuaTemp/";                //临时目录
    public const string ABDir = "Assets/StreamingAssets/res";
    public const string ResIndexFile = "res";
    public const string GameSettingName = "GameSettings";       //游戏设置
    public const string ResUrl = "https://sango-1251004655.cos.ap-chengdu.myqcloud.com/";
    public const string PatchUrl = ResUrl +"patchs/";           //测试更新地址

    public const string SocketAddress = "127.0.0.1";            //Socket服务器地址
    public const ushort SocketPort = 15940;                      //Socket服务器端口

    public static string TablePath = Application.dataPath + "/res/Tables/";
    public static string[] DataPrefixs = {"datas_", "scripts_", "tables_"};
    public static string[] AssetPaths = { "/res/Datas/", "/res/Tables/", "/StreamingAssets/res/" };

    public static readonly WaitForSeconds WaitForSeconds_01 = new WaitForSeconds(0.1f);
    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
}
namespace FirClient.Define
{
    public enum ResPlaceType : byte
    {
        StreamAsset = 0,    //包内路径
        DataDisk = 1,       //数据磁盘
    }

    public enum AppState
    {
        None,
        IsPlaying,
        Exiting,
    }

    public class VersionInfo
    {
        public string mainVersion;         //大版本号
        public string primaryVersion;      //主要版本号
        public string patchVersion;        //补丁版本号

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}", mainVersion, primaryVersion, patchVersion);
        }
    }

    public class DownloadText
    {
        public string text;
    }
}
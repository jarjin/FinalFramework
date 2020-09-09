using Mono.Xml;
using System.Security;

public class XmlHelper : BaseBehaviour
{
    public static SecurityElement LoadXml(string xmlPath)
    {
        SecurityParser sp = new SecurityParser();
        var data = resMgr.LoadLocalAsset<string>(xmlPath);
        sp.LoadXml(data.ToString());
        return sp.ToXml();
    }
}

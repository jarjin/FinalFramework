using Mono.Xml;
using System.Security;
using System.IO;

namespace FirCommon.Utility
{
    public class XmlHelper
    {
        public static SecurityElement LoadXml(string xmlPath)
        {
            xmlPath = AppUtil.CurrDirectory + xmlPath;
            if (!xmlPath.EndsWith(".xml"))
            {
                xmlPath += ".xml";
            }
            var sp = new SecurityParser();
            var data = File.ReadAllText(xmlPath);
            sp.LoadXml(data.ToString());
            return sp.ToXml();
        }
    }
}

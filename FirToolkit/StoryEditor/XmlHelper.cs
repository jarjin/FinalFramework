using Mono.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace StoryEditor
{
    public class XmlHelper
    {
        public static SecurityElement LoadXml(string xmlPath)
        {
            if (File.Exists(xmlPath))
            {
                SecurityParser sp = new SecurityParser();
                var data = File.ReadAllText(xmlPath);
                sp.LoadXml(data.ToString());
                return sp.ToXml();
            }
            return null;
        }
    }
}

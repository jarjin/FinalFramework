using System.IO;
using System.Net;
using System.Text;

namespace FirCommon.Utility
{
    public class HttpHelper
    {
        /// <summary>
        /// 获取网页html源文件
        /// </summary>
        public static string ReqHttp(string url, string encoding = "utf-8")
        {
            HttpWebResponse res = null;
            string strResult = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                //req.Method = "POST";
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Accept = "text/Html,application/xhtml+XML,application/xml;q=0.9,*/*;q=0.8";
                req.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.2; zh-CN; rv:1.9.2.8) Gecko/20100722 Firefox/3.6.8";
                res = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding(encoding));
                strResult = reader.ReadToEnd();
                reader.Close();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }
    }
}

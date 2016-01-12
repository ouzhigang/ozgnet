using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Drawing;
using System.Xml;
using System.IO;
using System.Net.NetworkInformation;

namespace OZGNet.Net
{
    /// <summary>
    /// 网络工具类
    /// </summary>
    public class Utility
    {
        //伪装WindowsXP下的IE6
        private static string Browser = BrowserUserAgentString.WindowsNT51_IE6;

        #region 获取一个来自网络的图片
        /// <summary>
        /// 获取一个来自网络的图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Bitmap GetUrlBitmap(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            
            //伪装浏览器
            request.UserAgent = Browser;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (System.IO.Stream s = response.GetResponseStream())
            {
                Bitmap bmp = (Bitmap)Bitmap.FromStream(s);
                return bmp;
            }
        }
        #endregion

        #region POST方式提交表单
        /// <summary>
        /// POST方式提交表单
        /// </summary>
        /// <param name="postData">提交数据,（name=name＆pwd=pwd）</param>
        /// <param name="postUrl">提交路径，（http://127.0.0.1/Default.aspx）</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string FormPOST(string postData, string postUrl, string encoding)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);

            //伪装浏览器
            request.UserAgent = Browser;

            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postData.Length;
            StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.GetEncoding(encoding));
            writer.Write(postData);
            writer.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
            string tmp = reader.ReadToEnd();
            response.Close();
            reader.Close();
            reader.Dispose();
            tmp = tmp.Replace("\r", "");
            tmp = tmp.Replace("\n", "");
            tmp = tmp.Replace("\t", "");
            tmp = tmp.Trim();
            return tmp;
        }
        #endregion

        #region Get方式提交表单
        /// <summary>
        /// Get方式提交表单
        /// </summary>
        /// <param name="getUrl">提交路径,（http://127.0.0.1/Default.aspx?name=name＆pwd=pwd）</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string FormGET(string getUrl, string encoding)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getUrl);

            //伪装浏览器
            request.UserAgent = Browser;

            request.Method = WebRequestMethods.Http.Get;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
            string tmp = reader.ReadToEnd();
            response.Close();
            reader.Close();
            reader.Dispose();
            tmp = tmp.Replace("\r", "");
            tmp = tmp.Replace("\n", "");
            tmp = tmp.Replace("\t", "");
            tmp = tmp.Trim();
            return tmp;
        }
        #endregion

        #region 获取URL的Cookies
        /// <summary>
        /// 获取URL的Cookies
        /// </summary>
        /// <param name="url">目标URL</param>
        /// <returns></returns>
        public static CookieCollection GetNetCookies(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            //伪装浏览器
            request.UserAgent = Browser;

            request.CookieContainer = new CookieContainer();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return response.Cookies;
        }
        #endregion

        #region 返回一个url的xmldoc
        /// <summary>
        /// 返回一个url的xmldoc
        /// </summary>
        /// <param name="url">目标url</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static XmlDocument GetUrlXmlDoc(string url, string encoding)
        {
            string content = FormGET(url, encoding);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content);
            return doc;
        }
        #endregion

        #region 返回一个url的xmldoc
        /// <summary>
        /// 返回一个url的xmldoc
        /// </summary>
        /// <param name="postUrl">目标url</param>
        /// <returns></returns>
        public static XmlDocument GetUrlXmlDoc(string postUrl)
        {
            return GetUrlXmlDoc(postUrl, "gb2312");
        }
        #endregion

        #region 获取本机的Dns地址
        /// <summary>
        /// 获取本机的Dns地址
        /// </summary>
        /// <returns></returns>
        public static IList<IPAddress> GetDnsAddresses()
        {
            IList<IPAddress> DnsAddresses = new List<IPAddress>();
            NetworkInterface[] t_interface = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface e_interface in t_interface)
            {
                IPInterfaceProperties e_property = e_interface.GetIPProperties();
                IPAddressCollection e_collection = e_property.DnsAddresses;
                foreach (IPAddress dns in e_collection)
                {
                    DnsAddresses.Add(dns);
                }
            }
            return DnsAddresses;
        } 
        #endregion

        #region 获取FTP文件的大小,文件不存在则返回0
        /// <summary>
        /// 获取FTP文件的大小,文件不存在则返回0
        /// </summary>
        /// <param name="serverFile">FTP服务器文件</param>
        /// <param name="userName">用户名</param>
        /// <param name="userPwd">密码</param>
        /// <param name="KeepAlive">执行完毕是否关闭控制连接</param>
        /// <returns></returns>
        public static long GetFtpFileSize(string serverFile, string userName, string userPwd, bool KeepAlive)
        {
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(serverFile);
            req.Method = WebRequestMethods.Ftp.GetFileSize;
            req.Credentials = new NetworkCredential(userName, userPwd);
            long servLength = 0;
            try
            {
                FtpWebResponse res = (FtpWebResponse)req.GetResponse();
                servLength = res.ContentLength;
                res.GetResponseStream().Close();
                res.Close();
                req.KeepAlive = KeepAlive;
            }
            catch { }
            return servLength;
        }
        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Xml;

namespace OZGNet.Net
{
    /// <summary>
    /// 获取Alexa排名数据
    /// </summary>
    public class Alexa
    {
        /// <summary>
        /// 获取目标域名的Alexa排名数据(返回字符)
        /// </summary>
        /// <param name="host">目标域名</param>
        /// <returns></returns>
        public static string GetString(string host)
        {
            WebClient wc = null;
            string str = null;
            try
            {
                wc = new WebClient();
                wc.Encoding = System.Text.Encoding.UTF8;
                str = wc.DownloadString("http://data.alexa.com/data/?cli=10&dat=snba&ver=7.0&url=" + host);
            }
            catch { }
            finally
            {
                wc.Dispose();
            }
            return str;
        }

        /// <summary>
        /// 获取目标域名的Alexa排名数据(返回Xml)
        /// </summary>
        /// <param name="host">目标域名</param>
        /// <returns></returns>
        public static XmlDocument GetXmlDoc(string host)
        {
            string str = GetString(host);
            if (str != null)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(str);
                return doc;
            }
            return null;
        }

        /// <summary>
        /// 获取目标域名的Alexa排名数据(返回一个已经定义好的table)
        /// </summary>
        /// <param name="host">目标域名</param>
        /// <returns></returns>
        public static string GetHtml(string host)
        {
            StringBuilder sb = new StringBuilder();
            XmlDocument doc = GetXmlDoc(host);
            if (doc != null)
            {
                sb.Append("<table>");
                sb.Append("<tr><td>网站(" + host + ") Alexa 排名数据结果如下:</td></tr>");
                XmlNode node = doc.GetElementsByTagName("POPULARITY")[0];
                if (node != null)
                {
                    sb.Append("<tr><td>全球排名: <strong>" + node.Attributes[1].Value + "</strong></td></tr>");
                }
                XmlNode nodeRank = doc.GetElementsByTagName("LINKSIN")[0];
                if (nodeRank != null)
                {
                    sb.Append("<tr><td>外部链接: <strong>" + nodeRank.Attributes[0].Value + "个站点</strong></td></tr>");
                }
                sb.Append("</table>");
            }
            return sb.ToString();
        }

    }
}

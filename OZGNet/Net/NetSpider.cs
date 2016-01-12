using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;

namespace OZGNet.Net
{
    /// <summary>
    /// 网络蜘蛛帮助类
    /// </summary>
    public class NetSpider
    {
        #region 过滤Html标签
        /// <summary>
        /// 过滤Script
        /// </summary>
        /// <param name="str">内容</param>
        /// <returns></returns>
        public static string FilterScript(string str)
        {
            return FilterCustomTag(str, "Script");
        }
        /// <summary>
        /// 过滤IFrame
        /// </summary>
        /// <param name="str">内容</param>
        /// <returns></returns>
        public static string FilterIFrame(string str)
        {
            return FilterCustomTag(str, "IFrame");
        }
        /// <summary>
        /// 过滤Object
        /// </summary>
        /// <param name="str">内容</param>
        /// <returns></returns>
        public static string FilterObject(string str)
        {
            return FilterCustomTag(str, "Object");
        }
        /// <summary>
        /// 过滤Applet
        /// </summary>
        /// <param name="str">内容</param>
        /// <returns></returns>
        public static string FilterApplet(string str)
        {
            return FilterCustomTag(str, "Applet");
        }
        /// <summary>
        /// 过滤Div
        /// </summary>
        /// <param name="str">内容</param>
        /// <returns></returns>
        public static string FilterDiv(string str)
        {
            return FilterCustomTag(str, "Div");
        }
        /// <summary>
        /// 过滤Font
        /// </summary>
        /// <param name="str">内容</param>
        /// <returns></returns>
        public static string FilterFont(string str)
        {
            return FilterCustomTag(str, "Font");
        }
        /// <summary>
        /// 过滤Span
        /// </summary>
        /// <param name="str">内容</param>
        /// <returns></returns>
        public static string FilterSpan(string str)
        {
            return FilterCustomTag(str, "Span");
        }
        /// <summary>
        /// 过滤A
        /// </summary>
        /// <param name="str">内容</param>
        /// <returns></returns>
        public static string FilterA(string str)
        {
            return FilterCustomTag(str, "A");
        }
        /// <summary>
        /// 过滤Img
        /// </summary>
        /// <param name="str">内容</param>
        /// <returns></returns>
        public static string FilterImg(string str)
        {
            return FilterCustomTag(str, "Img");
        }
        /// <summary>
        /// 过滤Form
        /// </summary>
        /// <param name="str">内容</param>
        /// <returns></returns>
        public static string FilterForm(string str)
        {
            return FilterCustomTag(str, "Form");
        }
        /// <summary>
        /// 过滤Form
        /// </summary>
        /// <param name="str">内容</param>
        /// <returns></returns>
        public static string FilterHtml(string str)
        {
            return FilterCustomTag(str, "Html");
        }
        /// <summary>
        /// 过滤自定义HTML标签
        /// </summary>
        /// <param name="str">内容</param>
        /// <param name="tag">HTML标签</param>
        /// <returns></returns>
        public static string FilterCustomTag(string str, string tag)
        {
            return Filter(str, @"<" + tag + "[^>]*?>.*?</" + tag + ">");
        }
        /// <summary>
        /// 过滤内容的私有方法
        /// </summary>
        /// <param name="str">内容</param>
        /// <param name="reg">正则</param>
        /// <returns></returns>
        protected static string Filter(string str, string reg)
        {
            str = Regex.Replace(str, reg, "", RegexOptions.IgnoreCase);
            return str;
        }
        #endregion

        /// <summary>
        /// 取Html(标签)的开始到结束部分(支持正则)
        /// </summary>
        /// <param name="html">html内容</param>
        /// <param name="startTag">开始</param>
        /// <param name="endTag">结束</param>
        /// <returns></returns>
        public static string GetStartToEndTag(string html, string startTag, string endTag)
        {
            //实例
            //content <a href='#' class='abc'>123</a>456
            //start <a href='#'[^>]*?>
            //end </a>
            //return 123
            
            return OZGNet.Utility.GetStartToEndString(html, startTag, endTag);
        }

        /// <summary>
        /// 取字符(标签)的开始到结束部分
        /// </summary>
        /// <param name="html">html内容</param>
        /// <param name="startTag">开始（如：<div class=\"article_title_nei\">）</param>
        /// <param name="endTag">结束（如：</div>）</param>
        /// <returns></returns>
        public static string GetStartToEndTag2(string html, string startTag, string endTag)
        {
            //实例
            //content <a href='#' class='abc'>123</a>456
            //start <a href='#' (不要输后面的">")
            //end </a>
            //return 123

            //原来的版本
            //startTag = startTag + "[^>]*?>";
            //return GetStartToEndTag(html, startTag, endTag);


            Regex r = new Regex("(?<=(" + startTag + ")).*?(?=" + endTag + ")", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return r.Match(html).Value;
        }

        #region (私有方法)取HTML内容里面的所有href,src,action中之一
        /// <summary>
        /// 取HTML内容里面的所有href,src,action中之一
        /// </summary>
        /// <param name="html">html内容</param>
        /// <param name="Attribute">href,src,action中之一</param>
        /// <returns></returns>
        protected static List<string> GetLinkList(string html, string Attribute)
        {
            return GetLinkList(html, Attribute, true);
        }
        /// <summary>
        /// 取HTML内容里面的所有href,src,action中之一
        /// </summary>
        /// <param name="html">html内容</param>
        /// <param name="Attribute">href,src,action中之一</param>
        /// <param name="toLower">是否转化为小写</param>
        /// <returns></returns>        
        protected static List<string> GetLinkList(string html, string Attribute, bool toLower)
        {
            if (toLower)
            {
                //转小写
                List<string> list = new List<string>();
                string strRef = @"(href|src|action)[ ]*=[ ]*[""'][^""'#>]+[""']";
                MatchCollection matches = new Regex(strRef).Matches(html.ToLower());
                foreach (Match m in matches)
                {
                    string item = m.Groups[0].Value;
                    string attribute = item.Split('=')[0];
                    if (attribute == Attribute.ToLower())
                    {
                        //这个判断是用来防止出现这样 href='javascript:void(0)'
                        if (!item.ToLower().Contains("javascript:"))
                        {
                            item = item.Replace(attribute, "");
                            item = item.Replace("'", "");
                            item = item.Replace("\"", "");
                            item = item.Substring(1, item.Length - 1);
                            list.Add(item);
                        }
                    }
                }
                return list;
            }
            else
            {
                List<string> list = new List<string>();
                string strRef = @"(href|src|action)[ ]*=[ ]*[""'][^""'#>]+[""']";
                MatchCollection matches = new Regex(strRef).Matches(html);
                foreach (Match m in matches)
                {
                    string item = m.Groups[0].Value;
                    string attribute = item.Split('=')[0];
                    if (attribute == Attribute)
                    {
                        //这个判断是用来防止出现这样 href='javascript:void(0)'
                        if (!item.Contains("javascript:"))
                        {
                            item = item.Replace(attribute, "");
                            item = item.Replace("'", "");
                            item = item.Replace("\"", "");
                            item = item.Substring(1, item.Length - 1);
                            list.Add(item);
                        }
                    }
                }
                return list;
            }
        } 
        #endregion

        /// <summary>
        /// 获取html内容里面的所有href属性
        /// </summary>
        /// <param name="html">html内容</param>
        /// <returns></returns>
        public static List<string> GetHrefList(string html)
        {
            return GetLinkList(html, "href");
        }

        /// <summary>
        /// 获取html内容里面的所有href属性
        /// </summary>
        /// <param name="html">html内容</param>
        /// <param name="toLower">是否转化为小写</param>
        /// <returns></returns>
        public static List<string> GetHrefList(string html, bool toLower)
        {
            return GetLinkList(html, "href", toLower);
        }

        /// <summary>
        /// 获取html内容里面的所有src属性
        /// </summary>
        /// <param name="html">html内容</param>
        /// <returns></returns>
        public static List<string> GetSrcList(string html)
        {
            return GetLinkList(html, "src");
        }

        /// <summary>
        /// 获取html内容里面的所有src属性
        /// </summary>
        /// <param name="html">html内容</param>
        /// <param name="toLower">是否转化为小写</param>
        /// <returns></returns>
        public static List<string> GetSrcList(string html, bool toLower)
        {
            return GetLinkList(html, "src", toLower);
        }

        /// <summary>
        /// 获取html内容里面的所有(form里面的)action属性
        /// </summary>
        /// <param name="html">html内容</param>
        /// <returns></returns>
        public static List<string> GetActionList(string html)
        {
            return GetLinkList(html, "action");
        }

        /// <summary>
        /// 获取html内容里面的所有(form里面的)action属性
        /// </summary>
        /// <param name="html">html内容</param>
        /// <param name="toLower">是否转化为小写</param>
        /// <returns></returns>
        public static List<string> GetActionList(string html, bool toLower)
        {
            return GetLinkList(html, "action", toLower);
        }        

        /// <summary>
        /// 返回目标url内容,并过滤style和script
        /// </summary>
        /// <param name="url">目标url</param>
        /// <returns></returns>
        public static string GetUrlContent(string url)
        {
            List<string> FilterList = new List<string>();
            FilterList.Add("style");
            FilterList.Add("script");
            return GetUrlContent(url, FilterList);
        }

        /// <summary>
        /// 返回目标url内容
        /// </summary>
        /// <param name="url">目标url</param>
        /// <param name="FilterList">需要过滤的html标签</param>
        /// <returns></returns>
        public static string GetUrlContent(string url, List<string> FilterList)
        {
            return GetUrlContent(url, FilterList, "gb2312");
        }

        /// <summary>
        /// 返回目标url内容
        /// </summary>
        /// <param name="url">目标url</param>
        /// <param name="FilterList">需要过滤的html标签</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string GetUrlContent(string url, List<string> FilterList, string encoding)
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.GetEncoding(encoding);
            string html = client.DownloadString(url);            
            if (FilterList != null)
            {
                foreach (string FilterString in FilterList)
                {
                    html = FilterCustomTag(html, FilterString);
                }
            }
            html = html.Replace("\r", "");
            html = html.Replace("\n", "");
            html = html.Replace("\t", "");
            html = html.Trim();
            return html;
        }

        

    }
}

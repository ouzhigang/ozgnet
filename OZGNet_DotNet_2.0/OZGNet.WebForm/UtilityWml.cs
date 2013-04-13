using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;

namespace OZGNet.WebForm
{
    /// <summary>
    /// Wml工具类
    /// </summary>
    public class UtilityWml
    {
        #region 输出Wml文件尾
        /// <summary>
        /// 结束WML输出
        /// </summary>
        public static void WmlFoot()
        {
            WmlFoot(HttpContext.Current);
        }
        /// <summary>
        /// 结束WML输出
        /// </summary>
        /// <param name="context">HttpContext实例</param>
        public static void WmlFoot(HttpContext context)
        {
            context.Response.Write("</wml>\r\n");
        }
        #endregion

        #region 输出Wml文件头
        /// <summary>
        /// 输出WML头部内容，编码为utf-8
        /// </summary>
        public static void WmlHead()
        {
            WmlHead(HttpContext.Current, "utf-8");
        }
        /// <summary>
        /// 输出WML头部内容
        /// </summary>
        /// <param name="encoding">编码</param>
        public static void WmlHead(string encoding)
        {
            WmlHead(HttpContext.Current, encoding);
        }
        /// <summary>
        /// 输出WML头部内容，编码为utf-8
        /// </summary>
        /// <param name="context">HttpContext实例</param>
        public static void WmlHead(HttpContext context)
        {
            WmlHead(context, "utf-8");
        }
        /// <summary>
        /// 输出WML头部内容
        /// </summary>
        /// <param name="context">HttpContext实例</param>
        /// <param name="encoding">编码</param>
        public static void WmlHead(HttpContext context, string encoding)
        {
            context.Response.ContentType = "text/vnd.wap.wml";
            context.Response.Charset = encoding;
            context.Response.Write("<?xml version=\"1.0\" encoding=\"" + encoding + "\" ?>\r\n");
            context.Response.Write("<!DOCTYPE wml PUBLIC \"-//WAPFORUM//DTD WML 1.1//EN\" \"http://www.wapforum.org/DTD/wml_1.1.xml\">\r\n");
            context.Response.Write("<wml>\r\n");
        }
        #endregion

    }
}

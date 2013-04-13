using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;

namespace OZGNet.WebForm.ThirdPart.DsJian
{
    /// <summary>
    /// IP处理
    /// </summary>
    public static class HttpDeal
    {
        #region IP处理

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            if (HttpContext.Current == null) return "localhost";
            HttpRequest request = HttpContext.Current.Request;
            if (request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
            {
                return request.ServerVariables["Remote_Addr"];
            }
            return request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        }

        /// <summary>
        /// 获取截断的IP字符串
        /// </summary>
        /// <returns></returns>
        public static string GetDimClientIP()
        {
            return GetDimClientIP(2);
        }

        /// <summary>
        /// 获取截断的IP字符串，指定IP
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetDimClientIP(String ip)
        {
            return GetDimClientIP(2, ip);
        }

        /// <summary>
        /// 获取截断的IP字符串
        /// </summary>
        /// <returns></returns>
        public static string GetDimClientIP(int length)
        {
            return GetDimClientIP(length, GetClientIP());
        }

        /// <summary>
        /// 获取截断的IP字符串
        /// </summary>
        /// <returns></returns>
        public static string GetDimClientIP(int length, String ip)
        {
            string[] textArray = ip.Split(new char[] { '.' });
            for (int i = textArray.Length - 1; length > 0; i--)
            {
                length--;
                textArray[i] = "*";
            }
            StringBuilder builder = new StringBuilder();
            for (int j = 0; j < textArray.Length; j++)
            {
                builder.Append(textArray[j]);
                builder.Append('.');
            }
            return builder.ToString().TrimEnd(new char[] { '.' });
        }

        /// <summary>
        /// 获取截断的IP字符串段
        /// </summary>
        /// <returns></returns>
        public static string GetClientIPSect(int length)
        {
            String ip = GetClientIP();
            String resultIp = "";
            string[] textArray = ip.Split(new char[] { '.' });
            for (int i = 0; length > 0; i++)
            {
                length--;
                resultIp += textArray[i] + ".";
            }

            return resultIp.TrimEnd(new char[] { '.' });
        }

        #endregion

        

    }
}

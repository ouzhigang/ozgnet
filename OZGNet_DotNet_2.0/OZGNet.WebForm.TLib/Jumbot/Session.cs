using System;
using System.Web;
using System.Text;

namespace OZGNet.WebForm.ThirdPart.Jumbot
{
    /// <summary>
    /// Session操作类
    /// </summary>
    public static class Session
    {
        /// <summary>
        /// 添加Session，调动有效期为20分钟
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="objValue">Session值</param>
        public static void Add(string strSessionName, object objValue)
        {
            HttpContext.Current.Session[strSessionName] = objValue;
            HttpContext.Current.Session.Timeout = 20;
        }
        /// <summary>
        /// 添加Session，调动有效期为20分钟
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="objValues">Session值数组</param>
        public static void Adds(string strSessionName, object[] objValues)
        {
            HttpContext.Current.Session[strSessionName] = objValues;
            HttpContext.Current.Session.Timeout = 20;
        }
        /// <summary>
        /// 添加Session
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="objValue">Session值</param>
        /// <param name="iExpires">调动有效期（分钟）</param>
        public static void Add(string strSessionName, object objValue, int iExpires)
        {
            HttpContext.Current.Session[strSessionName] = objValue;
            HttpContext.Current.Session.Timeout = iExpires;
        }
        /// <summary>
        /// 添加Session
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="objValues">Session值数组</param>
        /// <param name="iExpires">调动有效期（分钟）</param>
        public static void Adds(string strSessionName, object[] objValues, int iExpires)
        {
            HttpContext.Current.Session[strSessionName] = objValues;
            HttpContext.Current.Session.Timeout = iExpires;
        }
        /// <summary>
        /// 读取某个Session对象值
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <returns>Session对象值</returns>
        public static object Get(string strSessionName)
        {
            if (HttpContext.Current.Session[strSessionName] == null)
            {
                return null;
            }
            else
            {
                return HttpContext.Current.Session[strSessionName];
            }
        }
        /// <summary>
        /// 读取某个Session对象值数组
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <returns>Session对象值数组</returns>
        public static object[] Gets(string strSessionName)
        {
            if (HttpContext.Current.Session[strSessionName] == null)
            {
                return null;
            }
            else
            {
                return (object[])HttpContext.Current.Session[strSessionName];
            }
        }
        /// <summary>
        /// 删除某个Session对象
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        public static void Del(string strSessionName)
        {
            HttpContext.Current.Session[strSessionName] = null;
        }
    }
}

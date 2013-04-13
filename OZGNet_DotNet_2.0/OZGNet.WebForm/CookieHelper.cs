using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace OZGNet.WebForm
{
    /// <summary>
    /// Cookie帮助类
    /// </summary>
    public class CookieHelper
    {
        #region 判断该Cookie是否存在
        /// <summary>
        /// 判断该Cookie是否存在
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <returns>存在返回true</returns>
        public static bool ExistsCookie(string name)
        {
            return GetCookie(name) != null ? true : false;
        } 
        #endregion
        #region 写入Cookie
        /// <summary>
        /// 写入Cookie
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <param name="value">Cookie值</param>
        /// <returns></returns>
        public static bool SetCookie(string name, string value)
        {
            try
            {
                HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[name];
                if (cookie == null)
                {
                    cookie = new HttpCookie(name, value);
                }
                else
                {
                    cookie.Value = value;
                }
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region 写入Cookie
        /// <summary>
        /// 写入Cookie
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <param name="value">Cookie值</param>
        /// <param name="days">天数</param>
        /// <returns></returns>
        public static bool SetCookie(string name, string value, int days)
        {
            try
            {
                HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[name];
                if (cookie == null)
                {
                    cookie = new HttpCookie(name, value);
                }
                else
                {
                    cookie.Value = value;
                }
                cookie.Expires = DateTime.Now.AddDays(days);
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region 写入Cookie(能保存Object)
        /// <summary>
        /// 写入Cookie(能保存Object)
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <param name="value">对象值</param>
        /// <returns></returns>
        public static bool SetCookieSerialize(string name, object value)
        {
            if (value == null)
            {
                return false;
            }
            return SetCookie(name, Utility.ObjectSerializeForString(value));
        } 
        #endregion
        #region 写入Cookie(能保存Object)
        /// <summary>
        /// 写入Cookie(能保存Object)
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <param name="value">对象值</param>
        /// <param name="days">保存时间(天)</param>
        /// <returns></returns>
        public static bool SetCookieSerialize(string name, object value, int days)
        {
            if (value == null)
            {
                return false;
            }
            return SetCookie(name, Utility.ObjectSerializeForString(value), days);
        } 
        #endregion
        #region 获取Cookie
        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <returns></returns>
        public static string GetCookie(string name)
        {
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[name];
            if (cookie != null)
            {
                return cookie.Value;
            }
            else
            {
                return null;
            }
        }
        #endregion
        #region 获取Cookie(对象)
        /// <summary>
        /// 获取Cookie(对象)
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <returns></returns>
        public static object GetCookieSerialize(string name)
        {
            return Utility.ObjectDeserializeForString(GetCookie(name));
        } 
        #endregion
        #region 删除Cookie
        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <returns></returns>
        public static bool DelCookie(string name)
        {
            try
            {
                HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[name];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}

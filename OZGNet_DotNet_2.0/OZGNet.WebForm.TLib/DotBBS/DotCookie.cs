using System;
using System.Web;

namespace OZGNet.WebForm.ThirdPart.DotBBS
{    
    /// <summary>
    /// DotBBS Cookie帮助类
    /// </summary>
    public class DotCookie
    {
        /// <summary>
        /// 添加Cookie
        /// </summary>
        /// <param name="strName">Cookie名称</param>
        /// <param name="strValue">Cookie值</param>
        /// <param name="strMinute">超时时间(分)</param>
        public static void Add(string strName, string strValue, int strMinute)
        {
            try
            {
                HttpCookie cookie = new HttpCookie(DESCrypt.Crypt(strName));
                cookie.Expires = DateTime.Now.AddMinutes((double) strMinute);
                cookie.Value = DESCrypt.Crypt(strValue);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="strName">Cookie名称</param>
        public static void Delete(string strName)
        {
            try
            {
                HttpCookie cookie = new HttpCookie(DESCrypt.Crypt(strName));
                cookie.Expires = DateTime.Now.AddDays(-1.0);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
        /// <summary>
        /// 检查指定Cookie是否存在
        /// </summary>
        /// <param name="strName">Cookie名称</param>
        /// <returns></returns>
        public static bool Exists(string strName)
        {
            bool flag = false;
            if ((Read(strName) != null) && (Read(strName) != ""))
            {
                flag = true;
            }
            return flag;
        }
        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="strName">Cookie名称</param>
        /// <returns></returns>
        public static string Read(string strName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[DESCrypt.Crypt(strName)];
            if (cookie != null)
            {
                return DESCrypt.DeCrypt(cookie.Value);
            }
            return "";
        }
    }
}


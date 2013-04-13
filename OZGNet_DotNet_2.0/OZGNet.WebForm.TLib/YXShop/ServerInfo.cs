using System;
using System.Text.RegularExpressions;
using System.Web;

namespace OZGNet.WebForm.ThirdPart.YXShop
{
    /// <summary>
    /// YXShop 获取服务器相关信息
    /// </summary>
    public class ServerInfo
    {
        /// <summary>
        /// 获取站点根目录
        /// </summary>
        /// <returns></returns>
        public static string GetRootPath()
        {
            string AppPath = "";
            HttpContext HttpCurrent = HttpContext.Current;
            if (HttpCurrent != null)
            {
                return HttpCurrent.Server.MapPath("~");
            }
            AppPath = AppDomain.CurrentDomain.BaseDirectory;
            if (Regex.Match(AppPath, @"\\$", RegexOptions.Compiled).Success)
            {
                AppPath = AppPath.Substring(0, AppPath.Length - 1);
            }
            return AppPath;
        }
        /// <summary>
        /// 获取站点虚拟路径
        /// </summary>
        /// <returns></returns>
        public static string GetRootURI()
        {
            string AppPath = "";
            HttpContext HttpCurrent = HttpContext.Current;
            if (HttpCurrent == null)
            {
                return AppPath;
            }
            HttpRequest Req = HttpCurrent.Request;
            string UrlAuthority = Req.Url.GetLeftPart(UriPartial.Authority);
            if ((Req.ApplicationPath == null) || (Req.ApplicationPath == "/"))
            {
                return UrlAuthority;
            }
            return (UrlAuthority + Req.ApplicationPath);
        }
        /// <summary>
        /// 获取站点虚拟路径
        /// </summary>
        /// <param name="Req">HttpRequest实例</param>
        /// <returns></returns>
        public static string GetRootURI(HttpRequest Req)
        {
            string AppPath = "";
            if (Req == null)
            {
                return AppPath;
            }
            string UrlAuthority = Req.Url.GetLeftPart(UriPartial.Authority);
            if ((Req.ApplicationPath == null) || (Req.ApplicationPath == "/"))
            {
                return UrlAuthority;
            }
            return (UrlAuthority + Req.ApplicationPath);
        }
    }
}


using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Security.Cryptography;
using System.Web.Caching;
using System.Net;
using System.Diagnostics;
using System.Drawing;
using System.Xml;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Threading;

namespace OZGNet.WebForm
{
    /// <summary>
    /// Asp.Net专用工具类
    /// </summary>
    public class Utility : OZGNet.Utility
    {
        #region 判断该Session是否存在
        /// <summary>
        /// 判断该Session是否存在
        /// </summary>
        /// <param name="session">Session名称</param>
        /// <returns></returns>
        public static bool ExistsSession(string name)
        {
            Session.ISession session = new Session.ServerSession();
            return session.Exists(name);
        } 
        #endregion

        #region 判断该Cookie是否存在
        /// <summary>
        /// 判断该Cookie是否存在
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <returns></returns>
        public static bool ExistsCookie(string name)
        {
            Session.ISession session = new Session.CookieSession();
            return session.Exists(name);
        } 
        #endregion

        #region 添加缓存(绝对过期策略)
        /// <summary>
        /// 添加缓存(绝对过期策略)
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="v">过期时间(秒)</param>
        public static void CacheAbsolute(string cacheKey, object cacheValue, double v)
        {
            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey);
            }
            HttpContext.Current.Cache.Insert(cacheKey, cacheValue, null, DateTime.Now.AddSeconds(v), System.Web.Caching.Cache.NoSlidingExpiration);
        } 
        #endregion

        #region 添加缓存(绝对过期策略)
        /// <summary>
        /// 添加缓存(绝对过期策略)
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="dTime">过期时间(dTime减去现在的时间)</param>
        public static void CacheAbsolute(string cacheKey, object cacheValue, DateTime dTime)
        {
            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey);
            }
            HttpContext.Current.Cache.Insert(cacheKey, cacheValue, null, dTime, System.Web.Caching.Cache.NoSlidingExpiration);
        } 
        #endregion

        #region 添加缓存(相对过期策略)
        /// <summary>
        /// 添加缓存(相对过期策略)
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="v">过期时间(秒)</param>
        public static void CacheRelative(string cacheKey, object cacheValue, double v)
        {
            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey);
            }
            HttpContext.Current.Cache.Insert(cacheKey, cacheValue, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(v));
        } 
        #endregion

        #region 添加缓存(相对过期策略)
        /// <summary>
        /// 添加缓存(相对过期策略)
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="dTime">dTime减去现在的时间</param>
        public static void CacheRelative(string cacheKey, object cacheValue, DateTime dTime)
        {
            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey);
            }
            TimeSpan ts = (TimeSpan)(dTime - DateTime.Now);
            HttpContext.Current.Cache.Insert(cacheKey, cacheValue, null, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);
        } 
        #endregion

        #region 添加缓存(文件依赖)
        /// <summary>
        /// 添加缓存(文件依赖)
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="filePath">依赖文件</param>
        public static void CacheFile(string cacheKey, object cacheValue, string filePath)
        {
            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey);
            }
            HttpContext.Current.Cache.Insert(cacheKey, cacheValue, new CacheDependency(HttpContext.Current.Server.MapPath(filePath)));
        } 
        #endregion

        #region 添加缓存(文件依赖)
        /// <summary>
        /// 添加缓存(文件依赖)
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="filePaths">依赖的多个文件</param>
        public static void CacheFile(string cacheKey, object cacheValue, ArrayList filePaths)
        {
            string[] paths = new string[filePaths.Count];
            for (int i = 0; i < filePaths.Count; i++)
            {
                paths[i] = HttpContext.Current.Server.MapPath(filePaths[i].ToString());
            }
            HttpContext.Current.Cache.Insert(cacheKey, cacheValue, new CacheDependency(paths));
        } 
        #endregion

        #region 当前请求文件
        /// <summary>
        /// 当前请求文件
        /// </summary>
        public static string GetCurrentFile()
        {
            return HttpContext.Current.Request.CurrentExecutionFilePath.Substring(HttpContext.Current.Request.CurrentExecutionFilePath.LastIndexOf("/")).Replace("/", "");
        } 
        #endregion

        #region 读取纯真IP数据库
        /// <summary>
        /// 读取纯真IP数据库
        /// </summary>
        /// <param name="ip">目标IP</param>
        /// <returns></returns>
        public static string GetIPData(string ip)
        {
            return OZGNet.Utility.GetIPData(ip, HttpContext.Current.Server.MapPath("~/App_Data/QQWry.Dat"));
        } 
        #endregion

        #region 读取纯真IP数据库
        /// <summary>
        /// 读取纯真IP数据库
        /// </summary>
        /// <param name="ip">目标IP</param>
        /// <param name="filePath">数据库路径(相对路径)</param>
        /// <returns></returns>
        public static string GetIPData(string ip, string filePath)
        {
            return OZGNet.Utility.GetIPData(ip, HttpContext.Current.Server.MapPath(filePath));
        } 
        #endregion

        #region 过滤html标签
        /// <summary>
        /// 过滤html标签
        /// </summary>
        /// <param name="str">目标字符</param>
        /// <returns></returns>
        public static string ReplaceHtml(string str)
        {
            string Str = System.Text.RegularExpressions.Regex.Replace(str, "<[^>]+>", "");
            Str = System.Text.RegularExpressions.Regex.Replace(Str, "<br>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            return Str;
        } 
        #endregion

        #region 过滤空格换行或＆nbsp;之类的字符
        /// <summary>
        /// 过滤空格换行或＆nbsp;之类的字符
        /// </summary>
        /// <param name="str">目标字符串</param>
        /// <returns>返回过滤后的字符</returns>
        public static string ReplaceSpace(string str)
        {
            return str.Replace("&nbsp;", "").Replace("\r", "").Replace("\n", "").Replace(" ", "").Replace("&quot;", "").Replace("&amp;", "").Replace("&lt;", "").Replace("&gt;", "").Replace("&euro;", "").Replace("&Aacute;", "").Replace("&middot;", "").Replace("&copy;", "");
        }       
        #endregion      

        #region 返回客户端IP
        /// <summary>
        /// 返回客户端IP
        /// </summary>
        /// <returns>返回客户端IP</returns>
        public static string GetClientIP()
        {
            string test = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (String.IsNullOrEmpty(test) || !Validate.IsIPAddress(test))
            {
                test = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (String.IsNullOrEmpty(test) || !Validate.IsIPAddress(test))
            {
                test = HttpContext.Current.Request.UserHostAddress;
            }
            return test;
        } 
        #endregion

        #region 获取Asp.Net内存占用,单位:M
        /// <summary>
        /// 获取Asp.Net内存占用,单位:M
        /// </summary>
        public static string GetAspNetMemory()
        {
            string temp;
            try
            {
                temp = ((Double)System.Diagnostics.Process.GetCurrentProcess().WorkingSet / 1048576).ToString("N2");
            }
            catch
            {
                temp = "未知";
            }
            return temp;
        } 
        #endregion

        #region 多文件上传
        /// <summary>
        /// 多文件上传
        /// </summary>
        /// <param name="_list">后缀名列表</param>
        /// <param name="uploadList">key为HttpPostedFile,value为保存路径(不要加后缀名)</param>
        /// <returns></returns>
        public static IList<string> Uploads(IList<string> _list, Hashtable uploadList)
        {
            IList<string> list = new List<string>();

            foreach (DictionaryEntry de in uploadList)
            {
                HttpPostedFile hpf = (HttpPostedFile)de.Key;
                string path = de.Value + System.IO.Path.GetExtension(hpf.FileName).ToLower();
                string fileName = UpFile(_list, hpf, path);
                list.Add(fileName);
            }

            return list;
        } 
        #endregion

        #region 文件上传(返回文件名)
        /// <summary>
        /// 文件上传(返回文件名)
        /// </summary>
        /// <param name="list">后缀名列表</param>
        /// <param name="up">上传控件</param>
        /// <param name="path">保存路径(不要加后缀名)</param>
        /// <returns></returns>
        public static string Upload(IList<string> list, FileUpload up, string path)
        {
            string filename = Convert.ToString(path).ToLower() + System.IO.Path.GetExtension(up.FileName).ToLower();
            return UpFile(list, up.PostedFile, filename);
        } 
        #endregion

        #region 多文件上传,(自动生成文件名,然后返回文件名)
        /// <summary>
        /// 多文件上传,(自动生成文件名,然后返回文件名)
        /// </summary>
        /// <param name="_list">后缀名列表</param>
        /// <param name="files">Request.Files</param>
        /// <param name="path">目标目录</param>
        /// <returns></returns>
        public static IList<string> UploadsAutoFileName(IList<string> _list, HttpFileCollection files, string path)
        {
            IList<string> list = new List<string>();

            for (int i = 0; i < files.Count; i++)
            {
                //获取文件名
                string file = DateTime.Now.ToString("yyyyMMddHHmmss_" + GetRandString(OZGNet.Options.RandString.Number, 2) + "_" + (i + 1));
                string hz = System.IO.Path.GetExtension(files[i].FileName).ToLower();
                string fileName = UpFile(_list, files[i], Convert.ToString(path + file + hz).ToLower());
                list.Add(fileName);
                Thread.Sleep(50);
            }

            return list;
        } 
        #endregion

        #region 文件上传,(自动生成文件名,然后返回文件名)
        /// <summary>
        /// 文件上传,(自动生成文件名,然后返回文件名)
        /// </summary>
        /// <param name="list">后缀名列表</param>
        /// <param name="up">上传控件</param>
        /// <param name="path">目标目录</param>
        /// <returns></returns>
        public static string UploadAutoFileName(IList<string> list, FileUpload up, string path)
        {
            //获取文件名
            string file = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + GetRandString(OZGNet.Options.RandString.Number, 2);
            string hz = System.IO.Path.GetExtension(up.FileName).ToLower();
            string filename = filename = Convert.ToString(path + file + hz).ToLower();


            return UpFile(list, up.PostedFile, filename);
        } 
        #endregion

        #region HTML控件 文件上传(返回文件名)
        /// <summary>
        /// 文件上传(返回文件名)
        /// </summary>
        /// <param name="list">后缀名列表</param>
        /// <param name="up">上传HTML控件</param>
        /// <param name="path">保存路径(不要加后缀名)</param>
        /// <returns></returns>
        public static string Upload(IList<string> list, HttpPostedFile up, string path)
        {
            string filename = Convert.ToString(path).ToLower() + System.IO.Path.GetExtension(up.FileName).ToLower();
            return UpFile(list, up, filename);
        }
        #endregion

        #region HTML控件 文件上传,(自动生成文件名,然后返回文件名)
        /// <summary>
        /// 文件上传,(自动生成文件名,然后返回文件名)
        /// </summary>
        /// <param name="list">后缀名列表</param>
        /// <param name="up">上传HTML控件</param>
        /// <param name="path">目标目录</param>
        /// <returns></returns>
        public static string UploadAutoFileName(IList<string> list, HttpPostedFile up, string path)
        {
            //获取文件名
            string file = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + GetRandString(OZGNet.Options.RandString.Number, 2);
            string hz = System.IO.Path.GetExtension(up.FileName).ToLower();
            string filename = filename = Convert.ToString(path + file + hz).ToLower();

            return UpFile(list, up, filename);
        }
        #endregion        

        #region 上传文件的私有方法
        /// <summary>
        /// 上传文件的私有方法
        /// </summary>
        /// <param name="list">后缀名列表</param>
        /// <param name="hpf">HttpPostedFile</param>
        /// <param name="path">保存路径</param>
        /// <returns></returns>
        protected static string UpFile(IList<string> list, HttpPostedFile hpf, string path)
        {
            bool isOK = false;
            string hz = System.IO.Path.GetExtension(hpf.FileName).ToLower();
            string filename = null;

            if (list.Contains(".*"))
            {
                isOK = true;
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].ToLower() == hz)
                    {
                        isOK = true;
                        break;
                    }
                }
            }


            if (isOK)
            {
                filename = Convert.ToString(path).ToLower();
                hpf.SaveAs(HttpContext.Current.Server.MapPath(filename));
            }
            else
            {
                filename = null;
            }

            return filename;
        } 
        #endregion

        #region 上传视频文件并转换成FLV(返回文件名)
        /// <summary>
        /// 上传视频文件并转换成FLV(返回文件名)
        /// </summary>
        /// <param name="ffmpeg">ffmpeg的路径</param>
        /// <param name="list">后缀名列表</param>
        /// <param name="up">上传控件</param>
        /// <param name="path">保存文件名,(虚拟路径,且不用加后缀名)</param>
        /// <param name="width">FLV的宽</param>
        /// <param name="height">FLV的高</param>
        /// <returns></returns>
        public static string UploadFLV(string ffmpeg, IList<string> list, FileUpload up, string path, string width, string height)
        {
            bool isOK = false;
            string hz = System.IO.Path.GetExtension(up.FileName).ToLower();
            string filename = null;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ToLower() == hz)
                {
                    isOK = true;
                    break;
                }
            }

            if (isOK)
            {
                if (width == null)
                {
                    width = "480";
                }

                if (height == null)
                {
                    height = "320";
                }

                filename = Convert.ToString(path + ".flv").ToLower();
                OZGNet.WebForm.UtilityIO.ChangeFLV(ffmpeg, up.PostedFile.FileName, filename, width, height);
            }
            else
            {
                filename = null;
            }

            return filename;
        } 
        #endregion

        #region 上传视频文件并转换成FLV(自动生成文件名,然后返回文件名)
        /// <summary>
        /// 上传视频文件并转换成FLV(自动生成文件名,然后返回文件名)
        /// </summary>
        /// <param name="ffmpeg">ffmpeg的路径</param>
        /// <param name="list">后缀名列表</param>
        /// <param name="up">上传控件</param>
        /// <param name="path">保存目录</param>
        /// <param name="width">FLV的宽</param>
        /// <param name="height">FLV的高</param>
        /// <returns></returns>
        public static string UploadFLVAutoFileName(string ffmpeg, IList<string> list, FileUpload up, string path, string width, string height)
        {
            //获取文件名
            string file = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + GetRandString(OZGNet.Options.RandString.Number, 2);

            string filename = null;

            bool isOK = false;
            string hz = System.IO.Path.GetExtension(up.FileName).ToLower();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ToLower() == hz)
                {
                    isOK = true;
                    break;
                }
            }

            if (isOK)
            {
                if (width == null)
                {
                    width = "480";
                }

                if (height == null)
                {
                    height = "320";
                }

                filename = Convert.ToString(path + file + ".flv").ToLower();
                OZGNet.WebForm.UtilityIO.ChangeFLV(ffmpeg, up.PostedFile.FileName, filename, width, height);
            }
            else
            {
                filename = null;
            }

            return filename;
        } 
        #endregion                        

        private static string sqlValidStr = "'|;|select| and |exec|insert|delete|update|count|*|%|chr|char|mid|master|truncate|declare|<|>";

        #region 对QueryString进行危险字符的检测，针对Aspx文件
        /// <summary>
        /// 对QueryString进行危险字符的检测，针对Aspx文件
        /// </summary>
        /// <returns></returns>
        public static string SqlQueryValidForAspx()
        {
            return SqlQueryValidForAspx(HttpContext.Current.Handler as Page);
        }
        /// <summary>
        /// 对QueryString进行危险字符的检测，针对Aspx文件
        /// </summary>
        /// <param name="p">页面对象</param>
        /// <returns></returns>
        public static string SqlQueryValidForAspx(Page p)
        {
            string msg = null;
            List<string> sql_list = OZGNet.Utility.StrSplit(sqlValidStr, "|");
            for (int i = 0; i < sql_list.Count; i++)
            {
                foreach (string s in p.Request.QueryString)
                {
                    if (p.Request.QueryString[s].ToLower().Contains(sql_list[i]))
                    {
                        if (msg == null)
                        {
                            msg = "QueryString提交的字符不能含有：" + sql_list[i];
                        }
                        else
                        {
                            if (!msg.Contains(sql_list[i]))
                            {
                                msg += "," + sql_list[i];
                            }
                        }
                    }
                }
            }

            if (msg == null)
            {
                return "QueryString验证成功";
            }
            else
            {
                return msg;
            }
        }
        #endregion

        #region 对所有Form进行危险字符的检测，针对Aspx文件
        /// <summary>
        /// 对所有Form进行危险字符的检测，针对Aspx文件
        /// </summary>
        /// <returns></returns>
        public static string SqlFormValidForAspx()
        {
            return SqlFormValidForAspx(HttpContext.Current.Handler as Page);
        }
        /// <summary>
        /// 对所有Form进行危险字符的检测，针对Aspx文件
        /// </summary>
        /// <param name="p">页面对象</param>
        /// <returns></returns>
        public static string SqlFormValidForAspx(Page p)
        {
            string msg = null;
            List<string> sql_list = OZGNet.Utility.StrSplit(sqlValidStr, "|");
            for (int i = 0; i < sql_list.Count; i++)
            {
                for (int j = 0; j < p.Request.Form.Count; j++)
                {
                    //这个判断用来跳过视图状态
                    if (j != 0)
                    {
                        if (p.Request.Form[j].ToLower().Contains(sql_list[i].ToString()))
                        {
                            if (msg == null)
                            {
                                msg = "Form提交的字符不能含有：" + sql_list[i];
                            }
                            else
                            {
                                if (!msg.Contains(sql_list[i]))
                                {
                                    msg += "," + sql_list[i];
                                }
                            }
                        }
                    }
                }
            }

            if (msg == null)
            {
                return "Form验证成功";
            }
            else
            {
                return msg;
            }
        }
        #endregion

        #region 对整个页面进行危险字符的检测，针对Aspx文件
        /// <summary>
        /// 对整个页面进行危险字符的检测，针对Aspx文件
        /// </summary>
        /// <returns></returns>
        public static string SqlValidForAspx()
        {
            return SqlValidForAspx(HttpContext.Current.Handler as Page);
        }
        /// <summary>
        /// 对整个页面进行危险字符的检测
        /// </summary>
        /// <param name="p">页面对象</param>
        /// <returns>返回false则说明有问题</returns>
        public static string SqlValidForAspx(Page p)
        {
            string msg1 = SqlQueryValidForAspx(p);
            string msg2 = SqlFormValidForAspx(p);
            return msg1 + "|" + msg2;
        }
        #endregion

        #region 对QueryString进行危险字符的检测，针对Ashx文件
        /// <summary>
        /// 对QueryString进行危险字符的检测，针对Ashx文件
        /// </summary>
        /// <returns></returns>
        public static string SqlQueryValidForAshx()
        {
            return SqlQueryValidForAshx(HttpContext.Current);
        }
        /// <summary>
        /// 对QueryString进行危险字符的检测，针对Ashx文件
        /// </summary>
        /// <param name="context">HttpContext实例</param>
        /// <returns></returns>
        public static string SqlQueryValidForAshx(HttpContext context)
        {
            string msg = null;
            List<string> sql_list = OZGNet.Utility.StrSplit(sqlValidStr, "|");
            for (int i = 0; i < sql_list.Count; i++)
            {
                foreach (string s in context.Request.QueryString)
                {
                    if (context.Request.QueryString[s].ToLower().Contains(sql_list[i]))
                    {
                        if (msg == null)
                        {
                            msg = "QueryString提交的字符不能含有：" + sql_list[i];
                        }
                        else
                        {
                            if (!msg.Contains(sql_list[i]))
                            {
                                msg += "," + sql_list[i];
                            }
                        }
                    }
                }
            }

            if (msg == null)
            {
                return "QueryString验证成功";
            }
            else
            {
                return msg;
            }
        }
        #endregion

        #region 对所有Form进行危险字符的检测，针对Ashx文件
        /// <summary>
        /// 对所有Form进行危险字符的检测，针对Ashx文件
        /// </summary>
        /// <returns></returns>
        public static string SqlFormValidForAshx()
        {
            return SqlFormValidForAshx(HttpContext.Current);
        }
        /// <summary>
        /// 对所有Form进行危险字符的检测，针对Ashx文件
        /// </summary>
        /// <param name="context">HttpContext实例</param>
        /// <returns></returns>
        public static string SqlFormValidForAshx(HttpContext context)
        {
            string msg = null;
            List<string> sql_list = OZGNet.Utility.StrSplit(sqlValidStr, "|");
            for (int i = 0; i < sql_list.Count; i++)
            {
                for (int j = 0; j < context.Request.Form.Count; j++)
                {
                    if (context.Request.Form[j].ToLower().Contains(sql_list[i].ToString()))
                    {
                        if (msg == null)
                        {
                            msg = "Form提交的字符不能含有：" + sql_list[i];
                        }
                        else
                        {
                            if (!msg.Contains(sql_list[i]))
                            {
                                msg += "," + sql_list[i];
                            }
                        }
                    }
                }
            }

            if (msg == null)
            {
                return "Form验证成功";
            }
            else
            {
                return msg;
            }
        }
        #endregion

        #region 对整个页面进行危险字符的检测，针对Ashx文件
        /// <summary>
        /// 对整个页面进行危险字符的检测，针对Ashx文件
        /// </summary>
        /// <returns></returns>
        public static string SqlValidForAshx()
        {
            return SqlValidForAshx(HttpContext.Current);
        }
        /// <summary>
        /// 对整个页面进行危险字符的检测
        /// </summary>
        /// <param name="context">页面对象</param>
        /// <returns>返回false则说明有问题</returns>
        public static string SqlValidForAshx(HttpContext context)
        {
            string msg1 = SqlQueryValidForAshx(context);
            string msg2 = SqlFormValidForAshx(context);
            return msg1 + "|" + msg2;
        }
        #endregion

        #region 对QueryString进行危险字符的检测，针对Aspx文件
        /// <summary>
        /// 对QueryString进行危险字符的检测，针对Aspx文件
        /// </summary>
        /// <returns></returns>
        public static bool IsSqlQueryValidForAspx()
        {
            string msg = SqlQueryValidForAspx(HttpContext.Current.Handler as Page);
            return msg.Contains("成功");
        }
        /// <summary>
        /// 对QueryString进行危险字符的检测，针对Aspx文件
        /// </summary>
        /// <param name="p">页面对象</param>
        /// <returns></returns>
        public static bool IsSqlQueryValidForAspx(Page p)
        {
            string msg = SqlQueryValidForAspx(p);
            return msg.Contains("成功");
        }
        #endregion

        #region 对所有Form进行危险字符的检测，针对Aspx文件
        /// <summary>
        /// 对所有Form进行危险字符的检测，针对Aspx文件
        /// </summary>
        /// <returns></returns>
        public static bool IsSqlFormValidForAspx()
        {
            string msg = SqlFormValidForAspx(HttpContext.Current.Handler as Page);
            return msg.Contains("成功");
        }
        /// <summary>
        /// 对所有Form进行危险字符的检测，针对Aspx文件
        /// </summary>
        /// <param name="p">页面对象</param>
        /// <returns></returns>
        public static bool IsSqlFormValidForAspx(Page p)
        {
            string msg = SqlFormValidForAspx(p);
            return msg.Contains("成功");
        }
        #endregion

        #region 对整个页面进行危险字符的检测，针对Aspx文件
        /// <summary>
        /// 对整个页面进行危险字符的检测，针对Aspx文件
        /// </summary>
        /// <returns></returns>
        public static bool IsSqlValidForAspx()
        {
            return IsSqlValidForAspx(HttpContext.Current.Handler as Page);
        }
        /// <summary>
        /// 对整个页面进行危险字符的检测
        /// </summary>
        /// <param name="p">页面对象</param>
        /// <returns>返回false则说明有问题</returns>
        public static bool IsSqlValidForAspx(Page p)
        {
            bool b = IsSqlQueryValidForAspx(p);
            if (b)
            {
                b = IsSqlFormValidForAspx(p);
                return b;
            }
            return false;
        }
        #endregion

        #region 对QueryString进行危险字符的检测，针对Ashx文件
        /// <summary>
        /// 对QueryString进行危险字符的检测，针对Ashx文件
        /// </summary>
        /// <returns></returns>
        public static bool IsSqlQueryValidForAshx()
        {
            string msg = SqlQueryValidForAshx(HttpContext.Current);
            return msg.Contains("成功");
        }
        /// <summary>
        /// 对QueryString进行危险字符的检测，针对Ashx文件
        /// </summary>
        /// <param name="context">HttpContext实例</param>
        /// <returns></returns>
        public static bool IsSqlQueryValidForAshx(HttpContext context)
        {
            string msg = SqlQueryValidForAshx(context);
            return msg.Contains("成功");
        }
        #endregion

        #region 对所有Form进行危险字符的检测，针对Ashx文件
        /// <summary>
        /// 对所有Form进行危险字符的检测，针对Ashx文件
        /// </summary>
        /// <returns></returns>
        public static bool IsSqlFormValidForAshx()
        {
            string msg = SqlFormValidForAshx(HttpContext.Current);
            return msg.Contains("成功");
        }
        /// <summary>
        /// 对所有Form进行危险字符的检测，针对Ashx文件
        /// </summary>
        /// <param name="context">HttpContext实例</param>
        /// <returns></returns>
        public static bool IsSqlFormValidForAshx(HttpContext context)
        {
            string msg = SqlFormValidForAshx(context);
            return msg.Contains("成功");
        }
        #endregion

        #region 对整个页面进行危险字符的检测，针对Ashx文件
        /// <summary>
        /// 对整个页面进行危险字符的检测，针对Ashx文件
        /// </summary>
        /// <returns></returns>
        public static bool IsSqlValidForAshx()
        {
            return IsSqlValidForAshx(HttpContext.Current);
        }
        /// <summary>
        /// 对整个页面进行危险字符的检测
        /// </summary>
        /// <param name="context">页面对象</param>
        /// <returns>返回false则说明有问题</returns>
        public static bool IsSqlValidForAshx(HttpContext context)
        {
            bool b = IsSqlQueryValidForAshx(context);
            if (b)
            {
                b = IsSqlFormValidForAshx(context);
                return b;
            }
            return false;
        }
        #endregion

        #region UBB过滤
        /// <summary>
        /// UBB过滤
        /// </summary>
        /// <param name="content">目标内容</param>
        /// <returns></returns>
        public static string UbbReplace(string content)
        {
            content = content.Replace("\n", "<BR>");
            content = content.Replace("\t", "   ");
            content = content.Replace(" ", "&nbsp;");
            for (int i = 1; i < 43; i++)
                content = content.Replace("[em" + i + "]", "<IMG SRC=\"ubb/face/em" + i + ".gif\">");
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[url=(?<x>[^\]]*)\](?<y>[^\]]*)\[/url\]", @"<a href=$1 target=_blank>$2</a>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[email=(?<x>[^\]]*)\](?<y>[^\]]*)\[/email\]", @"<a href=mailto:$1>$2</a>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[flash](?<x>[^\]]*)\[/flash]", @"<OBJECT codeBase=http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0 classid=clsid:D27CDB6E-AE6D-11cf-96B8-444553540000 width=500 height=400><PARAM NAME=movie VALUE=""$1""><PARAM NAME=quality VALUE=high><embed src=""$1"" quality=high pluginspage='http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash' type='application/x-shockwave-flash' width=500 height=400>$1</embed></OBJECT>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[img](?<x>[^\]]*)\[/img]", @"<IMG SRC=""$1"" border=0>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[color=(?<x>[^\]]*)\](?<y>[^\]]*)\[/color\]", @"<font color=$1>$2</font>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[face=(?<x>[^\]]*)\](?<y>[^\]]*)\[/face\]", @"<font face=$1>$2</font>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[size=(?<x>[^\]]*)\](?<y>[^\]]*)\[/size\]", @"<font size=$1>$2</font>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[align=(?<x>[^\]]*)\](?<y>[^\]]*)\[/align\]", @"<div align=$1>$2</div>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[fly](?<x>[^\]]*)\[/fly]", @"<marquee width=90% behavior=alternate scrollamount=3>$1</marquee>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[move](?<x>[^\]]*)\[/move]", @"<marquee scrollamount=3>$1</marquee>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[glow=(?<x>[^\]]*),(?<y>[^\]]*),(?<z>[^\]]*)\](?<w>[^\]]*)\[/glow\]", @"<table width=$1 style=""filter:glow(color=$2, strength=$3)"">$4</table>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[shadow=(?<x>[^\]]*),(?<y>[^\]]*),(?<z>[^\]]*)\](?<w>[^\]]*)\[/shadow\]", @"<table width=$1 style=""filter:shadow(color=$2, strength=$3)"">$4</table>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[b\](?<x>[^\]]*)\[/b\]", @"<b>$1</b>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[i\](?<x>[^\]]*)\[/i\]", @"<i>$1</i>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[u\](?<x>[^\]]*)\[/u\]", @"<u>$1</u>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[h1\](?<x>[^\]]*)\[/h1\]", @"<h1>$1</h1>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[h2\](?<x>[^\]]*)\[/h2\]", @"<h2>$1</h2>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[h3\](?<x>[^\]]*)\[/h3\]", @"<h3>$1</h3>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[h4\](?<x>[^\]]*)\[/h4\]", @"<h4>$1</h4>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[h5\](?<x>[^\]]*)\[/h5\]", @"<h5>$1</h5>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[h6\](?<x>[^\]]*)\[/h6\]", @"<h6>$1</h6>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[small\](?<x>[^\]]*)\[/small\]", @"<small>$1</small>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[big\](?<x>[^\]]*)\[/big\]", @"<big>$1</big>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[del\](?<x>[^\]]*)\[/del\]", @"<del>$1</del>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[blink\](?<x>[^\]]*)\[/blink\]", @"<blink>$1</blink>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[sub\](?<x>[^\]]*)\[/sub\]", @"<sub>$1</sub>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[sup\](?<x>[^\]]*)\[/sup\]", @"<sup>$1</sup>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[list\](?<x>[^\]]*)\[/list\]", @"<li>$1</li>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[indent\](?<x>[^\]]*)\[/indent\]", @"<blockquote><p>$1</p></blockquote>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\[quote\](?<x>[^\]]*)\[/quote\]", @"以下内容为引用：<table border=0 width=95% cellpadding=10 cellspacing=1 bgcolor=#000000><tr><td bgcolor=#FFFFFF>$1</td></tr></table>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            return content;
        } 
        #endregion

        #region 绑定DataGrid,GridView,DataList,Repeater
        /// <summary>
        /// 绑定DataGrid,GridView,DataList,Repeater
        /// </summary>
        /// <param name="control">数据控件</param>
        /// <param name="datasource">数据源</param>
        public static void BindData(Control control, object datasource)
        {
            if (control is DataGrid)
            {
                DataGrid obj = control as DataGrid;
                obj.DataSource = datasource;
                obj.DataBind();
            }
            else if (control is GridView)
            {
                GridView obj = control as GridView;
                obj.DataSource = datasource;
                obj.DataBind();
            }
            else if (control is DataList)
            {
                DataList obj = control as DataList;
                obj.DataSource = datasource;
                obj.DataBind();
            }
            else if (control is Repeater)
            {
                Repeater obj = control as Repeater;
                obj.DataSource = datasource;
                obj.DataBind();
            }
        } 
        #endregion

        #region 绑定列表数据
        /// <summary>
        /// 绑定列表数据
        /// </summary>
        /// <param name="control">数据控件</param>
        /// <param name="datasource">数据源</param>
        /// <param name="text">文本字段</param>
        /// <param name="value">值字段</param>
        public static void BindData(Control control, object datasource, string text, string value)
        {
            if (control is DropDownList)
            {
                DropDownList obj = control as DropDownList;
                obj.DataSource = datasource;
                obj.DataTextField = text;
                obj.DataValueField = value;
                obj.DataBind();
            }
            else if (control is ListBox)
            {
                ListBox obj = control as ListBox;
                obj.DataSource = datasource;
                obj.DataTextField = text;
                obj.DataValueField = value;
                obj.DataBind();
            }
            else if (control is CheckBoxList)
            {
                CheckBoxList obj = control as CheckBoxList;
                obj.DataSource = datasource;
                obj.DataTextField = text;
                obj.DataValueField = value;
                obj.DataBind();
            }
            else if (control is RadioButtonList)
            {
                RadioButtonList obj = control as RadioButtonList;
                obj.DataSource = datasource;
                obj.DataTextField = text;
                obj.DataValueField = value;
                obj.DataBind();
            }
            else if (control is BulletedList)
            {
                BulletedList obj = control as BulletedList;
                obj.DataSource = datasource;
                obj.DataTextField = text;
                obj.DataValueField = value;
                obj.DataBind();
            }
            else if (control is HtmlSelect)
            {
                HtmlSelect obj = control as HtmlSelect;
                obj.DataSource = datasource;
                obj.DataTextField = text;
                obj.DataValueField = value;
                obj.DataBind();
            }
        } 
        #endregion

        #region 生成缩略图
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height)
        {
            Stream s = File.OpenRead(HttpContext.Current.Server.MapPath(originalImagePath));
            System.Drawing.Image originalImage = System.Drawing.Image.FromStream(s);
            MakeThumbnail(originalImagePath, thumbnailPath, width, height, originalImage.RawFormat);
            s.Close();
            s.Dispose();
            originalImage.Dispose();
        }

        
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="format">生成缩略图的格式</param>
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, System.Drawing.Imaging.ImageFormat format)
        {
            OZGNet.GDI.Utility.MakeThumbnail(HttpContext.Current.Server.MapPath(originalImagePath), HttpContext.Current.Server.MapPath(thumbnailPath), width, height, format);
            
        }
        #endregion

        #region 生成水印(使用文本),然后删除源图
        /// <summary>
        /// 生成水印(使用文本),然后删除源图
        /// </summary>
        /// <param name="Path">源图路径</param>
        /// <param name="Path_sy">生成水印后的图片路径</param>
        /// <param name="text">水印文字</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="color">字体颜色</param>
        public static void ShuiYinWord(string Path, string Path_sy, string text, float fontSize, System.Drawing.Color color)
        {
            string path = HttpContext.Current.Server.MapPath(Path);
            string path_sy = HttpContext.Current.Server.MapPath(Path_sy);
            OZGNet.GDI.Utility.ShuiYinWord(path, path_sy, text, fontSize, color);
        }
        #endregion

        #region 生成水印(使用文本)
        /// <summary>
        /// 生成水印(使用文本)
        /// </summary>
        /// <param name="Path">源图路径</param>
        /// <param name="Path_sy">生成水印后的图片路径</param>
        /// <param name="text">水印文字</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="color">字体颜色</param>
        /// <param name="delete">是否删除源图</param>
        public static void ShuiYinWord(string Path, string Path_sy, string text, float fontSize, System.Drawing.Color color, bool delete)
        {
            string path = HttpContext.Current.Server.MapPath(Path);
            string path_sy = HttpContext.Current.Server.MapPath(Path_sy);
            OZGNet.GDI.Utility.ShuiYinWord(Path, Path_sy, text, fontSize, color, delete);
        }
        #endregion

        #region 在图片上生成图片水印,然后删除源图
        /// <summary>
        /// 在图片上生成图片水印,然后删除源图
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_syp">生成的带图片水印的图片路径</param>
        /// <param name="Path_sypf">水印图片路径</param>
        public static void ShuiYinPic(string Path, string Path_syp, string Path_sypf)
        {
            OZGNet.GDI.Utility.ShuiYinPic(HttpContext.Current.Server.MapPath(Path), HttpContext.Current.Server.MapPath(Path_syp), HttpContext.Current.Server.MapPath(Path_sypf), OZGNet.Options.ShuiYinPicPoint.RightDown, true);
        }
        #endregion

        #region 在图片上生成图片水印
        /// <summary>
        /// 在图片上生成图片水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_syp">生成的带图片水印的图片路径</param>
        /// <param name="Path_sypf">水印图片路径</param>
        /// <param name="point">打水印图的位置</param>
        /// <param name="delete">是否删除原图片</param>
        public static void ShuiYinPic(string Path, string Path_syp, string Path_sypf, OZGNet.Options.ShuiYinPicPoint point, bool delete)
        {
            OZGNet.GDI.Utility.ShuiYinPic(HttpContext.Current.Server.MapPath(Path), HttpContext.Current.Server.MapPath(Path_syp), HttpContext.Current.Server.MapPath(Path_sypf), point, delete);
        }
        #endregion

        #region 图片防盗链
        /// <summary>
        /// 图片防盗链
        /// </summary>
        /// <param name="path">目标图片</param>
        /// <param name="path_err">目标图片被盗链后显示的图片</param>
        /// <param name="session">会话名称</param>
        public static void FDPic(string path, string path_err, string session)
        {
            string str_path = "";
            if (!String.IsNullOrEmpty(HttpContext.Current.Session[session] as String))
            {
                str_path = path;
            }
            else
            {
                str_path = path_err;
            }

            System.Drawing.Imaging.ImageFormat imageformat;
            if (System.IO.Path.GetExtension(str_path).ToLower() == ".jpg" || System.IO.Path.GetExtension(str_path).ToLower() == ".jpeg")
            {
                imageformat = System.Drawing.Imaging.ImageFormat.Jpeg;
            }
            else if (System.IO.Path.GetExtension(str_path).ToLower() == ".png")
            {
                imageformat = System.Drawing.Imaging.ImageFormat.Png;
            }
            else
            {
                imageformat = System.Drawing.Imaging.ImageFormat.Gif;
            }

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(HttpContext.Current.Server.MapPath(str_path));
            try
            {
                bmp.Save(HttpContext.Current.Response.OutputStream, imageformat);
            }
            catch { }
            bmp.Dispose();
        } 
        #endregion           
                        
        #region 执行程序(马上执行)
        /// <summary>
        /// 执行程序(马上执行)
        /// </summary>
        /// <param name="ApplicationPath">程序路径</param>
        /// <param name="Arguments">命令参数</param>
        /// <returns></returns>
        public static Process ExecuteApplication(string ApplicationPath, string Arguments)
        {
            return OZGNet.Utility.ExecuteApplication(HttpContext.Current.Server.MapPath(ApplicationPath), Arguments);
        } 
        #endregion
        #region 执行程序
        /// <summary>
        /// 执行程序
        /// </summary>
        /// <param name="ApplicationPath">程序路径</param>
        /// <param name="Arguments">命令参数</param>
        /// <param name="IsStart">是否马上执行</param>
        /// <returns></returns>
        public static Process ExecuteApplication(string ApplicationPath, string Arguments, bool IsStart)
        {
            return OZGNet.Utility.ExecuteApplication(HttpContext.Current.Server.MapPath(ApplicationPath), Arguments, IsStart);
        } 
        #endregion

        #region 检测定时防刷新(返回false,则有效时间未消失)
        /// <summary>
        /// 检测定时防刷新(返回false,则有效时间未消失)
        /// </summary>
        /// <returns></returns>
        public static bool PreventionRefreshCheck()
        {
            /*
            使用实例
            double v = 2;
            if (OZGNet.AspNet.PreventionRefreshCheck())
            {
                OZGNet.AspNet.PreventionRefresh(v);            
            }
            else
            {
                OZGNet.AspNet.msg("本站启用了防刷新机制 " + v.ToString() + "秒内", false, null);
            }
            */
            bool isOK = true;
            if (HttpContext.Current.Cache[GetClientIP()] != null)
            {
                isOK = false;
            }
            return isOK;
        } 
        #endregion
        #region 启用定时防刷新(n秒内禁止刷新)
        /// <summary>
        /// 启用定时防刷新(n秒内禁止刷新)
        /// </summary>
        /// <param name="v">定时时间(秒)</param>
        public static void PreventionRefresh(double v)
        {
            CacheAbsolute(GetClientIP(), "Has", v + 1);
        } 
        #endregion

        #region 切割图片(GDI版)
        /// <summary>
        /// 切割图片(GDI版)
        /// </summary>
        /// <param name="source_pic">原图路径</param>
        /// <param name="save_pic">输出路径</param>
        /// <param name="layout_left">左上角坐标和剪切的宽和高</param>
        /// <param name="layout_top">左上角坐标和剪切的宽和高</param>
        /// <param name="layout_width">左上角坐标和剪切的宽和高</param>
        /// <param name="layout_height">左上角坐标和剪切的宽和高</param>
        /// <param name="layout_zoom">放大缩小的倍数</param>
        /// <param name="save_width">保存图的宽度</param>
        /// <param name="disp_width">截图前的显示图的宽度</param>
        public static void LayoutForGDI(string source_pic, string save_pic, int layout_left, int layout_top, int layout_width, int layout_height, int layout_zoom, int save_width, int disp_width)
        {
            OZGNet.GDI.Utility.LayoutForGDI(HttpContext.Current.Server.MapPath(source_pic), HttpContext.Current.Server.MapPath(save_pic), layout_left, layout_top, layout_width, layout_height, layout_zoom, save_width, disp_width);            
        }
        #endregion

        #region 切割图片(ASPJpeg版)
        /// <summary>
        /// 切割图片(ASPJpeg版)
        /// </summary>
        /// <param name="source_pic">原图路径</param>
        /// <param name="save_pic">输出路径</param>
        /// <param name="layout_left">左上角坐标和剪切的宽和高</param>
        /// <param name="layout_top">左上角坐标和剪切的宽和高</param>
        /// <param name="layout_width">左上角坐标和剪切的宽和高</param>
        /// <param name="layout_height">左上角坐标和剪切的宽和高</param>
        /// <param name="layout_zoom">放大缩小的倍数</param>
        /// <param name="save_width">保存图的宽度</param>
        /// <param name="disp_width">截图前的显示图的宽度</param>
        public static void LayoutForASPJpeg(string source_pic, string save_pic, int layout_left, int layout_top, int layout_width, int layout_height, int layout_zoom, int save_width, int disp_width)
        {
            if (!CheckActiveX("Persits.Jpeg"))
            {
                throw new Exception("该服务器不支持ASPJpeg!");
            }
            OZGNet.GDI.Utility.LayoutForASPJpeg(HttpContext.Current.Server.MapPath(source_pic), HttpContext.Current.Server.MapPath(save_pic), layout_left, layout_top, layout_width, layout_height, layout_zoom, save_width, disp_width);        
        }
        #endregion

        #region 测试服务器是否支持ActiveX组件
        /// <summary>
        /// 测试服务器是否支持ActiveX组件
        /// </summary>
        /// <param name="ActiveX_Name"></param>
        /// <returns></returns>
        public static bool CheckActiveX(string ActiveX_Name)
        {
            try
            {
                HttpContext.Current.Server.CreateObject(ActiveX_Name);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
              
        #region 获取webform的根目录
        /// <summary>
        /// 获取webform的根目录
        /// </summary>
        /// <returns></returns>
        public static string GetAppPath()
        {
            return HttpContext.Current.Server.MapPath("/");
        }
        #endregion

        #region 输入新的宽度，然后按比例返回图片
        /// <summary>
        /// 输入新的宽度，然后按比例返回图片
        /// </summary>
        /// <param name="path">图片路径</param>
        /// <param name="newWidth">新的宽度</param>
        /// <returns></returns>
        public static Bitmap GetProportionBitmap(string path, int newWidth)
        {
            return OZGNet.GDI.Utility.GetProportionBitmap(HttpContext.Current.Server.MapPath(path), newWidth);
        }
        #endregion
            
        #region 获取客户端操作系统 (未支持非Windows系统,Vista和2008)
        /// <summary>
        /// 获取客户端操作系统 (未支持非Windows系统,Vista和2008)
        /// </summary>
        /// <returns></returns>
        public static string GetClientOSName()
        {
            string s = HttpContext.Current.Request.ServerVariables["Http_User_Agent"];
            if (s.Contains("98"))
            {
                return "Windows 98";
            }
            else if (s.Contains("Me"))
            {
                return "Windows Me";
            }
            else if (s.Contains("Windows NT 4"))
            {
                return "Windows NT";
            }
            else if (s.Contains("NT 5.0"))
            {
                return "Windows 2000";
            }
            else if (s.Contains("NT 5.1"))
            {
                return "Windows XP";
            }
            else if (s.Contains("NT 5.2"))
            {
                return "Windows 2003";
            }
            else
            {
                return "未知";
            }
        } 
        #endregion

        #region 返回时间提醒语
        /// <summary>
        /// 返回当前时间提醒语
        /// </summary>
        /// <returns></returns>
        public static string GetDateMessge()
        {
            return GetDateMessge(DateTime.Now);
        }
        #endregion        

        #region 以服务器本地的路径方式，下载文件
        /// <summary>
        /// 以服务器本地的路径方式，下载文件
        /// </summary>
        /// <param name="ServerLocalPath">服务器的本地路径</param>
        public static void DownLocalFile(string ServerLocalPath)
        {
            string path = ServerLocalPath;
            FileInfo file_info = new FileInfo(path);
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(file_info.Name, Encoding.UTF8).Replace("+", " "));
            HttpContext.Current.Response.End();
        }
        #endregion

        #region 以保存文件的方式保存Post提交的二进制(目前主要用来处理as3提交的ByteArray)
        /// <summary>
        /// 以保存文件的方式保存Post提交的二进制(目前主要用来处理as3提交的ByteArray)
        /// </summary>
        /// <param name="savePath">保存文件的路径</param>
        public static void SavePostBinary(string savePath)
        {
            byte[] buffer = HttpContext.Current.Request.BinaryRead(HttpContext.Current.Request.TotalBytes);
            OZGNet.WebForm.UtilityIO.FO_WriteFile(buffer, savePath);
        } 
        #endregion

        #region 检测客户端是否使用手机浏览
        /// <summary>
        /// 检测客户端是否使用手机浏览
        /// </summary>
        /// <returns></returns>
        public static bool IsWapClient()
        {
            if (IsWebClient())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
        #region 检测客户端是否使用电脑的浏览器浏览
        /// <summary>
        /// 检测客户端是否使用电脑的浏览器浏览
        /// </summary>
        /// <returns></returns>
        public static bool IsWebClient()
        {
            HttpContext context = HttpContext.Current;
            try
            {
                if (context.Request.ServerVariables["HTTP_USER_AGENT"].Contains("Mozilla"))
                {
                    return true;
                }
                else if (context.Request.ServerVariables["HTTP_USER_AGENT"].Contains("Opera"))
                {
                    return true;
                }

                if (context.Request.ServerVariables["User-Agent"].Contains("Mozilla"))
                {
                    return true;
                }
                else if (context.Request.ServerVariables["User-Agent"].Contains("Opera"))
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion
      
        #region 页面meta相关
        /// <summary>
        /// 往页面添加描述
        /// </summary>
        /// <param name="value">描述</param>
        public static void AddDescription(string value)
        {
            AddHeaderMeta("Description", value);
        }
        /// <summary>
        /// 往页面添加关键字
        /// </summary>
        /// <param name="value">关键字</param>
        public static void AddKeywords(string value)
        {
            AddHeaderMeta("Keywords", value);
        }
        /// <summary>
        /// 往页面添加meta
        /// </summary>
        /// <param name="name">meta名称</param>
        /// <param name="value">meta值</param>
        public static void AddHeaderMeta(string name, string value)
        {
            Literal l = new Literal();
            l.EnableViewState = false;
            l.Text = string.Format("<meta name=\"{0}\" content=\"{1}\" />\r\n", name, value);
            (HttpContext.Current.Handler as Page).Header.Controls.Add(l);
        } 
        #endregion

        #region 应用程序路径转化虚拟路径，如~/JS/，转化为/WebPath/Root/JS/
        /// <summary>
        /// 应用程序路径转化虚拟路径，如~/JS/，转化为/WebPath/Root/JS/
        /// </summary>
        /// <param name="relativeUrl">应用程序路径，如~/JS/</param>
        /// <returns>转化后的虚拟路径，如/WebPath/Root/JS/</returns>
        public static string GetResolveUrl(object relativeUrl)
        {
            return GetResolveUrl(relativeUrl.ToString());
        }

        /// <summary>
        /// 应用程序路径转化虚拟路径，如~/JS/，转化为/WebPath/Root/JS/
        /// </summary>
        /// <param name="relativeUrl">应用程序路径，如~/JS/</param>
        /// <returns>转化后的虚拟路径，如/WebPath/Root/JS/</returns>
        public static string GetResolveUrl(string relativeUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(relativeUrl))
                {
                    return null;
                }

                string url = null;
                if (string.IsNullOrEmpty(relativeUrl))
                {
                    url = string.Empty;
                }
                else if (!relativeUrl.StartsWith("~"))
                {
                    url = relativeUrl;
                }
                else
                {
                    string basePath = HttpContext.Current.Request.ApplicationPath;
                    url = basePath + relativeUrl.Substring(1);
                    url = url.Replace("//", "/");
                }
                return url;
            }
            catch
            {
                return null;
            }            
        } 
        #endregion

        #region UBB代码处理函数
        /// <summary>
        /// UBB代码处理函数
        /// </summary>
        /// <param name="sDetail">输入字符串</param>
        /// <returns>输出字符串</returns>
        public static string UBBToHTML(string sDetail)
        {
            Regex r;
            Match m;
            #region 处理空格
            sDetail = sDetail.Replace(" ", "&nbsp;");
            #endregion
            #region 处理单引号
            sDetail = sDetail.Replace("'", "’");
            #endregion
            #region 处理双引号
            sDetail = sDetail.Replace("\"", "&quot;");
            #endregion
            #region html标记符
            sDetail = sDetail.Replace("<", "&lt;");
            sDetail = sDetail.Replace(">", "&gt;");

            #endregion
            #region 处理换行
            //处理换行，在每个新行的前面添加两个全角空格
            r = new Regex(@"(\r\n((&nbsp;)|　)+)(?<正文>\S+)", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<BR>　　" + m.Groups["正文"].ToString());
            }
            //处理换行，在每个新行的前面添加两个全角空格
            sDetail = sDetail.Replace("\r\n", "<BR>");
            #endregion
            #region 处[b][/b]标记
            r = new Regex(@"(\[b\])([ \S\t]*?)(\[\/b\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<B>" + m.Groups[2].ToString() + "</B>");
            }
            #endregion
            #region 处[i][/i]标记
            r = new Regex(@"(\[i\])([ \S\t]*?)(\[\/i\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<I>" + m.Groups[2].ToString() + "</I>");
            }
            #endregion
            #region 处[u][/u]标记
            r = new Regex(@"(\[U\])([ \S\t]*?)(\[\/U\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<U>" + m.Groups[2].ToString() + "</U>");
            }
            #endregion
            #region 处[p][/p]标记
            //处[p][/p]标记
            r = new Regex(@"((\r\n)*\[p\])(.*?)((\r\n)*\[\/p\])", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<P class=\"pstyle\">" + m.Groups[3].ToString() + "</P>");
            }
            #endregion
            #region 处[sup][/sup]标记
            //处[sup][/sup]标记
            r = new Regex(@"(\[sup\])([ \S\t]*?)(\[\/sup\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<SUP>" + m.Groups[2].ToString() + "</SUP>");
            }
            #endregion
            #region 处[sub][/sub]标记
            //处[sub][/sub]标记
            r = new Regex(@"(\[sub\])([ \S\t]*?)(\[\/sub\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<SUB>" + m.Groups[2].ToString() + "</SUB>");
            }
            #endregion
            #region 处[url][/url]标记
            //处[url][/url]标记
            r = new Regex(@"(\[url\])([ \S\t]*?)(\[\/url\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<A href=\"" + m.Groups[2].ToString() + "\" target=\"_blank\">"
            + m.Groups[2].ToString() + "</A>");
            }
            #endregion
            #region 处[url=xxx][/url]标记
            //处[url=xxx][/url]标记
            r = new Regex(@"(\[url=([ \S\t]+)\])([ \S\t]*?)(\[\/url\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<A href=\"" + m.Groups[2].ToString() + "\" target=\"_blank\">"
            + m.Groups[3].ToString() + "</A>");
            }
            #endregion
            #region 处[email][/email]标记
            //处[email][/email]标记
            r = new Regex(@"(\[email\])([ \S\t]*?)(\[\/email\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<A href=\"mailto:" + m.Groups[2].ToString() + "\" target=\"_blank\">" +
                 m.Groups[2].ToString() + "</A>");
            }
            #endregion
            #region 处[email=xxx][/email]标记
            //处[email=xxx][/email]标记
            r = new Regex(@"(\[email=([ \S\t]+)\])([ \S\t]*?)(\[\/email\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<A href=\"mailto:" + m.Groups[2].ToString() + "\" target=\"_blank\">" +
                 m.Groups[3].ToString() + "</A>");
            }
            #endregion
            #region 处[size=x][/size]标记
            //处[size=x][/size]标记
            r = new Regex(@"(\[size=([1-7])\])([ \S\t]*?)(\[\/size\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<FONT SIZE=" + m.Groups[2].ToString() + ">" +
                 m.Groups[3].ToString() + "</FONT>");
            }
            #endregion
            #region 处[color=x][/color]标记
            //处[color=x][/color]标记
            r = new Regex(@"(\[color=([\S]+)\])([ \S\t]*?)(\[\/color\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<FONT COLOR=" + m.Groups[2].ToString() + ">" +
                 m.Groups[3].ToString() + "</FONT>");
            }
            #endregion
            #region 处[font=x][/font]标记
            //处[font=x][/font]标记
            r = new Regex(@"(\[font=([\S]+)\])([ \S\t]*?)(\[\/font\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<FONT FACE=" + m.Groups[2].ToString() + ">" +
                 m.Groups[3].ToString() + "</FONT>");
            }
            #endregion
            #region 处理图片链接
            //处理图片链接
            r = new Regex("\\[picture\\](\\d+?)\\[\\/picture\\]", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<A href=\"ShowImage.aspx?Type=ALL&Action=forumImage&ImageID="
            + m.Groups[1].ToString() +
                 "\" target=\"_blank\"><IMG border=0 Title=\"点击打开新窗口查看\" src=\"ShowImage.aspx?Action=forumImage&ImageID=" + m.Groups[1].ToString() +
                 "\"></A>");
            }
            #endregion
            #region 处理[align=x][/align]
            //处理[align=x][/align]
            r = new Regex(@"(\[align=([\S]+)\])([ \S\t]*?)(\[\/align\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<P align=" + m.Groups[2].ToString() + ">" +
                 m.Groups[3].ToString() + "</P>");
            }
            #endregion
            #region 处[H=x][/H]标记
            //处[H=x][/H]标记
            r = new Regex(@"(\[H=([1-6])\])([ \S\t]*?)(\[\/H\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<H" + m.Groups[2].ToString() + ">" +
                 m.Groups[3].ToString() + "</H" + m.Groups[2].ToString() + ">");
            }
            #endregion
            #region 处理[list=x][*][/list]
            //处理[list=x][*][/list]
            r = new Regex(@"(\[list(=(A|a|I|i| ))?\]([ \S\t]*)\r\n)((\[\*\]([ \S\t]*\r\n))*?)(\[\/list\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                string strLI = m.Groups[5].ToString();
                Regex rLI = new Regex(@"\[\*\]([ \S\t]*\r\n?)", RegexOptions.IgnoreCase);
                Match mLI;
                for (mLI = rLI.Match(strLI); mLI.Success; mLI = mLI.NextMatch())
                {
                    strLI = strLI.Replace(mLI.Groups[0].ToString(), "<LI>" + mLI.Groups[1]);
                }
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<UL TYPE=\"" + m.Groups[3].ToString() + "\"><B>" + m.Groups[4].ToString() + "</B>" +
                 strLI + "</UL>");
            }

            #endregion
            #region 处[SHADOW=x][/SHADOW]标记
            //处[SHADOW=x][/SHADOW]标记
            r = new Regex(@"(\[SHADOW=)(\d*?),(#*\w*?),(\d*?)\]([\S\t]*?)(\[\/SHADOW\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<TABLE WIDTH=" + m.Groups[2].ToString() + "  STYLE=FILTER:SHADOW(COLOR=" + m.Groups[3].ToString() + ", STRENGTH=" + m.Groups[4].ToString() + ")>" +
                 m.Groups[5].ToString() + "</TABLE>");
            }
            #endregion
            #region 处[glow=x][/glow]标记
            //处[glow=x][/glow]标记
            r = new Regex(@"(\[glow=)(\d*?),(#*\w*?),(\d*?)\]([\S\t]*?)(\[\/glow\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<TABLE WIDTH=" + m.Groups[2].ToString() + "  STYLE=FILTER:GLOW(COLOR=" + m.Groups[3].ToString() + ", STRENGTH=" + m.Groups[4].ToString() + ")>" +
                 m.Groups[5].ToString() + "</TABLE>");
            }
            #endregion
            #region 处[center][/center]标记
            r = new Regex(@"(\[center\])([ \S\t]*?)(\[\/center\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<CENTER>" + m.Groups[2].ToString() + "</CENTER>");
            }
            #endregion
            #region 处[IMG][/IMG]标记
            r = new Regex(@"(\[IMG\])(http|https|ftp):\/\/([ \S\t]*?)(\[\/IMG\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<br><a onfocus=this.blur() href=" + m.Groups[2].ToString() + "://" + m.Groups[3].ToString() + " target=_blank><IMG SRC=" + m.Groups[2].ToString() + "://" + m.Groups[3].ToString() + " border=0 alt=按此在新窗口浏览图片 onload=javascript:if(screen.width-333<this.width)this.width=screen.width-333></a>");
            }
            #endregion
            #region 处[em]标记
            r = new Regex(@"(\[em([\S\t]*?)\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<img src=pic/em" + m.Groups[2].ToString() + ".gif border=0 align=middle>");
            }
            #endregion
            #region 处[flash=x][/flash]标记
            //处[mp=x][/mp]标记
            r = new Regex(@"(\[flash=)(\d*?),(\d*?)\]([\S\t]*?)(\[\/flash\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<a href=" + m.Groups[4].ToString() + " TARGET=_blank><IMG SRC=pic/swf.gif border=0 alt=点击开新窗口欣赏该FLASH动画!> [全屏欣赏]</a><br><br><OBJECT codeBase=http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0 classid=clsid:D27CDB6E-AE6D-11cf-96B8-444553540000 width=" + m.Groups[2].ToString() + " height=" + m.Groups[3].ToString() + "><PARAM NAME=movie VALUE=" + m.Groups[4].ToString() + "><PARAM NAME=quality VALUE=high><param name=menu value=false><embed src=" + m.Groups[4].ToString() + " quality=high menu=false pluginspage=http://www.macromedia.com/go/getflashplayer type=application/x-shockwave-flash width=" + m.Groups[2].ToString() + " height=" + m.Groups[3].ToString() + ">" + m.Groups[4].ToString() + "</embed></OBJECT>");
            }
            #endregion
            #region 处[dir=x][/dir]标记
            //处[dir=x][/dir]标记
            r = new Regex(@"(\[dir=)(\d*?),(\d*?)\]([\S\t]*?)(\[\/dir\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<object classid=clsid:166B1BCA-3F9C-11CF-8075-444553540000 codebase=http://download.macromedia.com/pub/shockwave/cabs/director/sw.cab#version=7,0,2,0 width=" + m.Groups[2].ToString() + " height=" + m.Groups[3].ToString() + "><param name=src value=" + m.Groups[4].ToString() + "><embed src=" + m.Groups[4].ToString() + " pluginspage=http://www.macromedia.com/shockwave/download/ width=" + m.Groups[2].ToString() + " height=" + m.Groups[3].ToString() + "></embed></object>");
            }
            #endregion
            #region 处[rm=x][/rm]标记
            //处[rm=x][/rm]标记
            r = new Regex(@"(\[rm=)(\d*?),(\d*?)\]([\S\t]*?)(\[\/rm\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<OBJECT classid=clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA class=OBJECT id=RAOCX width=" + m.Groups[2].ToString() + " height=" + m.Groups[3].ToString() + "><PARAM NAME=SRC VALUE=" + m.Groups[4].ToString() + "><PARAM NAME=CONSOLE VALUE=Clip1><PARAM NAME=CONTROLS VALUE=imagewindow><PARAM NAME=AUTOSTART VALUE=true></OBJECT><br><OBJECT classid=CLSID:CFCDAA03-8BE4-11CF-B84B-0020AFBBCCFA height=32 id=video2 width=" + m.Groups[2].ToString() + "><PARAM NAME=SRC VALUE=" + m.Groups[4].ToString() + "><PARAM NAME=AUTOSTART VALUE=-1><PARAM NAME=CONTROLS VALUE=controlpanel><PARAM NAME=CONSOLE VALUE=Clip1></OBJECT>");
            }
            #endregion
            #region 处[mp=x][/mp]标记
            //处[mp=x][/mp]标记
            r = new Regex(@"(\[mp=)(\d*?),(\d*?)\]([\S\t]*?)(\[\/mp\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<object align=middle classid=CLSID:22d6f312-b0f6-11d0-94ab-0080c74c7e95 class=OBJECT id=MediaPlayer width=" + m.Groups[2].ToString() + " height=" + m.Groups[3].ToString() + " ><param name=ShowStatusBar value=-1><param name=Filename value=" + m.Groups[4].ToString() + "><embed type=application/x-oleobject codebase=http://activex.microsoft.com/activex/controls/mplayer/en/nsmp2inf.cab#Version=5,1,52,701 flename=mp src=" + m.Groups[4].ToString() + "  width=" + m.Groups[2].ToString() + " height=" + m.Groups[3].ToString() + "></embed></object>");
            }
            #endregion
            #region 处[qt=x][/qt]标记
            //处[qt=x][/qt]标记
            r = new Regex(@"(\[qt=)(\d*?),(\d*?)\]([\S\t]*?)(\[\/qt\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<embed src=" + m.Groups[4].ToString() + " width=" + m.Groups[2].ToString() + " height=" + m.Groups[3].ToString() + " autoplay=true loop=false controller=true playeveryframe=false cache=false scale=TOFIT bgcolor=#000000 kioskmode=false targetcache=false pluginspage=http://www.apple.com/quicktime/>");
            }
            #endregion
            #region 处[QUOTE][/QUOTE]标记
            r = new Regex(@"(\[QUOTE\])([ \S\t]*?)(\[\/QUOTE\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<table cellpadding=0 cellspacing=0 border=1 WIDTH=94% bordercolor=#000000 bgcolor=#F2F8FF align=center  style=FONT-SIZE: 9pt><tr><td  ><table width=100% cellpadding=5 cellspacing=1 border=0><TR><TD >" + m.Groups[2].ToString() + "</table></table><br>");
            }
            #endregion
            #region 处[move][/move]标记
            r = new Regex(@"(\[move\])([ \S\t]*?)(\[\/move\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<MARQUEE scrollamount=3>" + m.Groups[2].ToString() + "</MARQUEE>");
            }
            #endregion
            #region 处[FLY][/FLY]标记
            r = new Regex(@"(\[FLY\])([ \S\t]*?)(\[\/FLY\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<MARQUEE width=80% behavior=alternate scrollamount=3>" + m.Groups[2].ToString() + "</MARQUEE>");
            }
            #endregion
            #region 处[image][/image]标记
            //处[image][/image]标记
            r = new Regex(@"(\[image\])([ \S\t]*?)(\[\/image\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<img src=\"" + m.Groups[2].ToString() + "\" border=0 align=middle><br>");
            }
            #endregion

            return sDetail;
        } 
        #endregion

        #region 取得客户端真实IP。如果有代理则取第一个非内网地址 
        /// <summary> 
        /// 取得客户端真实IP。如果有代理则取第一个非内网地址 
        /// </summary> 
        public static string GetClientRealIP()
        {
            string result = String.Empty;

            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (result != null && result != String.Empty)
            {
                //可能有代理 
                if (result.IndexOf(".") == -1)    //没有“.”肯定是非IPv4格式 
                    result = null;
                else
                {
                    if (result.IndexOf(",") != -1)
                    {
                        //有“,”，估计多个代理。取第一个不是内网的IP。 
                        result = result.Replace(" ", "").Replace("'", "");
                        string[] temparyip = result.Split(",;".ToCharArray());
                        for (int i = 0; i < temparyip.Length; i++)
                        {
                            if (OZGNet.Validate.IsIPAddress(temparyip[i])
                                && temparyip[i].Substring(0, 3) != "10."
                                && temparyip[i].Substring(0, 7) != "192.168"
                                && temparyip[i].Substring(0, 7) != "172.16.")
                            {
                                return temparyip[i];    //找到不是内网的地址 
                            }
                        }
                    }
                    else if (OZGNet.Validate.IsIPAddress(result)) //代理即是IP格式 
                        return result;
                    else
                        result = null;    //代理中的内容 非IP，取IP 
                }

            }

            string IpAddress = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];



            if (null == result || result == String.Empty)
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (result == null || result == String.Empty)
                result = HttpContext.Current.Request.UserHostAddress;

            return result;
        } 
        #endregion

    }
}

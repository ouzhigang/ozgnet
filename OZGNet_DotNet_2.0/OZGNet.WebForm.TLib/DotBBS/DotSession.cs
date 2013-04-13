using System;
using System.Web;

namespace OZGNet.WebForm.ThirdPart.DotBBS
{
    /// <summary>
    /// DotBBS Session帮助类
    /// </summary>
    public class DotSession
    {
        /// <summary>
        /// 添加Session
        /// </summary>
        /// <param name="Key">Session名称</param>
        /// <param name="Value">Session值</param>
        public static void Add(string Key, string Value)
        {
            HttpContext.Current.Session[Key] = Value;
        }
        /// <summary>
        /// 删除Session
        /// </summary>
        /// <param name="Key">Session名称</param>
        public static void Delete(string Key)
        {
            HttpContext.Current.Session[Key] = "";
        }
        /// <summary>
        /// 检查指定Session是否存在
        /// </summary>
        /// <param name="Key">Session名称</param>
        /// <returns></returns>
        public static bool Exists(string Key)
        {
            bool flag2;
            try
            {
                bool flag = true;
                if ((HttpContext.Current.Session[Key] == null) || (HttpContext.Current.Session[Key].ToString() == ""))
                {
                    flag = false;
                }
                flag2 = flag;
            }
            catch (Exception exception)
            {
                throw new Exception(Key + " " + exception.Message);
            }
            return flag2;
        }
        /// <summary>
        /// 获取Session
        /// </summary>
        /// <param name="Key">Session名称</param>
        /// <returns></returns>
        public static object Read(string Key)
        {
            if (Exists(Key))
            {
                return HttpContext.Current.Session[Key];
            }
            return "";
        }

    }
}


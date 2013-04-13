using System;
using System.Web;
using System.Web.Caching;
using System.Text;

namespace OZGNet.WebForm.ThirdPart.Jumbot
{
    /// <summary>
    /// Cache操作类
    /// </summary>
    public static class Cache
    {
        /// <summary>
        /// 简单创建/修改Cache
        /// </summary>
        /// <param name="strCacheName">Cache名称</param>
        /// <param name="objValue">Cache值</param>
        /// <param name="iExpires">有效期，秒数（使用的是当前时间+秒数得到一个绝对到期值）</param>
        /// <param name="priority">保留优先级，1最不会被清除，6最容易被内存管理清除（1:NotRemovable；2:High；3:AboveNormal；4:Normal；5:BelowNormal；6:Low）</param>
        public static void Insert(string strCacheName, object objValue, int iExpires, int priority)
        {
            TimeSpan ts = new TimeSpan(0, 0, iExpires);
            CacheItemPriority cachePriority;
            switch (priority)
            {
                case 6:
                    cachePriority = CacheItemPriority.Low;
                    break;
                case 5:
                    cachePriority = CacheItemPriority.BelowNormal;
                    break;
                case 4:
                    cachePriority = CacheItemPriority.Normal;
                    break;
                case 3:
                    cachePriority = CacheItemPriority.AboveNormal;
                    break;
                case 2:
                    cachePriority = CacheItemPriority.High;
                    break;
                case 1:
                    cachePriority = CacheItemPriority.NotRemovable;
                    break;
                default:
                    cachePriority = CacheItemPriority.Default;
                    break;
            }
            HttpContext.Current.Cache.Insert(strCacheName, objValue, null, DateTime.Now.Add(ts), System.Web.Caching.Cache.NoSlidingExpiration, cachePriority, null);
        }
        /// <summary>
        /// 简单读Cache对象的值
        /// </summary>
        /// <param name="strCacheName">Cache名称</param>
        /// <returns>Cache字符串值</returns>
        public static object Get(string strCacheName)
        {
            return HttpContext.Current.Cache[strCacheName];
        }
        /// <summary>
        /// 删除Cache对象
        /// </summary>
        /// <param name="strCacheName">Cache名称</param>
        public static void Del(string strCacheName)
        {
            HttpContext.Current.Cache.Remove(strCacheName);
        }
    }
}

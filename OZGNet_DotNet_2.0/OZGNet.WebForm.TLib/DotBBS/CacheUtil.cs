using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;

namespace OZGNet.WebForm.ThirdPart.DotBBS
{    
    /// <summary>
    /// DotBBS缓存工具类
    /// </summary>
    public class CacheUtil
    {
        /// <summary>
        /// 清空全部缓存
        /// </summary>
        public static void Clear()
        {
            IDictionaryEnumerator enumerator = HttpContext.Current.Cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Remove(enumerator.Key.ToString());
            }
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="strKey">缓存名称</param>
        /// <param name="valueObj">缓存值</param>
        /// <param name="durationSecond">超时(秒)</param>
        /// <returns></returns>
        public static bool Insert(string strKey, object valueObj, double durationSecond)
        {
            if (((strKey != null) && (strKey.Length != 0)) && (valueObj != null))
            {
                CacheItemRemovedCallback onRemoveCallback = new CacheItemRemovedCallback(CacheUtil.onRemove);
                HttpContext.Current.Cache.Insert(strKey, valueObj, null, DateTime.Now.AddSeconds(durationSecond), Cache.NoSlidingExpiration, CacheItemPriority.Normal, onRemoveCallback);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 指定一个缓存是否存在
        /// </summary>
        /// <param name="strKey">缓存名称</param>
        /// <returns></returns>
        public static bool IsExist(string strKey)
        {
            return (HttpContext.Current.Cache[strKey] != null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="obj"></param>
        /// <param name="reason"></param>
        private static void onRemove(string strKey, object obj, CacheItemRemovedReason reason)
        {
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="strKey">缓存名称</param>
        /// <returns></returns>
        public static object Read(string strKey)
        {
            if (HttpContext.Current.Cache[strKey] == null)
            {
                return null;
            }
            object obj2 = HttpContext.Current.Cache[strKey];
            if (obj2 == null)
            {
                return null;
            }
            return obj2;
        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="strKey">缓存名称</param>
        public static void Remove(string strKey)
        {
            if (HttpContext.Current.Cache[strKey] != null)
            {
                HttpContext.Current.Cache.Remove(strKey);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        public static void RemoveByRegexp(string pattern)
        {
            if (pattern != "")
            {
                IDictionaryEnumerator enumerator = HttpContext.Current.Cache.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string input = enumerator.Key.ToString();
                    if (Regex.IsMatch(input, pattern))
                    {
                        Remove(input);
                    }
                }
            }
        }
    }
}


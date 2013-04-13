using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Caching;
using OZGNet.WebForm.Cache.Options;

namespace OZGNet.WebForm.Cache
{
    /// <summary>
    /// 使用系统自带的Cache类
    /// </summary>
    public class SimpleCache : ICache
    {
        object _ConfigObject = null;
        DateTime _ExpiryTime = DateTime.Now;
        string _FilePath = null;

        public SimpleCache()
        { 
        
        }
        public SimpleCache(SimpleCacheMode configObject)
        {
            this.ConfigObject = configObject;
        }

        /// <summary>
        /// 共用配置对象
        /// </summary>
        public object ConfigObject
        {
            get
            {
                return this._ConfigObject;
            }
            set
            {
                this._ConfigObject = value;
            }
        }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpiryTime
        {
            get
            {
                return this._ExpiryTime;
            }
            set
            {
                this._ExpiryTime = value;
            }
        }
        /// <summary>
        /// 文件路径（文件模式时用到）
        /// </summary>
        public string FilePath
        {
            get
            {
                return this._FilePath;
            }
            set
            {
                this._FilePath = value;
            }
        }

        #region ICache 成员
        /// <summary>
        /// 写入缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Set(string key, object val)
        {
            this.Set(key, val, this.ExpiryTime);
        }
        /// <summary>
        /// 写入缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="expiryTime">过期时间</param>
        public void Set(string key, object val, DateTime expiryTime)
        {
            SimpleCacheMode configObject = (SimpleCacheMode)this.ConfigObject;
            if (configObject == SimpleCacheMode.Absolute)
            {
                Utility.CacheAbsolute(key, val, expiryTime);
            }
            else if (configObject == SimpleCacheMode.Relative)
            {
                Utility.CacheRelative(key, val, expiryTime);
            }
            else if (configObject == SimpleCacheMode.File)
            {
                Utility.CacheFile(key, val, this.FilePath);
            }
            else 
            {
                Utility.CacheAbsolute(key, val, expiryTime);
            }
        }
        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
            System.Web.Caching.Cache cache = System.Web.HttpContext.Current.Cache;
            return cache[key] != null ? cache[key] : null;
        }
        /// <summary>
        /// 删除缓存对象
        /// </summary>
        /// <param name="key"></param>
        public void Delete(string key)
        {
            System.Web.Caching.Cache cache = System.Web.HttpContext.Current.Cache;
            cache.Remove(key);
        }
        /// <summary>
        /// 缓存对象是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            return this.Get(key) != null ? true : false;
        }

        #endregion
    }
}

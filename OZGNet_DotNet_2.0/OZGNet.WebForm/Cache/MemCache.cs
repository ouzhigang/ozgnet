using System;
using System.Collections.Generic;
using System.Text;
using Memcached.ClientLibrary;

namespace OZGNet.WebForm.Cache
{
    /// <summary>
    /// 依赖Memcached.ClientLibrary.dll
    /// </summary>
    public class MemCache : ICache
    {
        object _ConfigObject = null;
        DateTime _ExpiryTime = DateTime.Now;
        SockIOPool pool = null;
        MemcachedClient mc = null;

        public MemCache()
        {
            this.Initialize();
        }
        public MemCache(object configObject)
        {
            this.ConfigObject = configObject;

            if (configObject is string[])
            {
                this.Initialize();
                this.pool.SetServers(configObject as string[]);
            }
            else 
            {
                this.pool = this.ConfigObject as SockIOPool;
            }
        }

        #region 私有方法
        /// <summary>
        /// 实例化SockIOPool
        /// </summary>
        protected void Initialize()
        {
            //初始化池
            if (this.pool == null)
            {
                this.pool = SockIOPool.GetInstance();
                this.pool.InitConnections = 3;
                this.pool.MinConnections = 3;
                this.pool.MaxConnections = 5;
                this.pool.SocketConnectTimeout = 1000;
                this.pool.SocketTimeout = 3000;
                this.pool.MaintenanceSleep = 30;
                this.pool.Failover = true;
                this.pool.Nagle = false;
            }
        }

        /// <summary>
        /// 实例化MemcachedClient
        /// </summary>
        protected void InitializeMemCache()
        {
            if (this.mc == null)
            {
                if (this._ConfigObject is string[])
                {
                    this.pool.SetServers(this._ConfigObject as string[]);
                }

                this.pool.Initialize();
                this.mc = new MemcachedClient();
                this.mc.EnableCompression = false;
            }
        }
        
        #endregion

        #region ICache 成员
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
            this.InitializeMemCache();
            this.mc.Set(key, val, expiryTime);
        }
        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
            this.InitializeMemCache();
            return this.mc.Get(key);
        }
        /// <summary>
        /// 删除缓存对象
        /// </summary>
        /// <param name="key"></param>
        public void Delete(string key)
        {
            this.InitializeMemCache();
            this.mc.Delete(key);
        }
        /// <summary>
        /// 缓存对象是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            this.InitializeMemCache();
            return this.mc.KeyExists(key);
        }

        #endregion
    }
}

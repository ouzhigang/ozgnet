// ================================================
// DataCache.cs, 通用缓存基类
// Copyright (C) 2010 Benking
// mail:ben.wangzj@gmail.com
// website:http://www.dsjian.com/
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// ================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace OZGNet.WebForm.ThirdPart.DsJian.Cache
{
    /// <summary>
    /// 通用缓存基类。
    /// </summary>
    /// <typeparam name="TKey">键类型</typeparam>
    /// <typeparam name="TValue">值类型</typeparam>
    public class DataCache<TKey, TValue> : IDataCache<TKey, TValue>
    {
        private Dictionary<TKey, TValue> _Cache = null;
        private bool _IsChanged = true;

        /// <summary>
        /// 
        /// </summary>
        public DataCache()
        {
            _Cache = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// 
        /// </summary>
        protected Dictionary<TKey, TValue> Cache
        {
            get { return _Cache; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsChanged
        {
            get { return _IsChanged; }
            protected set { _IsChanged = value; }
        }

        #region ICommonCache<TValue> 成员
        /// <summary>
        /// 获取缓存项的总数
        /// </summary>
        public int Count
        {
            get { return _Cache.Count; }
        }
        /// <summary>
        /// 获取指定缓存值
        /// </summary>
        /// <param name="key">缓存项</param>
        /// <returns></returns>
        public virtual TValue Get(TKey key)
        {
            lock (_Cache)
            {
                if (_Cache.ContainsKey(key))
                {
                    return (TValue)_Cache[key];
                }
                else
                {
                    return default(TValue);
                }
            }
        }
        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="key">缓存项</param>
        /// <param name="value">缓存值</param>
        public virtual void Add(TKey key, TValue value)
        {
            _Cache[key] = value;
            this.IsChanged = true;
        }
        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="key">缓存项</param>
        /// <param name="value">缓存值</param>
        public virtual void Set(TKey key, TValue value)
        {
            _Cache[key] = value;
            this.IsChanged = true;
        }
        /// <summary>
        /// 指定缓存项是否存在
        /// </summary>
        /// <param name="key">缓存项</param>
        /// <returns></returns>
        public bool Contains(TKey key)
        {
            lock (_Cache)
            {
                return _Cache.ContainsKey(key);
            }
        }
        /// <summary>
        /// 删除指定缓存
        /// </summary>
        /// <param name="key">缓存项</param>
        public void Remove(TKey key)
        {
            lock (_Cache)
            {
                _Cache.Remove(key);
                this.IsChanged = true;
            }
        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TKey> GetKeys()
        {
            return new List<TKey>(this.Cache.Keys);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TValue> GetValues()
        {
            return new List<TValue>(this.Cache.Values);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public List<TValue> GetSortedList(IComparer<TValue> comparer)
        {
            List<TValue> values = new List<TValue>(this.Cache.Values);
            values.Sort(comparer);
            return values;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public List<TValue> GetSortedList(Comparison<TValue> comparison)
        {
            List<TValue> values = new List<TValue>(this.Cache.Values);
            values.Sort(comparison);
            return values;
        }
        /// <summary>
        /// 清空缓存
        /// </summary>
        public void Clear()
        {
            lock (_Cache)
            {
                _Cache.Clear();
                this.IsChanged = true;
            }
        }
    }

    /// <summary>
    /// 有时间过期机制的缓存类
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class ExpiredDataCache<TKey, TValue> : IDataCache<TKey, TValue>, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<TKey, ExpiredData<TValue>> _Cache = null;
        /// <summary>
        /// 
        /// </summary>
        protected TimeSpan _ExpireSpan;

        private System.Timers.Timer _CheckExpire;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expireSpan"></param>
        public ExpiredDataCache(TimeSpan expireSpan)
        {
            _Cache = new Dictionary<TKey, ExpiredData<TValue>>();
            _ExpireSpan = expireSpan;

            _CheckExpire = new System.Timers.Timer();
            _CheckExpire.Interval = 60000;
            _CheckExpire.Enabled = true;
            _CheckExpire.Elapsed += new System.Timers.ElapsedEventHandler(_CheckExpire_Elapsed);
        }

        void _CheckExpire_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var removeQueue = new Queue<TKey>();
            foreach (var item in this._Cache)
            {
                if (item.Value.ExpireTime < DateTime.Now)
                {
                    removeQueue.Enqueue(item.Key);
                }
            }

            while (removeQueue.Count > 0)
            {
                this.Remove(removeQueue.Dequeue());
            }
        }

        #region IDataCache<TValue> 成员

        /// <summary>
        /// 缓存项总数
        /// </summary>
        public int Count
        {
            get { return _Cache.Count; }
        }
        /// <summary>
        /// 获取指定缓存值
        /// </summary>
        /// <param name="key">缓存项</param>
        /// <returns></returns>
        public virtual TValue Get(TKey key)
        {
            lock (_Cache)
            {
                if (_Cache.ContainsKey(key))
                {
                    var item = _Cache[key].GetItem();
                    if (item == null)
                    {
                        _Cache.Remove(key);
                    }
                    return item;
                }
                else
                {
                    return default(TValue);
                }
            }
        }
        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="key">缓存项</param>
        /// <param name="value">缓存值</param>
        public virtual void Add(TKey key, TValue value)
        {
            this.Set(key, value);
        }
        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="key">缓存项</param>
        /// <param name="value">缓存值</param>
        public virtual void Set(TKey key, TValue value)
        {
            var item = new ExpiredData<TValue>(this._ExpireSpan, value);
            _Cache[key] = item;
        }
        /// <summary>
        /// 指定缓存项是否存在
        /// </summary>
        /// <param name="key">缓存项</param>
        /// <returns></returns>
        public bool Contains(TKey key)
        {
            lock (_Cache)
            {
                return _Cache.ContainsKey(key);
            }
        }
        /// <summary>
        /// 删除指定缓存
        /// </summary>
        /// <param name="key">缓存项</param>
        public void Remove(TKey key)
        {
            lock (_Cache)
            {
                _Cache.Remove(key);
            }
        }

        #endregion
        /// <summary>
        /// 清空缓存
        /// </summary>
        public void Clear()
        {
            lock (_Cache)
            {
                _Cache.Clear();
            }

            _CheckExpire.Enabled = false;
            _CheckExpire.Dispose();
        }

        class ExpiredData<T>
        {
            public DateTime ExpireTime { get; set; }

            public Int32 VisitTimes { get; set; }

            public ExpiredData(TimeSpan expireSpan, T item)
            {
                ExpireTime = DateTime.Now.Add(expireSpan);
                Item = item;
            }

            protected T Item { get; set; }

            public T GetItem()
            {
                if (ExpireTime >= DateTime.Now)
                {
                    VisitTimes++;
                    return Item;
                }

                return default(T);
            }
        }

        #region IDisposable 成员
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _CheckExpire.Enabled = false;
            _CheckExpire.Dispose();
        }

        #endregion
    }
}

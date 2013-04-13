// ================================================
// IDataCache.cs, 通用缓存接口
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
    /// 通用缓存
    /// </summary>
    /// <typeparam name="TKey">键类型</typeparam>
    /// <typeparam name="TValue">值类型</typeparam>
    public interface IDataCache<TKey, TValue>
    {
        /// <summary>
        /// 缓存记录条数
        /// </summary>
        int Count { get; }
        /// <summary>
        /// 获取缓存项
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        TValue Get(TKey key);
        /// <summary>
        /// 添加缓存项
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        void Add(TKey key, TValue value);
        /// <summary>
        /// 修改缓存项
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        void Set(TKey key, TValue value);
        /// <summary>
        /// 检测是否包含某项
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>为true表示包含此键</returns>
        Boolean Contains(TKey key);
        /// <summary>
        /// 移除项
        /// </summary>
        /// <param name="key">键</param>
        void Remove(TKey key);
    }
}

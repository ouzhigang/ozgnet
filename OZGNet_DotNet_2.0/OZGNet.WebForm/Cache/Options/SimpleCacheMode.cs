using System;
using System.Collections.Generic;
using System.Text;

namespace OZGNet.WebForm.Cache.Options
{
    public enum SimpleCacheMode : int
    {
        /// <summary>
        /// 绝对模式
        /// </summary>
        Absolute = 1,
        /// <summary>
        /// 相对模式
        /// </summary>
        Relative = 2,
        /// <summary>
        /// 文件模式
        /// </summary>
        File = 3
    }
}

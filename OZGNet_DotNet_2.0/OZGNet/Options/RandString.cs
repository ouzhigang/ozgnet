using System;
using System.Collections.Generic;
using System.Text;

namespace OZGNet.Options
{
    /// <summary>
    /// 枚举(获取随机字符Util.GetRandString())
    /// </summary>
    public enum RandString : int
    {
        /// <summary>
        /// 全部小写字母
        /// </summary>
        Lower = 1,
        /// <summary>
        /// 全部大写字母
        /// </summary>
        Upper = 2,
        /// <summary>
        /// 全部数字
        /// </summary>
        Number = 3,
        /// <summary>
        /// 大写字母和小写字母
        /// </summary>
        LowerAndUpper = 4,
        /// <summary>
        /// 大写字母和数字
        /// </summary>
        UpperAndNumber = 5,
        /// <summary>
        /// 小写字母和数字
        /// </summary>
        LowerAndNumber = 6,
        /// <summary>
        /// 大写字母,小写字母和数字
        /// </summary>
        All = 7
    }
}

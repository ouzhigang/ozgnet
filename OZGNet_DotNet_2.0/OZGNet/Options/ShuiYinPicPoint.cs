using System;
using System.Collections.Generic;
using System.Text;

namespace OZGNet.Options
{
    /// <summary>
    /// 打水印图的位置
    /// </summary>
    public enum ShuiYinPicPoint : int
    {
        /// <summary>
        /// 左上
        /// </summary>
        LeftTop = 1,
        /// <summary>
        /// 中上
        /// </summary>
        MiddleTop = 2,
        /// <summary>
        /// 右上
        /// </summary>
        RightTop = 3,
        /// <summary>
        /// 左中
        /// </summary>
        LeftMiddle = 4,
        /// <summary>
        /// 中间
        /// </summary>
        Middle = 5,
        /// <summary>
        /// 右中
        /// </summary>
        RightMiddle = 6,
        /// <summary>
        /// 左下
        /// </summary>
        LeftDown = 7,
        /// <summary>
        /// 中下
        /// </summary>
        MiddleDown = 8,
        /// <summary>
        /// 右下
        /// </summary>
        RightDown = 9
    }
}

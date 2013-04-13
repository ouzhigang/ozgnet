using System;
using System.Collections.Generic;
using System.Text;

namespace OZGNet.WebForm.Options
{
    /// <summary>
    /// 选择重定向方式(UtilClient.Redirect())
    /// </summary>
    public enum RedirectTarget : int
    {
        /// <summary>
        /// 当前页
        /// </summary>
        Current = 1,
        /// <summary>
        /// 父框架
        /// </summary>
        Parent = 2
    }
}

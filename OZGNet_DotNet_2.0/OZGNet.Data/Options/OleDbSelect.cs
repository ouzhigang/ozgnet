using System;
using System.Collections.Generic;
using System.Text;

namespace OZGNet.Data.Options
{
    /// <summary>
    /// 选择OleDb支持的种类
    /// </summary>
    public enum OleDbSelect : int
    {
        /// <summary>
        /// 支持Access2000 or 2003
        /// </summary>
        Access = 1,
        /// <summary>
        /// 支持Excel2000 or 2003
        /// </summary>
        Excel = 2,
        /// <summary>
        /// 支持Access2007
        /// </summary>
        Access2007 = 3,
        /// <summary>
        /// 支持Excel2007
        /// </summary>
        Excel2007 = 4
    }
}

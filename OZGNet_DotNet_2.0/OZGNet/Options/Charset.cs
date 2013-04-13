using System;
using System.Collections.Generic;
using System.Web;

namespace OZGNet.Options
{
    /// <summary>
    /// 字符编码
    /// </summary>
    public class Charset
    {
        /// <summary>
        /// GB2312编码(简体中文)
        /// </summary>
        public static string GB2312
        {
            get
            {
                return "GB2312";
            }
        }

        /// <summary>
        /// GBK编码(简体中文)
        /// </summary>
        public static string GBK
        {
            get
            {
                return "GBK";
            }
        }

        /// <summary>
        /// Big5编码(繁体中文)
        /// </summary>
        public static string Big5
        {
            get
            {
                return "Big5";
            }
        }

        /// <summary>
        /// UTF-7编码
        /// </summary>
        public static string UTF7
        {
            get
            {
                return "UTF-7";
            }
        }

        /// <summary>
        /// UTF-8编码
        /// </summary>
        public static string UTF8
        {
            get
            {
                return "UTF-8";
            }
        }

        /// <summary>
        /// UTF-32编码
        /// </summary>
        public static string UTF32
        {
            get
            {
                return "UTF-32";
            }
        }

        /// <summary>
        /// Shift_JIS编码（日文）
        /// </summary>
        public static string Shift_JIS
        {
            get
            {
                return "Shift_JIS";
            }
        }

        /// <summary>
        /// JIS编码（日文）
        /// </summary>
        public static string JIS
        {
            get
            {
                return "JIS";
            }
        }

        /// <summary>
        /// EUC编码（日文）
        /// </summary>
        public static string EUC
        {
            get
            {
                return "EUC";
            }
        }


    }
}

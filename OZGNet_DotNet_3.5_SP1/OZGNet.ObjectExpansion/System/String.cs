using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OZGNet.ObjectExpansion.System
{
    /// <summary>
    /// System.String的扩展方法，依赖OZGNet.dll
    /// </summary>
    public static class String
    {
        /// <summary>
        /// 取指定字符
        /// </summary>
        /// <param name="str">目标字符</param>
        /// <param name="i">字符索引</param>
        /// <returns></returns>
        public static string StringAt(this string str, int i)
        {
            return Utility.StringAt(str, i);
        }

        /// <summary>
        /// 取开始到结束的值
        /// </summary>
        /// <param name="content">目标字符</param>
        /// <param name="start">开始字符</param>
        /// <param name="end">结束字符</param>
        /// <returns></returns>
        public static string GetStartToEndString(this string content, string start, string end)
        {
            return Utility.GetStartToEndString(content, start, end);
        }

        /// <summary>
        /// 字符转List(针对每个字符)
        /// </summary>
        /// <param name="str">目标字符</param>
        /// <returns></returns>
        public static string[] Split(this string str)
        {
            return Utility.StrSplit(str).ToArray();
        }

        /// <summary>
        /// 字符转List
        /// </summary>
        /// <param name="str">目标字符</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string[] Split(this string str, string separator)
        {
            return Utility.StrSplit(str, separator).ToArray();
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="inputString">目标字符串</param>
        /// <param name="len">长度</param>
        /// <returns>返回截取后的字符</returns>
        public static string GetTruncate(this string inputString, int len)
        {
            return Utility.GetTruncate(inputString, len, "...");
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="inputString">目标字符串</param>
        /// <param name="len">长度</param>
        /// <param name="Omission">返回截取后的字符</param>
        /// <returns></returns>
        public static string GetTruncate(this string inputString, int len, string Omission)
        {
            return Utility.GetTruncate(inputString, len, Omission);
        }

        /// <summary>
        /// QQ MD5加密
        /// </summary>
        /// <param name="str">目标字符</param>
        /// <returns></returns>
        public static string ToQQHash(this string str)
        {
            return Utility.ToQQHash(str);
        }

        /// <summary>
        /// 使用md5算法
        /// </summary>
        /// <param name="str">目标字符串</param>
        /// <returns>返回转换MD5后的字符</returns>
        public static string ToMD5(this string str)
        {
            return Utility.ToMD5(str);
        }

        /// <summary>
        /// 使用SHA1算法
        /// </summary>
        /// <param name="str">目标字符串</param>
        /// <returns>返回转换shs1后的字符</returns>
        public static string ToSHA1(this string str)
        {
            return Utility.ToSHA1(str);
        }

        /// <summary>
        /// 使用SHA256算法
        /// </summary>
        /// <param name="str">目标字符串</param>
        /// <returns>返回转换SHA256后的字符</returns>
        public static string ToSHA256(this string str)
        {
            return Utility.ToSHA256(str);
        }

        /// <summary>
        /// 使用SHA384算法
        /// </summary>
        /// <param name="str">目标字符串</param>
        /// <returns>返回转换SHA384后的字符</returns>
        public static string ToSHA384(this string str)
        {
            return Utility.ToSHA384(str);
        }

        /// <summary>
        /// 使用SHA512算法
        /// </summary>
        /// <param name="str">目标字符串</param>
        /// <returns>返回转换SHA512后的字符</returns>
        public static string ToSHA512(this string str)
        {
            return Utility.ToSHA512(str);
        }

        /// <summary>
        /// 得到字符串长度，一个汉字长度为2
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static int GetRealLength(this string inputString)
        {
            return Utility.StrLength(inputString);
        }

        /// <summary>
        /// 转繁体
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string ToBIG(this string src)
        {
            return Utility.ToBIG(src);
        }

        /// <summary>
        /// 转简体
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string ToGB(this string src)
        {
            return Utility.ToGB(src);
        }

        /// <summary>
        /// 不区分大小写的替换
        /// </summary>
        /// <param name="original">原字符串</param>
        /// <param name="pattern">需替换字符</param>
        /// <param name="replacement">被替换内容</param>
        /// <returns></returns>
        public static string ReplaceEx(this string original, string pattern, string replacement)
        {
            return Utility.ReplaceEx(original, pattern, replacement);
        }

        /// <summary>
        /// 替换html中的特殊字符
        /// </summary>
        /// <param name="theString">需要进行替换的文本。</param>
        /// <returns>替换完的文本。</returns>
        public static string HtmlEncode(this string theString)
        {
            return Utility.HtmlEncode(theString);
        }

        /// <summary>
        /// 恢复html中的特殊字符
        /// </summary>
        /// <param name="theString">需要恢复的文本。</param>
        /// <returns>恢复好的文本。</returns>
        public static string HtmlDecode(this string theString)
        {
            return Utility.HtmlDecode(theString);
        }

        /// <summary>
        /// 字符转Unicode
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns></returns>
        public static string ToUnicode(this string str)
        {
            return Utility.StrToUnicode(str);
        }

        /// <summary>
        /// Unicode转字符
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns></returns>
        public static string ToDeUnicode(this string str)
        {
            return Utility.UnicodeToStr(str);
        }

    }
}

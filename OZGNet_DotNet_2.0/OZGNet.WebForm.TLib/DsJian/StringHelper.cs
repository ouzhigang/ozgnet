using System;
using System.Collections.Generic;
using System.Text;

namespace OZGNet.WebForm.ThirdPart.DsJian
{
    /// <summary>
    /// 字符串操作帮助类
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// 将数组打包成字符串
        /// </summary>
        /// <param name="list">列表</param>
        /// <returns></returns>
        public static string ArrayToString(params string[] list)
        {
            if (list.Length <= 0)
            {
                return string.Empty;
            }

            StringBuilder sbLengthLine = new StringBuilder();
            StringBuilder sbStringLine = new StringBuilder();

            for (int i = 0; i < list.Length; i++)
            {
                sbLengthLine.AppendFormat(":{0}", list[i].Length);
                sbStringLine.Append(list[i]);
            }

            return sbStringLine.ToString() + "$|^$^|$" + sbLengthLine.ToString().Substring(1);
        }

        /// <summary>
        /// 将对象数组打包成字符串
        /// </summary>
        /// <param name="list">对象列表</param>
        /// <returns></returns>
        public static string ArrayToString(params object[] list)
        {
            string[] strList = new string[list.Length];
            for (int i = 0; i < list.Length; ++i)
            {
                if (list[i] is string)
                {
                    strList[i] = (string)list[i];
                }
                else
                {
                    strList[i] = Convert.ToString(list[i]);
                }
            }
            return ArrayToString(strList);
        }

        /// <summary>
        /// 将字符串分解成字符串数组
        /// </summary>
        /// <param name="strStringLine"></param>
        /// <returns></returns>
        public static string[] StringToArray(string strStringLine)
        {
            int index = strStringLine.LastIndexOf("$|^$^|$");
            if (index == -1)
            {
                return new string[] { strStringLine };
            }

            string strLengthLine = strStringLine.Substring(index + 7);
            string[] aryLength = strLengthLine.Split(new char[] { ':' });
            string[] list = new string[aryLength.Length];
            int start = 0;
            for (int i = 0; i < list.Length; ++i)
            {
                int length = int.Parse(aryLength[i]);
                list[i] = strStringLine.Substring(start, length);
                start += length;
            }

            return list;
        }

        /// <summary>
        /// 转换特殊字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string JSEncode(string str)
        {
            StringBuilder sb = new StringBuilder(str);
            sb.Replace("\\", "\\\\");
            sb.Replace("\"", "\\\"");
            sb.Replace("\r\n", "\\n");
            sb.Replace("'", "\\'");

            return sb.ToString();
        }

        /// <summary>
        /// 转换特殊字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string JSEncodeString(string str)
        {
            StringBuilder sb = new StringBuilder(str);
            sb.Replace("\\", "\\\\");
            sb.Replace("\"", "\\\"");
            sb.Replace("\r\n", "");
            sb.Replace("\n", "");
            sb.Replace("\r", "");
            sb.Replace("'", "\\'");

            return sb.ToString();
        }

        /// <summary>
        /// 过滤屏蔽的字符串
        /// </summary>
        /// <param name="source"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static string FilterString(string source, string[] filter)
        {
            StringBuilder sb = new StringBuilder(source);

            foreach (String str in filter)
            {
                sb.Replace(str, "*");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 从中间截断字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CutMiddle(string str, int length)
        {
            return CutMiddle(str, length, "...");
        }

        /// <summary>
        /// 从中间截断字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <param name="cutStr"></param>
        /// <returns></returns>
        public static string CutMiddle(string str, int length, string cutStr)
        {
            if (length < GetBitLength(str))
            {
                return (GetLeftByteString(str, length / 2) + cutStr + GetRightByteString(str, length / 2));
            }
            return str;
        }

        /// <summary>
        /// 截断右边多余字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CutRight(string str, int length)
        {
            return CutRight(str, length, "...");
        }

        /// <summary>
        /// 截断右边多余字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <param name="cutStr"></param>
        /// <returns></returns>
        public static string CutRight(string str, int length, string cutStr)
        {
            if (length < GetBitLength(str))
            {
                return (GetLeftByteString(str, length) + cutStr);
            }
            return str;
        }

        /// <summary>
        /// 获取某字符串字节长度
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetBitLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="byteLength"></param>
        /// <returns></returns>
        public static string GetLeftByteString(string str, int byteLength)
        {
            char[] chArray;
            if (str.Length <= (byteLength / 2))
            {
                return str;
            }
            StringBuilder builder = new StringBuilder();
            int num = 0;
            if (str.Length < byteLength)
            {
                chArray = str.ToCharArray();
            }
            else
            {
                chArray = str.ToCharArray(0, byteLength);
            }
            foreach (char ch in chArray)
            {
                if (ch > '\x007f')
                {
                    num += 2;
                }
                else
                {
                    num++;
                }
                builder.Append(ch);
                if (num >= byteLength)
                {
                    break;
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="byteLength"></param>
        /// <returns></returns>
        public static string GetRightByteString(string str, int byteLength)
        {
            char[] chArray;
            if (str.Length <= (byteLength / 2))
            {
                return str;
            }
            StringBuilder builder = new StringBuilder();
            int num = 0;
            if (str.Length < byteLength)
            {
                chArray = str.ToCharArray();
            }
            else
            {
                chArray = str.ToCharArray(str.Length - byteLength, byteLength);
            }
            for (int i = chArray.Length - 1; i >= 0; i--)
            {
                if (chArray[i] > '\x007f')
                {
                    num += 2;
                }
                else
                {
                    num++;
                }
                builder.Insert(0, chArray[i]);
                if (num >= byteLength)
                {
                    break;
                }
            }
            return builder.ToString();
        }
    } 
}

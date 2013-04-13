using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;

namespace OZGNet.WebForm.ThirdPart.YXShop
{
    /// <summary>
    /// YXShop 页面验证类
    /// </summary>
    public class PageValidate
    {
        private static Regex RegCHZN = new Regex("[一-龥]");
        private static Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");
        private static Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$");
        private static Regex RegEmail = new Regex(@"^[\w-]+@[\w-]+\.(com|net|org|edu|mil|tv|biz|info)$");
        private static Regex RegNumber = new Regex("^[0-9]+$");
        private static Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
        /// <summary>
        /// HTML解码
        /// </summary>
        /// <param name="str">输入内容</param>
        /// <returns></returns>
        public static string Decode(string str)
        {
            str = str.Replace("<br>", "\n");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&quot;", "\"");
            return str;
        }
        /// <summary>
        /// HTML编码
        /// </summary>
        /// <param name="str">输入内容</param>
        /// <returns></returns>
        public static string Encode(string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("'", "''");
            str = str.Replace("\"", "&quot;");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\n", "<br>");
            return str;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="inputKey"></param>
        /// <param name="maxLen"></param>
        /// <returns></returns>
        public static string FetchInputDigit(HttpRequest req, string inputKey, int maxLen)
        {
            string retVal = string.Empty;
            if ((inputKey != null) && (inputKey != string.Empty))
            {
                retVal = req.QueryString[inputKey];
                if (null == retVal)
                {
                    retVal = req.Form[inputKey];
                }
                if (null != retVal)
                {
                    retVal = SqlText(retVal, maxLen);
                    if (!IsNumber(retVal))
                    {
                        retVal = string.Empty;
                    }
                }
            }
            if (retVal == null)
            {
                retVal = string.Empty;
            }
            return retVal;
        }
        /// <summary>
        /// HTML编码
        /// </summary>
        /// <param name="inputData">输入内容</param>
        /// <returns></returns>
        public static string HtmlEncode(string inputData)
        {
            return HttpUtility.HtmlEncode(inputData);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string InputText(string inputString, int maxLength)
        {
            StringBuilder retVal = new StringBuilder();
            if ((inputString != null) && (inputString != string.Empty))
            {
                inputString = inputString.Trim();
                if (inputString.Length > maxLength)
                {
                    inputString = inputString.Substring(0, maxLength);
                }
                for (int i = 0; i < inputString.Length; i++)
                {
                    switch (inputString[i])
                    {
                        case '<':
                        {
                            retVal.Append("&lt;");
                            continue;
                        }
                        case '>':
                        {
                            retVal.Append("&gt;");
                            continue;
                        }
                        case '"':
                        {
                            retVal.Append("&quot;");
                            continue;
                        }
                    }
                    retVal.Append(inputString[i]);
                }
                retVal.Replace("'", " ");
            }
            return retVal.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsDecimal(string inputData)
        {
            return ((inputData == "0") || RegDecimal.Match(inputData).Success);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsDecimalSign(string inputData)
        {
            return RegDecimalSign.Match(inputData).Success;
        }
        /// <summary>
        /// 检查是否为邮件地址
        /// </summary>
        /// <param name="inputData">输入内容</param>
        /// <returns></returns>
        public static bool IsEmail(string inputData)
        {
            return RegEmail.Match(inputData).Success;
        }
        /// <summary>
        /// 检查是否存在中文
        /// </summary>
        /// <param name="inputData">输入内容</param>
        /// <returns></returns>
        public static bool IsHasCHZN(string inputData)
        {
            return RegCHZN.Match(inputData).Success;
        }
        /// <summary>
        /// 检查是否是整数
        /// </summary>
        /// <param name="inputData">输入内容</param>
        /// <returns></returns>
        public static bool IsNumber(string inputData)
        {
            return RegNumber.Match(inputData).Success;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsNumberSign(string inputData)
        {
            return RegNumberSign.Match(inputData).Success;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lbl"></param>
        /// <param name="inputObj"></param>
        public static void SetLabel(Label lbl, object inputObj)
        {
            SetLabel(lbl, inputObj.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lbl"></param>
        /// <param name="txtInput"></param>
        public static void SetLabel(Label lbl, string txtInput)
        {
            lbl.Text = HtmlEncode(txtInput);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlInput"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string SqlText(string sqlInput, int maxLength)
        {
            if ((sqlInput != null) && (sqlInput != string.Empty))
            {
                sqlInput = sqlInput.Trim();
                if (sqlInput.Length > maxLength)
                {
                    sqlInput = sqlInput.Substring(0, maxLength);
                }
            }
            return sqlInput;
        }
    }
}


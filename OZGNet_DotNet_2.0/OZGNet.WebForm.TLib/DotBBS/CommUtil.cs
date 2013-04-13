using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

namespace OZGNet.WebForm.ThirdPart.DotBBS
{    
    /// <summary>
    /// DotBBS工具类
    /// </summary>
    public class CommUtil
    {
        /// <summary>
        /// 计算时间差
        /// </summary>
        /// <param name="unit">输入的是：year,month,day,hour,minute,second</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        public static int DateDiff(string unit, DateTime startDate, DateTime endDate)
        {
            int num = 0;
            int num2 = endDate.Year - startDate.Year;
            int num3 = endDate.Month - startDate.Month;
            int num4 = endDate.Day - startDate.Day;
            int num5 = endDate.Hour - startDate.Hour;
            int num6 = endDate.Minute - startDate.Minute;
            int second = endDate.Second;
            int num7 = startDate.Second;
            if (unit == "year")
            {
                num = num2;
            }
            if (unit == "month")
            {
                num = (num2 * 12) + num3;
            }
            if (unit == "day")
            {
                num = (((num2 * 12) + num3) * 30) + num4;
            }
            if (unit == "hour")
            {
                num = (((((num2 * 12) + num3) * 30) + num4) * 0x18) + num5;
            }
            if (unit == "minute")
            {
                num = (((((((num2 * 12) + num3) * 30) + num4) * 0x18) + num5) * 60) + num6;
            }
            if (unit == "second")
            {
                num = ((((((((num2 * 12) + num3) * 30) + num4) * 0x18) + num5) * 60) + num6) * 60;
            }
            return num;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        public static void Debug(string str)
        {
            HttpContext.Current.Response.Write(str);
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="safeStart"></param>
        /// <returns></returns>
        public static string DotEnCrypt(string str, int safeStart)
        {
            string str2 = "";
            string str3 = "";
            int startIndex = safeStart;
            str = FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
            str3 = DotReverse(str.Substring(startIndex, str.Length - startIndex));
            str2 = DotReverse(str.Substring(0, startIndex));
            return (str3 + str2);
        }
        /// <summary>
        /// 反转字符
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns></returns>
        public static string DotReverse(string str)
        {
            string str2 = "";
            for (int i = str.Length - 1; i > -1; i--)
            {
                str2 = str2 + str.Substring(i, 1);
            }
            return str2;
        }
        /// <summary>
        /// 获取随机数字
        /// </summary>
        /// <param name="len">返回长度</param>
        /// <returns></returns>
        public static string GenRndNum(int len)
        {
            string[] strArray = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15".Split(new char[] { ',' });
            string str2 = string.Empty;
            string str3 = string.Empty;
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                int num = random.Next();
                str2 = strArray[num % 15];
                if (str2 == "0")
                {
                    str2 = "2";
                }
                str3 = str3 + str2;
            }
            return str3;
        }

        /// <summary>
        /// 随机字母
        /// </summary>
        /// <param name="len">返回长度</param>
        /// <returns></returns>
        public static string GenRndString(int len)
        {
            char[] chArray = ("ABCDEFGHIJKLMNOPQRSTUVWXYZ" + "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower()).ToCharArray();
            string str2 = string.Empty;
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                int num = random.Next();
                str2 = str2 + chArray[num % 50].ToString();
            }
            return str2;
        }
        /// <summary>
        /// 获取站点的目录
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetAppPath(int type)
        {
            if (type == 1)
            {
                return HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"];
            }
            string str2 = HttpContext.Current.Request.ServerVariables["path_translated"];
            return str2.Substring(0, str2.LastIndexOf(@"\") + 1);
        }
        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetDataTimeRandomFileName()
        {
            Random random = new Random(0x3e8);
            return (DateTime.Now.Date.Year.ToString() + DateTime.Now.Date.Month.ToString() + DateTime.Now.Date.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + random.Next().ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int GetInt(int num)
        {
            return int.Parse(num.ToString().Split(new char[] { '.' })[0]);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="slen"></param>
        /// <param name="showDot"></param>
        /// <returns></returns>
        public static string GetSpeStr(string str, int slen, bool showDot)
        {
            if (str.Length > slen)
            {
                if (showDot)
                {
                    str = str.Substring(0, slen) + "...";
                    return str;
                }
                str = str.Substring(0, slen);
            }
            return str;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumStr(string str)
        {
            bool flag = true;
            if (string.IsNullOrEmpty(str))
            {
                flag = false;
            }
            foreach (char ch in str)
            {
                if (!char.IsNumber(ch))
                {
                    return false;
                }
            }
            return flag;
        }
        /// <summary>
        /// 过滤JS
        /// </summary>
        /// <param name="str">目标字符</param>
        /// <param name="exclueStr">过滤的JS内容</param>
        /// <returns></returns>
        public static string JsFilter(string str, string exclueStr)
        {
            if (exclueStr.IndexOf("<script>,") < 0)
            {
                str = str.Replace("<script>", "");
            }
            if (exclueStr.IndexOf("</script>,") < 0)
            {
                str = str.Replace("</script>", "");
            }
            if (exclueStr.IndexOf("javascript,") < 0)
            {
                str = str.Replace("javascript", "");
            }
            if (exclueStr.IndexOf("/,") < 0)
            {
                str = str.Replace("/", "");
            }
            if (exclueStr.IndexOf("',") < 0)
            {
                str = str.Replace("'", "");
            }
            if (exclueStr.IndexOf(";,") < 0)
            {
                str = str.Replace(";", "");
            }
            if (exclueStr.IndexOf("&,") < 0)
            {
                str = str.Replace("&", "");
            }
            if (exclueStr.IndexOf("#,") < 0)
            {
                str = str.Replace("#", "");
            }
            return str;
        }
        /// <summary>
        /// 过滤HTML
        /// </summary>
        /// <param name="Htmlstring">输入内容</param>
        /// <returns></returns>
        public static string NoHTML(string Htmlstring)
        {
            Htmlstring = Regex.Replace(Htmlstring, "<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(iexcl|#161);", "\x00a1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(cent|#162);", "\x00a2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(pound|#163);", "\x00a3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(copy|#169);", "\x00a9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }
        /// <summary>
        /// 过滤SQL关键字
        /// </summary>
        /// <param name="str">输入内容</param>
        /// <param name="exclueStr">过滤的SQL关键字</param>
        /// <returns></returns>
        public static string SafeFilter(string str, string exclueStr)
        {
            str = str.Replace(" ", "");
            if (exclueStr.IndexOf("',") < 0)
            {
                str = str.Replace("'", "");
            }
            if (exclueStr.IndexOf(",,") < 0)
            {
                str = str.Replace(",", "");
            }
            if (exclueStr.IndexOf("-,") < 0)
            {
                str = str.Replace("-", "");
            }
            if (exclueStr.IndexOf("+,") < 0)
            {
                str = str.Replace("+", "");
            }
            if (exclueStr.IndexOf("%,") < 0)
            {
                str = str.Replace("%", "");
            }
            if (exclueStr.IndexOf("&,") < 0)
            {
                str = str.Replace("&", "");
            }
            if (exclueStr.IndexOf("$,") < 0)
            {
                str = str.Replace("$", "");
            }
            if (exclueStr.IndexOf("*,") < 0)
            {
                str = str.Replace("*", "");
            }
            if (exclueStr.IndexOf(".,") < 0)
            {
                str = str.Replace(".", "");
            }
            if (exclueStr.IndexOf("=,") < 0)
            {
                str = str.Replace("=", "");
            }
            if (exclueStr.IndexOf("(,") < 0)
            {
                str = str.Replace("(", "");
            }
            if (exclueStr.IndexOf("),") < 0)
            {
                str = str.Replace(")", "");
            }
            if (exclueStr.IndexOf("!,") < 0)
            {
                str = str.Replace("!", "");
            }
            if (exclueStr.IndexOf("@,") < 0)
            {
                str = str.Replace("@", "");
            }
            if (exclueStr.IndexOf("#,") < 0)
            {
                str = str.Replace("#", "");
            }
            if (exclueStr.IndexOf("^,") < 0)
            {
                str = str.Replace("^", "");
            }
            if (exclueStr.IndexOf("|,") < 0)
            {
                str = str.Replace("|", "");
            }
            if (exclueStr.IndexOf(":,") < 0)
            {
                str = str.Replace(":", "");
            }
            if (exclueStr.IndexOf("xp_cmdshell,") < 0)
            {
                str = str.Replace("xp_cmdshell", "");
            }
            if (exclueStr.IndexOf("/add,") < 0)
            {
                str = str.Replace("/add", "");
            }
            str = str.Replace("exec master.dbo.xp_cmdshell", "");
            str = str.Replace("net localgroup administrators", "");
            str = str.Replace("select", "");
            str = str.Replace("insert", "");
            str = str.Replace("delete from", "");
            str = str.Replace("drop table", "");
            str = str.Replace("update", "");
            str = str.Replace("truncate", "");
            str = str.Replace("from", "");
            return str;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string ToDBC(string input)
        {
            char[] chArray = input.ToCharArray();
            for (int i = 0; i < chArray.Length; i++)
            {
                if (chArray[i] == '　')
                {
                    chArray[i] = ' ';
                }
                else if ((chArray[i] > 0xff00) && (chArray[i] < 0xff5f))
                {
                    chArray[i] = (char) (chArray[i] - 0xfee0);
                }
            }
            return new string(chArray);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string ToSBC(string input)
        {
            char[] chArray = input.ToCharArray();
            for (int i = 0; i < chArray.Length; i++)
            {
                if (chArray[i] == ' ')
                {
                    chArray[i] = '　';
                }
                else if (chArray[i] < '\x007f')
                {
                    chArray[i] = (char) (chArray[i] + 0xfee0);
                }
            }
            return new string(chArray);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="exclueStr"></param>
        /// <returns></returns>
        public static string TSEncode(string input, string exclueStr)
        {
            string str = input;
            if (exclueStr.IndexOf("',") < 0)
            {
                str = str.Replace("'", ToSBC("'"));
            }
            if (exclueStr.IndexOf("%,") < 0)
            {
                str = str.Replace("%", ToSBC("%"));
            }
            if (exclueStr.IndexOf(" ,") < 0)
            {
                str = str.Replace(" ", ToSBC(" "));
            }
            if (exclueStr.IndexOf("-,") < 0)
            {
                str = str.Replace("-", ToSBC("-"));
            }
            if (exclueStr.IndexOf("+,") < 0)
            {
                str = str.Replace("+", ToSBC("+"));
            }
            if (exclueStr.IndexOf("&,") < 0)
            {
                str = str.Replace("&", ToSBC("&"));
            }
            if (exclueStr.IndexOf("$,") < 0)
            {
                str = str.Replace("$", ToSBC("$"));
            }
            if (exclueStr.IndexOf(".,") < 0)
            {
                str = str.Replace(".", ToSBC("."));
            }
            if (exclueStr.IndexOf("=,") < 0)
            {
                str = str.Replace("=", ToSBC("="));
            }
            if (exclueStr.IndexOf("(,") < 0)
            {
                str = str.Replace("(", ToSBC("("));
            }
            if (exclueStr.IndexOf("),") < 0)
            {
                str = str.Replace(")", ToSBC(")"));
            }
            if (exclueStr.IndexOf("<,") < 0)
            {
                str = str.Replace("<", ToSBC("<"));
            }
            if (exclueStr.IndexOf(">,") < 0)
            {
                str = str.Replace(">", ToSBC(">"));
            }
            if (exclueStr.IndexOf("@,") < 0)
            {
                str = str.Replace("@", ToSBC("@"));
            }
            if (exclueStr.IndexOf("#,") < 0)
            {
                str = str.Replace("#", ToSBC("#"));
            }
            if (exclueStr.IndexOf(",,") < 0)
            {
                str = str.Replace(",", ToSBC(","));
            }
            if (exclueStr.IndexOf(":,") < 0)
            {
                str = str.Replace(":", ToSBC(":"));
            }
            if (exclueStr.IndexOf(";,") < 0)
            {
                str = str.Replace(";", ToSBC(";"));
            }
            if (exclueStr.IndexOf("!,") < 0)
            {
                str = str.Replace("!", ToSBC("!"));
            }
            if (exclueStr.IndexOf("|,") < 0)
            {
                str = str.Replace("|", ToSBC("|"));
            }
            return str;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        public static void ValidID(string ID)
        {
            if ((ID.Trim() == "") || !IsNumStr(ID))
            {
                throw new Exception("参数传递错误!");
            }
        }
    }
}


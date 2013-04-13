using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace OZGNet.WebForm.ThirdPart.DsJian
{
    /// <summary>
    /// Html相关的通用操作
    /// </summary>
    public class HtmlDeal
    {
        /// <summary>
        /// 
        /// </summary>
        public static Regex OutWebURL = new Regex("://[\\s\\S]*?/", RegexOptions.IgnoreCase);

        /// <summary>
        /// 移除不安全代码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveUnsafeCode(string str)
        {
            StringBuilder builder = new StringBuilder(str);
            Regex regex = new Regex(@"<script[\s\S]*?>[\s\S]*?</script>", RegexOptions.IgnoreCase);
            foreach (Match match in regex.Matches(builder.ToString()))
            {
                builder.Replace(match.Value, "");
            }
            regex = new Regex(@"<script[\s\S]*?/>", RegexOptions.IgnoreCase);
            foreach (Match match2 in regex.Matches(builder.ToString()))
            {
                builder.Replace(match2.Value, "");
            }
            regex = new Regex(@"<iframe[\s\S]*?/>", RegexOptions.IgnoreCase);
            foreach (Match match3 in regex.Matches(builder.ToString()))
            {
                builder.Replace(match3.Value, "");
            }
            regex = new Regex(@"<iframe[\s\S]*?>[\s\S]*?</iframe>", RegexOptions.IgnoreCase);
            foreach (Match match4 in regex.Matches(builder.ToString()))
            {
                builder.Replace(match4.Value, "");
            }
            return builder.ToString();
        }

        /// <summary>
        /// 移除HTML代码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveHtmlCode(string str)
        {
            StringBuilder builder = new StringBuilder(str);
            Regex regex = new Regex(@"<[\s\S]*?>", RegexOptions.IgnoreCase);
            foreach (Match match in regex.Matches(builder.ToString()))
            {
                builder.Replace(match.Value, "");
            }
            builder.Replace("&nbsp;", " ");
            return builder.ToString().Trim();
        }

        /// <summary>
        /// 移除HTML代码的某些标签
        /// </summary>
        /// <param name="str"></param>
        /// <param name="labels"></param>
        /// <returns></returns>
        public static string RemoveHtmlCode(string str, params String[] labels)
        {
            StringBuilder builder = new StringBuilder(str);

            foreach (String tempstr in labels)
            {
                Regex regex = new Regex(@"<\/{0,1}" + tempstr + @"[\s\S]*?>", RegexOptions.IgnoreCase);
                foreach (Match match in regex.Matches(builder.ToString()))
                {
                    builder.Replace(match.Value, "");
                }
            }

            builder.Replace("&nbsp;", " ");
            return builder.ToString().Trim();
        }

        /// <summary>
        /// 移除所有标签的某些属性
        /// </summary>
        /// <param name="str"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static string RemoveHtmlAttributeCode(string str, params String[] attributes)
        {
            StringBuilder builder = new StringBuilder(str);

            Regex regexHtmlLabel = new Regex(@"<[\s\S]*?>", RegexOptions.IgnoreCase);

            foreach (Match match in regexHtmlLabel.Matches(builder.ToString()))
            {   //原始标签赋值给os临时变量
                String tempLable = match.Value;

                //最外层需要替换掉的标签字符串
                String replaceLabel = tempLable;
                foreach (string attribute in attributes)
                {

                    Regex regexAttribute = new Regex(attribute + @"=\S*?( |>)", RegexOptions.IgnoreCase);


                    //找到标签中对应的属性
                    foreach (Match matchTemp in regexAttribute.Matches(replaceLabel))
                    {

                        //找到替换后的临时变量
                        tempLable = tempLable.Replace(matchTemp.Value.TrimEnd('>'), "");

                        //最外层替换
                        builder.Replace(replaceLabel, tempLable);

                        //替换掉的标签字符串修改
                        replaceLabel = tempLable;
                    }

                }

            }

            return builder.ToString().Trim();

        }

        /// <summary>
        /// 移除某个标签的某些属性
        /// </summary>
        /// <param name="str"></param>
        /// <param name="label"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static string RemoveHtmlAttributeCode(string str, String label, params String[] attributes)
        {
            StringBuilder builder = new StringBuilder(str);

            Regex regexHtmlLabel = new Regex(@"<\/{0,1}" + label + @"[\s\S]*?>", RegexOptions.IgnoreCase);

            foreach (Match match in regexHtmlLabel.Matches(builder.ToString()))
            {   //原始标签赋值给os临时变量
                String tempLable = match.Value;

                //最外层需要替换掉的标签字符串
                String replaceLabel = tempLable;
                foreach (string attribute in attributes)
                {

                    Regex regexAttribute = new Regex(attribute + @"=\S*?( |>)", RegexOptions.IgnoreCase);


                    //找到标签中对应的属性
                    foreach (Match matchTemp in regexAttribute.Matches(replaceLabel))
                    {

                        //找到替换后的临时变量
                        tempLable = tempLable.Replace(matchTemp.Value.TrimEnd('>'), "");

                        //最外层替换
                        builder.Replace(replaceLabel, tempLable);

                        //替换掉的标签字符串修改
                        replaceLabel = tempLable;
                    }

                }

            }

            return builder.ToString().Trim();

        }

        /// <summary>
        /// 移除某个标签的某个属性
        /// </summary>
        /// <param name="str"></param>
        /// <param name="label"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static string RemoveHtmlAttributeCode(string str, String label, String attribute)
        {
            StringBuilder builder = new StringBuilder(str);

            Regex regex = new Regex(@"<\/{0,1}" + label + @"[\s\S]*?>", RegexOptions.IgnoreCase);

            Regex regexAttribute = new Regex(attribute + @"=\S*?( |>)", RegexOptions.IgnoreCase);

            //找到匹配到标签
            foreach (Match match in regex.Matches(builder.ToString()))
            {
                //原始标签赋值给os临时变量
                String tempLable = match.Value;

                //最外层需要替换掉的标签字符串
                String replaceLabel = tempLable;

                //找到标签中对应的属性
                foreach (Match matchTemp in regexAttribute.Matches(replaceLabel))
                {

                    //找到替换后的临时变量
                    tempLable = tempLable.Replace(matchTemp.Value.TrimEnd('>'), "");

                    //最外层替换
                    builder.Replace(replaceLabel, tempLable);

                    //替换掉的标签字符串修改
                    replaceLabel = tempLable;
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// 移除除了自己的域名的其他URL
        /// </summary>
        /// <param name="str">Html字符串</param>
        /// <param name="myDomain">自己的域名</param>
        /// <returns>结果字符串</returns>
        public static string RemoveUrlLink(String str, String myDomain)
        {
            StringBuilder builder = new StringBuilder(str);

            //找到所有的href连接
            Regex regex = new Regex(@" href=[\s\S]*?( |>)", RegexOptions.IgnoreCase);

            foreach (Match match in regex.Matches(builder.ToString()))
            {
                //如果连接中存在域名则替换
                if (match.Value.ToLower().IndexOf(myDomain.ToLower()) == -1)
                {
                    String replaceStr = match.Value.TrimEnd('>');
                    builder.Replace(replaceStr, " ");
                }
            }

            return builder.ToString().Trim();
        }

        /// <summary>
        /// 判断是否存在别人的连接
        /// </summary>
        /// <param name="str"></param>
        /// <param name="myDomain"></param>
        /// <returns></returns>
        public static Boolean HasUrlLink(String str, String myDomain)
        {

            //找到所有的href连接
            Regex regex = new Regex(@" href=[\s\S]*?( |>)", RegexOptions.IgnoreCase);

            foreach (Match match in regex.Matches(str))
            {
                //如果连接中存在域名则返回true
                if (match.Value.ToLower().IndexOf(myDomain.ToLower()) == -1)
                {
                    return true;
                }

            }

            return false;
        }

        /// <summary>
        /// 判断是否连接
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean HasUrlLink(String str)
        {

            //找到所有的href连接
            Regex regex = new Regex(@" href=[\s\S]*?( |>)", RegexOptions.IgnoreCase);

            foreach (Match match in regex.Matches(str))
            {
                if (match.Success)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 处理外网连接替换
        /// </summary>
        /// <param name="str"></param>
        /// <param name="myDomain"></param>
        public static String DealOutSiteURL(String str, String myDomain)
        {
            StringBuilder sb = new StringBuilder(str);

            foreach (Match match in OutWebURL.Matches(str))
            {
                if (match.Value.ToLower().IndexOf(myDomain) == -1)
                {
                    sb.Replace(match.Value, "");
                }
            }

            return sb.ToString();
        }
    }
}

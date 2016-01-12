using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ConvertDateToChinese
{
    /// <summary>
    /// 数字日期转中文日期
    /// </summary>
    public class DateToChinese
    {
        private static DateToChinese instance = null;
        public static DateToChinese Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DateToChinese();

                }
                return instance;
            }

        }

        public char[] chinese;
        private DateToChinese()
        {
            chinese = new char[] { '0', '一', '二', '三', '四', '五', '六', '七', '八', '九', '十' };
        }


        public string ConvertMethod(DateTime dt)
        {
            return ConvertMethod(dt.ToString("yyyy-MM-dd"));
        }

        //Method for Convert to Chinese
        public string ConvertMethod(string date)
        {
            // Define stringBuilder variable  for write out the chinese of date
            StringBuilder strb = new StringBuilder();

            //define regex regularExpressins variable
            Regex regex = new Regex(@"(d{2}|d{4})(-|/)(d{1}|d{2})(-|/)(d{1}|d{2})");
            if (!regex.IsMatch(date) == true)
            {
                string[] str = null;
                if (date.Contains("-"))
                {
                    str = date.Split('-');
                }
                else
                {
                    if (date.Contains("/"))
                    {
                        str = date.Split('/');

                    }
                }
                //Convert year as follow
                for (int i = 0; i < str[0].Length; i++)
                {
                    strb = strb.Append(chinese[int.Parse(str[0][i].ToString())]);
                }
                strb.Append("年");

                //Convert month as follow
                // if (chinese[int.Parse(str[1][0].ToString())] != '0')
                // {
                //     strb.Append(chinese[int.Parse(str[1][0].ToString())]);
                // }
                // for (int i = 1; i < str[1].Length; i++)
                // {
                //     strb = strb.Append(chinese[int.Parse(str[1][i].ToString())]);
                // }
                // strb.Append("月");

                //        //Convert day as follow
                // if (chinese[int.Parse(str[2][0].ToString())] !='0')
                // {
                //     strb.Append(chinese[int.Parse(str[2][0].ToString())]);
                // }
                // for (int i = 1; i < str[2].Length; i++)
                // {
                //     strb = strb.Append(chinese[int.Parse(str[2][i].ToString())]);
                // }
                // strb.Append("日");
                #region  //convert month
                int monthod = int.Parse(str[1]);
                int MN1 = monthod / 10;
                int MN2 = monthod % 10;


                if (MN1 > 1)
                {
                    strb.Append(chinese[MN1]);
                }
                if (MN1 > 0)
                {
                    strb.Append(chinese[10]);
                }
                if (MN2 != 0)
                {
                    strb.Append(chinese[MN2]);
                    strb.Append("月");
                }
                #endregion
                #region //convert day
                int day = int.Parse(str[2]);
                int day1 = day / 10;
                int day2 = day % 10;

                if (day1 > 1)
                {
                    strb.Append(chinese[day1]);


                }
                if (day1 > 0)
                {
                    strb.Append(chinese[10]);
                }
                if (day2 > 0)
                {
                    strb.Append(chinese[day2]);
                    strb.Append("日");
                }

                #endregion


            }

            else
            {
                throw new ArgumentException();

            }
            return strb.ToString();
        }

    }
}

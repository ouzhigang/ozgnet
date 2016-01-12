using System;
using System.Collections.Generic;
using System.Text;

namespace OZGNet
{
    /// <summary>
    /// 数字转中文大写
    /// </summary>
    public class Cash
    {
        private static string MoneyNum = "零一二三四五六七八九";
        private static string MoneyUnit = "十百千万十百千亿";
        private static string CashUnit = "元角分整";
        private static string[] BeforeReplace = new string[] {  MoneyNum[0].ToString() + MoneyNum[0].ToString(),
                                                                MoneyUnit[7].ToString() + MoneyNum[0].ToString() + MoneyUnit[3].ToString() ,
                                                                MoneyNum[0].ToString() + MoneyUnit[3].ToString(),
                                                                MoneyNum[0].ToString() + MoneyUnit[7].ToString(),
                                                               MoneyUnit[7].ToString() + MoneyUnit[3].ToString(),
                                                                MoneyNum[0].ToString() + MoneyNum[0].ToString(),
                                                                MoneyNum[0].ToString() + CashUnit[1].ToString() + MoneyNum[0].ToString() + CashUnit[2].ToString(),
                                                                MoneyNum[0].ToString() + CashUnit[2].ToString()
                                                             };
        private static string[] AfterReplace = new string[] {  MoneyNum[0].ToString(),
                                                                MoneyUnit[7].ToString() + MoneyNum[0].ToString(),
                                                                MoneyUnit[3].ToString() + MoneyNum[0].ToString(),
                                                                MoneyUnit[7].ToString() + MoneyNum[0].ToString(),
                                                                MoneyUnit[7].ToString() + MoneyNum[0].ToString(),
                                                                MoneyNum[0].ToString(),
                                                                String.Empty,
                                                                String.Empty
                                                             };

        /**/
        /**/
        /**/
        /// <summary>
        /// 字符串替换方法，另外一种是以 4 个数字为基础的递归方法
        /// Create By HJ 2007-10-21
        /// </summary>
        /// <param name="moneyNum">输入数字，如888.88M</param>
        /// <returns></returns>
        public static string GetCash(decimal moneyNum)
        {
            string intNum, point;
            /**/
            /**/
            /**/
            ///取整数部分
            intNum = Convert.ToString(Math.Floor(moneyNum));
            /**/
            /**/
            /**/
            ///取小数部分
            point = Convert.ToString(Math.Floor(moneyNum * 100));
            point = point.Substring(point.Length - 2);

            /**/
            /**/
            /**/
            ///计算整数部分
            for (int i = 0; i < MoneyNum.Length; i++)
            {
                intNum = intNum.Replace(i.ToString(), MoneyNum[i].ToString());
            }
            int intNumLength = intNum.Length;
            for (int i = intNumLength - 1; i > 0; i--)
            {   /**//**//**////根据位数把单位加上
                ///如果是零则不加单位，但是 万  和  亿 需要加上
                if (intNum[i - 1] == MoneyNum[0] && (intNumLength - i + 7) % 8 != 3 && (intNumLength - i + 7) % 8 != 7) continue;
                intNum = intNum.Insert(i, MoneyUnit[(intNumLength - i + 7) % 8].ToString());
            }

            /**/
            /**/
            /**/
            ///加上 角 和 分 的单位
            intNum += CashUnit[0].ToString() + MoneyNum.Substring(Convert.ToInt16(point[0].ToString()), 1) + CashUnit[1].ToString() + MoneyNum.Substring(Convert.ToInt16(point[1].ToString()), 1) + CashUnit[2].ToString();

            /**/
            /**/
            /**/
            ///替换  零零 -> 零  亿零万  ->  亿零，零万  ->  万零，零亿 -> 亿零，亿万 -> 亿零，零角零分 - > ""，零分 - > ""，零零 -> 零，再调用一次，确保 亿零万 替换后的情况
            for (int i = 0; i < BeforeReplace.Length; i++)
            {
                while (intNum.IndexOf(BeforeReplace[i]) > -1)
                {
                    intNum = intNum.Replace(BeforeReplace[i], AfterReplace[i]);
                }
            }

            /**/
            /**/
            /**/
            ///最后的 零 去掉
            if (intNum.EndsWith(MoneyNum[0].ToString())) intNum = intNum.Substring(0, intNum.Length - 1);

            return intNum + CashUnit[3].ToString();
        }

    }
}
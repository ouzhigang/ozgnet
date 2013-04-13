using System;
using System.Data;
using System.Web;
using System.Text;

namespace OZGNet.WebForm.ThirdPart.Jumbot
{
    /// <summary>
    /// DataTable转成Json数据
    /// </summary>
    public static class JsonHelp
    {
        /// <summary>
        /// 将dt转化成Json数据 格式如 table[{id:1,title:'体育'},id:2,title:'娱乐'}]
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DtToSON(DataTable dt)
        {
            return DtToSON(dt, 0, "recordcount", "table");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fromCount"></param>
        /// <returns></returns>
        public static string DtToSON(DataTable dt, int fromCount)
        {
            return DtToSON(dt, fromCount, "recordcount", "table");
        }
        /// <summary>
        /// 将dt转化成Json数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fromCount"></param>
        /// <param name="totalCountStr"></param>
        /// <param name="tbname"></param>
        /// <returns></returns>
        public static string DtToSON(DataTable dt, int fromCount, string totalCountStr, string tbname)
        {
            return DtToSON(dt, fromCount, "recordcount", "table", true);
        }
        /// <summary>
        /// 将dt转化成Json数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fromCount"></param>
        /// <param name="totalCountStr"></param>
        /// <param name="tbname"></param>
        /// <param name="formatData"></param>
        /// <returns></returns>
        public static string DtToSON(DataTable dt, int fromCount, string totalCountStr, string tbname, bool formatData)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append(totalCountStr + ":" + dt.Rows.Count + "," + tbname + ": [");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i > 0)
                    jsonBuilder.Append(",");
                jsonBuilder.Append("{");
                jsonBuilder.Append("no:" + (fromCount + i + 1) + ",");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (j > 0)
                        jsonBuilder.Append(",");

                    jsonBuilder.Append(dt.Columns[j].ColumnName.ToLower() + ": '" + dt.Rows[i][j].ToString().Replace("\t", " ").Replace("\r", "").Replace("\n", "<br/>").Replace("\'", "\\\'") + "'");
                }
                jsonBuilder.Append("}");
            }
            jsonBuilder.Append("]");
            return jsonBuilder.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OZGNet.WebForm.ThirdPart.DotBBS
{
    /// <summary>
    /// DotBBS类型转换
    /// </summary>
    public class DotConvert
    {
        /// <summary>
        /// DataTable转IList
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="dt">DataTable实例</param>
        /// <returns></returns>
        public static IList<T> DataTableToList<T>(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }
            IList<T> list = new List<T>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                T local = (T) Activator.CreateInstance(typeof(T));
                foreach (PropertyInfo info in local.GetType().GetProperties())
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (info.Name.Equals(dt.Columns[j].ColumnName))
                        {
                            if (dt.Rows[i][j] != DBNull.Value)
                            {
                                if (info.PropertyType.ToString() == "System.Int32")
                                {
                                    info.SetValue(local, int.Parse(dt.Rows[i][j].ToString()), null);
                                }
                                if (info.PropertyType.ToString() == "System.DateTime")
                                {
                                    info.SetValue(local, Convert.ToDateTime(dt.Rows[i][j].ToString()), null);
                                }
                                if (info.PropertyType.ToString() == "System.String")
                                {
                                    info.SetValue(local, dt.Rows[i][j].ToString(), null);
                                }
                                if (info.PropertyType.ToString() == "System.Boolean")
                                {
                                    info.SetValue(local, Convert.ToBoolean(dt.Rows[i][j].ToString()), null);
                                }
                            }
                            else
                            {
                                info.SetValue(local, null, null);
                            }
                            break;
                        }
                    }
                }
                list.Add(local);
            }
            return list;
        }
    }
}


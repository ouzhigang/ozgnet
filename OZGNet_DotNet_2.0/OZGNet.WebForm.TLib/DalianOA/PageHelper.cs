using System;
using System.Text;

namespace OZGNet.WebForm.ThirdPart.DalianOA
{   
    public class PageHelper
    {
        protected string GetPagerSQL(string condition, int pageSize, int pageIndex, string fldSort, string tblName, bool sort)
        {
            string str = sort ? " DESC" : " ASC";
            if (pageIndex == 1)
            {
                return ("select top " + pageSize.ToString() + " * from " + tblName.ToString() + (string.IsNullOrEmpty(condition) ? string.Empty : (" where " + condition)) + " order by " + fldSort.ToString() + str);
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("select top {0} * from {1} ", pageSize, tblName);
            builder.AppendFormat(" where {1} not in (select top {0} {1} from {2} ", pageSize * (pageIndex - 1), fldSort.Substring(fldSort.LastIndexOf(',') + 1, (fldSort.Length - fldSort.LastIndexOf(',')) - 1), tblName);
            if (!string.IsNullOrEmpty(condition))
            {
                builder.AppendFormat(" where {0} order by {1}{2}) and {0}", condition, fldSort, str);
            }
            else
            {
                builder.AppendFormat(" order by {0}{1}) ", fldSort, str);
            }
            builder.AppendFormat(" order by {0}{1}", fldSort, str);
            return builder.ToString();
        }
    }
}


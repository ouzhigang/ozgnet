using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OZGNet.WebForm.Mvc.HtmlHelperExpansion.Pager
{
    public class PageList<T> : List<T>
    {
        /// <summary>   
        /// 当前分页   
        /// </summary>   
        public int PageIndex { get; private set; }
        /// <summary>   
        /// 显示数据条目   
        /// </summary>   
        public int PageSize { get; private set; }
        /// <summary>   
        /// 总页数   
        /// </summary>   
        public int TotalCount { get; private set; }

        public PageList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            PageIndex = pageIndex;

            PageSize = pageSize;

            TotalCount = totalCount;

            AddRange(source);
        }
    }
}

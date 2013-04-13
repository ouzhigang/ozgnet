using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace OZGNet.WebForm.Mvc.HtmlHelperExpansion.Pager
{


    /*

    CSS

    .pager   
    {   
        font-size: 12px;   
        margin: 8px 0;   
        padding: 3px 0 3px;   
        text-align: left;   
    }   
  
    .pager .current   
    {   
        background-color: #06c;   
        border: 1px solid #009;   
        color: #fff;   
        font-weight: bold;   
        margin-top: 4px;   
        padding: 3px 5px;   
        text-align: center;   
    }   
  
    .pager a   
    {   
        margin: 4px 3px;   
        border: 1px solid #9AAFE5;   
        padding: 3px 5px;   
        text-align: center;   
        text-decoration: none;   
        color: #2E6AB1;   
    }   
  
    .pager .pagerInput   
    {   
        padding: 3px 0 0 0;   
        border: 1px solid #9AAFE5;   
        text-align: center;   
        text-decoration: none;   
        height: 16px;   
        width: 30px;   
    }   
    .pager .pagerButton   
    {   
        border: 1px solid #9AAFE5;   
        cursor: pointer;   
    }  

    实例1：
    <div class="pager">  
        <%= Html.Pager(int.Parse(ViewData["pageIndex"].ToString()), int.Parse(ViewData["pageSize"].ToString()), int.Parse(ViewData["total"].ToString())) %>  
    </div>     


      
     
      
      
      
      
      
      
      
      
    
    实例2：    
        /// <summary>   
        /// 获取角色列表   
        /// </summary>   
        /// <returns></returns>   
        private List<RBAC_Role> RoleList(int? page)   
        {   
            if (!page.HasValue)   
            {   
                page = 1;   
            }   
  
            int total = 0;   
            int pageSize = 2;   
  
            //获得角色数据列表   
            List<RBAC_Role> roleItems = Bll.GetPageData(page.Value,pageSize,ref total);   
  
            return roleItems.ToPageList(page.Value, pageSize, total);   
        }  
      
      
    <%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PageList<Entity.RBAC_Role>>" %>
    
    <div class="pager">  
        <%= Html.Pager(Model.PageIndex,Model.PageSize,Model.TotalCount) %>  
    </div> 
    */


    public static class PagerHelper
    {
        private const int defaultDisplayCount = 11;

        #region HtmlHelper Extensions

        /// <summary>   
        /// Pager Helper Extensions   
        /// </summary>   
        /// <param name="htmlHelper"></param>   
        /// <param name="currentPage">当前页码</param>   
        /// <param name="pageSize">页面显示的数据条目</param>   
        /// <param name="totalCount">总记录数</param>   
        /// <param name="toDisplayCount">Helper要显示的页数</param>   
        /// <returns></returns>   
        public static string Pager(this HtmlHelper htmlHelper, int currentPage, int pageSize, int totalCount, int toDisplayCount)
        {
            RenderPager pager = new RenderPager(htmlHelper.ViewContext, currentPage, pageSize, totalCount, toDisplayCount);

            return pager.RenderHtml();
        }

        public static string Pager(this HtmlHelper htmlHelper, int currentPage, int pageSize, int totalCount, int toDisplayCount, string separated)
        {
            RenderPager pager = new RenderPager(htmlHelper.ViewContext, currentPage, pageSize, totalCount, toDisplayCount, separated);

            return pager.RenderHtml();
        }

        /// <summary>   
        /// Pager Helper Extensions   
        /// </summary>   
        /// <param name="htmlHelper"></param>   
        /// <param name="currentPage">当前页码</param>   
        /// <param name="pageSize">页面显示的数据条目</param>   
        /// <param name="totalCount">总记录数</param>   
        /// <returns></returns>   
        public static string Pager(this HtmlHelper htmlHelper, int currentPage, int pageSize, int totalCount)
        {
            RenderPager pager = new RenderPager(htmlHelper.ViewContext, currentPage, pageSize, totalCount, defaultDisplayCount);

            return pager.RenderHtml();
        }

        public static string Pager(this HtmlHelper htmlHelper, int currentPage, int pageSize, int totalCount, string separated)
        {
            RenderPager pager = new RenderPager(htmlHelper.ViewContext, currentPage, pageSize, totalCount, defaultDisplayCount, separated);

            return pager.RenderHtml();
        }

        /// <summary>   
        /// IEnumerable extensions   
        /// </summary>   
        public static PageList<T> ToPageList<T>(this IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            return new PageList<T>(source, pageIndex, pageSize, totalCount);
        }

        #endregion
    }
}

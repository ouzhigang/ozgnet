using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;

namespace OZGNet.WebForm
{
    /// <summary>
    /// Asp.Net出现异常时显示的页面处理
    /// </summary>
    public class ErrorPage
    {
        /*
            实例:
            放在Global.asax里面
            void Application_Error(object sender, EventArgs e) 
            { 
                //在出现未处理的错误时运行的代码

                OZGNet.AspNetErrorPage aa = new OZGNet.AspNetErrorPage();
                aa.ReadWebConfig();
                aa.RunErrorPage();
            }
            
            Web.Config
            <appSettings>
                <add key="ErrorPageVisible" value="True"/>
                <add key="ErrorTemplate" value="/ErrorTemplate.htm"/>
                <add key="ErrorMessage" value="$Message$"/>
	        </appSettings>
         * 
         * 
         * 
         * 
         * 
         * 
         */

        private bool _ErrorPageVisible;
        private string _ErrorTemplate;
        private string _ErrorMessage;

        #region 使用默认设置
        /// <summary>
        /// 使用默认设置
        /// </summary>
        public ErrorPage()
        {
            this._ErrorPageVisible = true;
            this._ErrorTemplate = UtilityIO.FileRead("~/ErrorTemplate.htm");
            this._ErrorMessage = "$Message$";
        } 
        #endregion
        #region 构造函数2
        /// <summary>
        /// 构造函数2
        /// </summary>
        /// <param name="ErrorPageVisible">是否显示异常处理页</param>
        /// <param name="ErrorTemplate">异常处理页的路径</param>
        /// <param name="ErrorMessage">错误信息的占位符</param>
        public ErrorPage(bool ErrorPageVisible, string ErrorTemplate, string ErrorMessage)
        {
            this._ErrorPageVisible = ErrorPageVisible;
            this._ErrorTemplate = UtilityIO.FileRead(ErrorTemplate);
            this._ErrorMessage = ErrorMessage;
        } 
        #endregion

        #region 读取Web.Config里面的设置
        /// <summary>
        /// 读取Web.Config里面的设置
        /// </summary>
        public void ReadWebConfig()
        {            
            if (ConfigurationManager.AppSettings["ErrorPageVisible"].ToLower() == "true")
            {
                this._ErrorPageVisible = true;
            }
            else if (ConfigurationManager.AppSettings["ErrorPageVisible"].ToLower() == "false")
            {
                this._ErrorPageVisible = false;
            }

            this._ErrorTemplate = UtilityIO.FileRead(ConfigurationManager.AppSettings["ErrorTemplate"]);
            this._ErrorMessage = ConfigurationManager.AppSettings["ErrorMessage"];
        } 
        #endregion

        #region 执行!
        /// <summary>
        /// 执行!
        /// </summary>
        public void RunErrorPage()
        {
            if (this._ErrorPageVisible)
            {
                Exception ex = HttpContext.Current.Server.GetLastError().GetBaseException();
                HttpContext.Current.Response.Write(this._ErrorTemplate.Replace(this._ErrorMessage, ex.Message));
                HttpContext.Current.Response.End();
                HttpContext.Current.Server.ClearError();
            }
        } 
        #endregion

        #region 设置是否显示异常处理页
        /// <summary>
        /// 设置是否显示异常处理页
        /// </summary>
        public bool ErrorPageVisible
        {
            set { this._ErrorPageVisible = value; }
        } 
        #endregion

        #region 设置异常处理页的路径
        /// <summary>
        /// 设置异常处理页的路径
        /// </summary>
        public string ErrorTemplate
        {
            set { this._ErrorTemplate = UtilityIO.FileRead(value); }
        } 
        #endregion

        #region 设置错误信息的占位符
        /// <summary>
        /// 设置错误信息的占位符
        /// </summary>
        public string ErrorMessage
        {
            set { this._ErrorMessage = value; }
        } 
        #endregion

    }
}

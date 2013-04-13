// ================================================
// SmptMail.cs, Smtp 邮件发送
// Copyright (C) 2010 Benking
// mail:ben.wangzj@gmail.com
// website:http://www.dsjian.com/
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// ================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace OZGNet.WebForm.ThirdPart.DsJian.Mail
{
    /// <summary>
    /// Smtp 邮件发送
    /// </summary>
    public class SmtpMail
    {
        /// <summary>
        /// Smtp服务器地址
        /// </summary>
        public string SmtpServer { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Smtp 端口
        /// </summary>
        public int SmtpPort { get; set; }

        /// <summary>
        /// 是否允许SSL
        /// </summary>
        public bool SmtpSSL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SmtpMail() 
        {
            this.SmtpServer = "mail.dsjian.com";
            this.UserName = "no-replyme@dsjian.com";
            this.Password = "DXNDNLL22B";
            this.DisplayName = "Xpress";
            this.SmtpPort = 25;
            this.SmtpSSL = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="smtpServer"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="displayName"></param>
        /// <param name="smtpPort"></param>
        /// <param name="smtpSSL"></param>
        public SmtpMail(string smtpServer, string userName, string password, string displayName, int smtpPort, bool smtpSSL)
        {
            this.SmtpServer = smtpServer;
            this.UserName = userName;
            this.Password = password;
            this.DisplayName = displayName;
            this.SmtpPort = smtpPort;
            this.SmtpSSL = smtpSSL;
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="smtpServer">邮件服务器地址</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="smtpPort">端口</param>
        /// <param name="smtpSSL">是否允许SSL</param>
        /// <returns></returns>
        public static SmtpMail CreateInstance(string smtpServer, string userName, string password, string displayName, int smtpPort, bool smtpSSL)
        {
            return new SmtpMail(smtpServer, userName, password, displayName, smtpPort, smtpSSL);
        }

        /// <summary>
        /// 发送系统邮件
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="body">正文</param>
        /// <param name="mailTo">收件人数组</param>
        /// <returns></returns>
        public bool Send(string title, string body, params string[] mailTo)
        {
            try
            {
                body += "<br><br>系统邮件，请勿直接回复！";
                SMTP smtp = new SMTP(this.UserName, mailTo, title, body, this.SmtpServer, UserName, Password);
                smtp.SMTPPort = this.SmtpPort;
                smtp.MailDisplyName = this.DisplayName;
                smtp.IsBodyHtml = true;

                return smtp.Send();
            }
            catch
            {
                //Logger.LogDeal log = new DsJian.Logger.LogDeal("mail_send_log");
                //log.WriteException("发送邮件出错", ex);
                return false;
            }
        }
    }
}

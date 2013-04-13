using System;

namespace OZGNet.Net.EmailData
{
    /// <summary>
    /// 获取188邮箱的POP3和SMTP信息
    /// </summary>
    public class _188Email : IEmail
    {
        /// <summary>
        /// 获取SMTP服务器地址
        /// </summary>
        public string SmtpServer
        {
            get { return "smtp.188.com"; }
        }

        /// <summary>
        /// 获取SMTP服务器端口
        /// </summary>
        public int SmtpPort
        {
            get { return 25; }
        }

        /// <summary>
        /// 获取Pop3服务器地址
        /// </summary>
        public string Pop3Server
        {
            get { return "pop3.188.com"; }
        }

        /// <summary>
        /// 获取POP3服务器端口
        /// </summary>
        public int Pop3Port
        {
            get { return 110; }
        }

        /// <summary>
        /// 获取SSL开启状态
        /// </summary>
        public bool EnabledSSL
        {
            get { return false; }
        }

    }
}

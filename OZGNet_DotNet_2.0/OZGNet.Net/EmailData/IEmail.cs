using System;

namespace OZGNet.Net.EmailData
{
    /// <summary>
    /// 获取主流邮箱的POP3和SMTP信息的接口
    /// </summary>
    public interface IEmail
    {
        /// <summary>
        /// 获取SMTP服务器地址
        /// </summary>
        string SmtpServer
        {
            get;
        }

        /// <summary>
        /// 获取SMTP服务器端口
        /// </summary>
        int SmtpPort
        {
            get;
        }

        /// <summary>
        /// 获取Pop3服务器地址
        /// </summary>
        string Pop3Server
        {
            get;
        }

        /// <summary>
        /// 获取POP3服务器端口
        /// </summary>
        int Pop3Port
        {
            get;
        }

        /// <summary>
        /// 获取SSL开启状态
        /// </summary>
        bool EnabledSSL
        {
            get;
        }

    }
}

using System;

namespace OZGNet.Net.EmailData
{
    namespace OZGNet.Net.EmailData
    {
        /// <summary>
        /// 获取Gmail邮箱的POP3和SMTP信息
        /// </summary>
        public class Gmail : IEmail
        {
            /// <summary>
            /// 获取SMTP服务器地址
            /// </summary>
            public string SmtpServer
            {
                get { return "smtp.gmail.com"; }
            }

            /// <summary>
            /// 获取SMTP服务器端口
            /// </summary>
            public int SmtpPort
            {
                get { return 465; }
            }

            /// <summary>
            /// 获取Pop3服务器地址
            /// </summary>
            public string Pop3Server
            {
                get { return "pop.gmail.com"; }
            }

            /// <summary>
            /// 获取POP3服务器端口
            /// </summary>
            public int Pop3Port
            {
                get { return 995; }
            }

            /// <summary>
            /// 获取SSL开启状态
            /// </summary>
            public bool EnabledSSL
            {
                get { return true; }
            }

        }
    }
}

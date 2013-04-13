#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Web;
using System.Web.Mail;
using System.Web.UI;
using OpenPOP.POP3;
using System.Reflection;
using System.IO;
using System.Xml;


namespace OZGNet.WebForm.ThirdPart.We7
{
	/// <summary>
	/// 邮件发送助手类
	/// </summary>
	public class MailHelper
    {
        #region 属性
        /// <summary>
        /// 实例化MailHelper
        /// </summary>
        public MailHelper() { }

        private string _adminEmail;//"master@duanke.com";
        /// <summary>
        /// 
        /// </summary>
        public string AdminEmail
        {
            get { return _adminEmail; }
            set { _adminEmail = value; }
        }

        private string _smtpServer;//"mail.duanke.com";
        /// <summary>
        /// 
        /// </summary>
        public string SmtpServer
        {
            get { return _smtpServer; }
            set { _smtpServer = value; }
        }

        private string _popServer;//"mail.duanke.com";
        /// <summary>
        /// 收邮件服务
        /// </summary>
        public string PopServer
        {
            get { return _popServer; }
            set { _popServer = value; }
        }

        private string _password;// "master@duanke.com888";
        /// <summary>
        /// 
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        private string _userName;// "master@duanke.com";
        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        #endregion

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public bool Send(string to, string from, string subject, string message, string priority)
        {
            //string SmtpServer = "mail.duanke.cn";
            //string AdminEmail = "webmaster@duanke.cn";
            //string Password = "webmaster8888";
            //string UserName = "webmaster@duanke.cn";

            ////第一类方法：MailMessage
            System.Web.Mail.MailMessage mailMessage = new System.Web.Mail.MailMessage();
            mailMessage.To = to;
            mailMessage.From = from;
            mailMessage.Subject = subject;
            mailMessage.Body = message.Replace("\r\n", "<br>");
            mailMessage.BodyFormat = MailFormat.Html;//.Text;//邮件的格式
            switch (priority)       //邮件优先级
            {
                case "High":
                    mailMessage.Priority = MailPriority.High;
                    break;
                case "Low":
                    mailMessage.Priority = MailPriority.Low;
                    break;
                case "Normal":
                    mailMessage.Priority = MailPriority.Normal;
                    break;
                default:
                    break;
            }

            //以下三句用在SMTP需要身份验证时。Framework1.1才有支持。
            mailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1"); //基本验证
            mailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", UserName); //设定用户名
            mailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", Password); //设定密码

            
            SmtpMail.SmtpServer = SmtpServer;//设定SMTP服务器
            try
            {
                SmtpMail.Send(mailMessage);
            }
            catch (Exception)
            {
                SendMailMessageToXml(mailMessage);
                string errorRoot = "<a href=\"/admin/Advice/AdviceProcessManage.aspx?tab=2\" >反馈监控管理</a>";
                throw new Exception("邮件服务器填写错误。请先确定服务器地址以及发件箱信息的正确。<br/>此次邮件发送失败，发送失败邮件请到" + errorRoot + " <邮件发送>下进行处理。");
            }

            //第二类方法：OpenSMTP
            //string smtpHost = SmtpServer; //"smtp.163.com"; 
            //int smtpPort = 25;
            //string senderEmail = AdminEmail; //"thehim@163.com";
            //Smtp smtp = new Smtp(smtpHost, smtpPort);
            //smtp.Password = Password;//"mypass";//用户密码 
            //smtp.Username = UserName; //"thehim"; //用户名称

            ////定义邮件信息==========================================================
            //OpenSmtp.Mail.MailMessage msg = new OpenSmtp.Mail.MailMessage();//(senderEmail, recipientEmail);
            //OpenSmtp.Mail.EmailAddress addfrom = new EmailAddress(senderEmail); //发件人
            //addfrom.Name = "短客网";
            //msg.From = addfrom;

            //OpenSmtp.Mail.EmailAddress addbcc = new EmailAddress(to);
            //msg.AddRecipient(addbcc, AddressType.To);

            //msg.Subject = subject;
            //msg.Charset = "gb2312";
            //msg.Body = message;

            //smtp.SendMail(msg);

            ////第三类方法：Mailserder Using
            //MailSender ms = new MailSender();
            //ms.From = AdminEmail;
            //ms.To = to;
            //ms.Subject = subject;
            //ms.Body = message;
            //ms.UserName = UserName;  
            //ms.Password = Password; 
            //ms.Server = SmtpServer;

            ////ms.Attachments.Add(new MailSender.AttachmentInfo(@"D:\\test.txt"));
            //ms.SendMail();
            return true;
        }

        /// <summary>
        /// 接受邮件，处理所有正确存在邮件
        /// </summary>
        public MailResult ReceiveMail(string asmName, string typeName, string methodName ,bool delete)
        {
            MailResult result = new MailResult();
            string strPort = "";
            if (strPort == "" || strPort == string.Empty) strPort = "110";
            POPClient popClient = new POPClient();
            try
            {
                popClient.Connect(PopServer, Convert.ToInt32(strPort));
                popClient.Authenticate(UserName, Password);
                int count = popClient.GetMessageCount();
                
                result.Count = count;
                for (int i = count; i >= 1; i--)
                {
                    OpenPOP.MIMEParser.Message msg = popClient.GetMessage(i, false);
                    if (msg != null)
                    {
                        //获取DLL所在路径:
                        string dllPath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                        dllPath = Path.GetDirectoryName(dllPath);
                        //依据所要执行类类型名，获取类实例：
                        string asmNames = asmName;//程序集名称（*.dll）
                        string dllFile = Path.Combine(dllPath, asmNames);
                        Assembly asm = Assembly.LoadFrom(dllFile);
                        //获取类方法并执行：
                        object obj = asm.CreateInstance(typeName, false);
                        Type type = obj.GetType();//类名
                        MethodInfo method = type.GetMethod(methodName);//方法名称
                        //如果需要参数则依此行
                        object[] args = new object[] { (object)msg ,(object)result};
                        //执行并调用方法
                        method.Invoke(obj, args);
                        if (delete)
                        {
                            popClient.DeleteMessage(i); //邮件保存成功，删除服务器备份
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }  
            finally
            {
                popClient.Disconnect();
            }
            return result;
        }

        /// <summary>
        /// 未能正确发送的邮件将以XML形式转存至/_Data/SendEmail/目录下
        /// </summary>
        /// <param name="mailMessage"></param>
        public void SendMailMessageToXml(MailMessage mailMessage)
        {
            try
            {
                string subject = mailMessage.Subject.ToString();//邮件标题
                string body = (string)mailMessage.Body;//邮件正文
                string replyTime = DateTime.Now.ToString();//邮件
                string user = mailMessage.To;//收件人地址
                string formUser = mailMessage.From;//发件人地址

                if (subject != "")
                {
                    string filePath = HttpContext.Current.Server.MapPath("/_Data/SendEmail/");
                    DateTime time = Convert.ToDateTime(replyTime);
                    string fileName = subject + DateTime.Now.ToString(".yyyy_MM_dd_HH_mm_ss") + ".xml";
                    string path = Path.Combine(filePath, fileName);

                    //检查是否XML文件存放临时路径存在，如果不存在则进行处理
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    //检查XMLSchema文件是否存在，如果不存在则进行处理
                    if (!File.Exists(subject))
                    {
                        XmlDocument doc = new XmlDocument();
                        //转换字符
                        subject = Base64Encode(subject);
                        user = Base64Encode(user);
                        body = Base64Encode(body);
                        formUser = Base64Encode(formUser);

                        string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n"
                            + "<root><infoSubject>" + subject + "</infoSubject><infoUser>" +user + "</infoUser><infoFormUser>" +
                            formUser + "</infoFormUser><infoBody>" + body + "</infoBody><infoTime>"
                            + replyTime + "</infoTime></root>";
                        doc.LoadXml(xml);
                        doc.Save(path);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #region 字符转Base64
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected string Base64Encode(string str)
        {
            byte[] b = System.Text.Encoding.Default.GetBytes(str);
            return Convert.ToBase64String(b);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        protected string Base64Decode(string base64String)
        {
            byte[] c = Convert.FromBase64String(base64String);
            return System.Text.Encoding.Default.GetString(c);
        } 
        #endregion

	}

    /// <summary>
    /// 
    /// </summary>
    public class MailResult
    {
        int count;
        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        int success;
        /// <summary>
        /// 
        /// </summary>
        public int Success
        {
            get { return success; }
            set { success = value; }
        }
    }
}


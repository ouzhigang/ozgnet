using System;
using System.Data;
using System.Collections.Generic;
using LumiSoft.Net.Mime;
using LumiSoft.Net.POP3.Client;

/*
List<KeyValuePair<string, Mime>> string为"邮件日期_邮件ID",Mime为邮件类
"邮件日期_邮件ID",加上日期是为了方便排序


实例:
public void ShowEmail(Mime m)
{
    POP3Helper p=new POP3Helper("pop.163.com","ouzhigangGBA","ZG21945");       
 
    List<KeyValuePair<string, Mime>> dd = p.GetEmailsDESC();  //dd中就可以查找出邮件的内容、主题、发件人等信息。你可以通过调试状态的快速监视查看
    foreach (KeyValuePair<string, Mime> mdd in dd)
    {
        if (mdd.Value.Attachments.Length > 0)
        {
            mdd.Attachments[0].DataToFile("f:\\1.txt");
            Page.Response.Write("附件 " + mdd.Value.Attachments[0].ContentDisposition_FileName + "<br><br>");
        }
        Page.Response.Write(mdd.Key + "<br><br>");
        Page.Response.Write(mdd.Value.MainEntity.Date + "<br><br>");
        Page.Response.Write(mdd.Value.MainEntity.Subject + "<br><br>"); //主题
        Page.Response.Write(mdd.Value.BodyHtml + "<br><br>");           //内容
    }
}

*/
namespace OZGNet.Net
{
    /// <summary>
    /// POP3Helper(依赖LumiSoft.Net.dll)
    /// </summary>
    public class POP3Helper
    {
        private string pop3Server;
        private int pop3Port = 110;
        private string username;
        private string password;
        /// <summary>
        /// 实例化POP3Helper
        /// </summary>
        public POP3Helper()
        { }
        /// <summary>
        /// 实例化POP3Helper
        /// </summary>
        /// <param name="pop3Server">POP3服务器</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        public POP3Helper(string pop3Server, string username, string password)
        {
            this.pop3Server = pop3Server;
            this.username = username;
            this.password = password;
        }
        /// <summary>
        /// 实例化POP3Helper
        /// </summary>
        /// <param name="pop3Server">POP3服务器</param>
        /// <param name="pop3Port">端口</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        public POP3Helper(string pop3Server, int pop3Port, string username, string password)
        {
            this.pop3Server = pop3Server;
            this.pop3Port = pop3Port;
            this.username = username;
            this.password = password;
        }

        #region 私有方法
        /// <summary>
        /// pop()私有方法
        /// </summary>
        /// <returns></returns>
        protected POP3_Client pop()
        {
            //需要首先设置这些信息
            //pop3Server = "pop.163.com";    //邮箱服务器 如："pop.sina.com.cn";或 "pop.tom.com" 好像sina的比较快
            //pop3Port = 110;          //端口号码   用"110"好使。最好看一下你的邮箱服务器用的是什么端口号
            bool pop3UseSsl = false;
            //username = "ouzhigangGBA";        //你的邮箱用户名
            //password = "ZG21945";      //你的邮箱密码
            //List<string> gotEmailIds = new List<string>();
            POP3_Client p = new POP3_Client();
            try
            {
                p.Connect(pop3Server, pop3Port, pop3UseSsl);
                p.Authenticate(username, password, false);
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 获取邮件列表(私有方法)
        /// <summary>
        /// 获取邮件列表(私有方法)
        /// </summary>
        /// <returns></returns>
        protected SortedList<string, Mime> getEmails()
        {
            SortedList<string, Mime> result = new SortedList<string, Mime>();

            //获取邮件信息列表
            POP3_ClientMessageCollection infos = this.pop().Messages;

            foreach (POP3_ClientMessage info in infos)
            {
                //每封Email会有一个在Pop3服务器范围内唯一的Id,检查这个Id是否存在就可以知道以前有没有接收过这封邮件
                if (result.ContainsKey(info.UID))
                {
                    continue;
                }
                //获取这封邮件的内容
                byte[] bytes = info.MessageToByte();

                //解析从Pop3服务器发送过来的邮件信息
                Mime mime = Mime.Parse(bytes);

                result.Add(mime.MainEntity.Date.ToString("yyyyMMddHHmmss") + "_" + info.UID, mime);
            }
            return result;
        }
        #endregion

        #region 获取邮件列表(时间顺序)
        /// <summary>
        /// 获取邮件列表(时间顺序)
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<string, Mime>> GetEmailsASC()
        {
            List<KeyValuePair<string, Mime>> result = new List<KeyValuePair<string, Mime>>();
            SortedList<string, Mime> list = this.getEmails();
            foreach (KeyValuePair<string, Mime> mdd in list)
            {
                KeyValuePair<string, Mime> item = new KeyValuePair<string, Mime>(mdd.Key, mdd.Value);
                result.Add(item);
            }
            return result;
        }
        #endregion

        #region 获取邮件列表(时间降序)
        /// <summary>
        /// 获取邮件列表(时间降序)
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<string, Mime>> GetEmailsDESC()
        {
            List<KeyValuePair<string, Mime>> result = new List<KeyValuePair<string, Mime>>();
            List<KeyValuePair<string, Mime>> list = this.GetEmailsASC();

            for (int i = list.Count - 1; i >= 0; i--)
            {
                result.Add(list[i]);
            }
            return result;
        }
        #endregion

        #region 邮件ID打开指定邮件
        /// <summary>
        /// 邮件ID打开指定邮件
        /// </summary>
        /// <param name="ID">邮件ID(格式为“邮件日期_邮件ID”)</param>
        /// <returns></returns>    
        public Mime GetMail(string ID)
        {
            SortedList<string, Mime> result = this.getEmails();
            foreach (KeyValuePair<string, Mime> mdd in result)
            {
                if (mdd.Key == ID)
                {
                    return mdd.Value;
                }
            }
            return null;
        }
        #endregion

        #region 删除邮件
        /// <summary>
        /// 删除邮件
        /// </summary>
        /// <param name="ID">邮件ID(格式为“邮件日期_邮件ID”)</param>
        public void DeleteMail(string ID)
        {
            string compare = null;
            string[] strs = ID.Split('_');
            if (strs.Length > 1)
            {
                compare = strs[1];
            }
            else
            {
                compare = ID;
            }

            //获取邮件信息列表
            POP3_ClientMessageCollection infos = this.pop().Messages;

            foreach (POP3_ClientMessage info in infos)
            {
                if (info.UID == compare)
                {
                    info.MarkForDeletion();
                    break;
                }
            }
        }
        #endregion        

        /// <summary>
        /// 获取或设置POP3服务器
        /// </summary>
        public string Pop3Server
        {
            get { return this.pop3Server; }
            set { this.pop3Server = value; }
        }
        /// <summary>
        /// 获取或设置端口
        /// </summary>
        public int Pop3Port
        {
            get { return this.pop3Port; }
            set { this.pop3Port = value; }
        }
        /// <summary>
        /// 获取或设置用户名
        /// </summary>
        public string UserName
        {
            get { return this.username; }
            set { this.username = value; }
        }
        /// <summary>
        /// 获取或设置密码
        /// </summary>
        public string PassWord
        {
            get { return this.password; }
            set { this.password = value; }
        }

    }
}

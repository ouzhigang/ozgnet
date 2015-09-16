using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;

/*
        private void Form1_Load(object sender, EventArgs e)
        {
            AsyncHttpClient Client = new AsyncHttpClient("http://news.163.com/");
            Client.StartedEvent += Client_StartedEvent;
            Client.ProgressChangedEvent += Client_ProgressChangedEvent;
            Client.CompletedEvent += Client_CompletedEvent;
            Client.ErrorEvent += Client_ErrorEvent;
            Client.Handle();

        }

        void Client_ErrorEvent(object sender, OZGNet.AsyncFileBase.ErrorEventArgs e)
        {
            label1.Text = "错误:" + e.Message;
        }

        void Client_CompletedEvent(object sender, AsyncHttpClient.CompletedEventArgs e)
        {
            label1.Text = "完成";
            //label1.Text = System.Text.Encoding.UTF8.GetString(e.Result);
        }

        void Client_ProgressChangedEvent(object sender, OZGNet.AsyncFileBase.ProgressChangedEventArgs e)
        {
            label1.Text = e.Percentage.ToString();
        }

        void Client_StartedEvent(object sender, OZGNet.AsyncFileBase.StartedEventArgs e)
        {
            label1.Text = "开始";
        }
*/

namespace OZGNet.Net
{
    public class AsyncHttpClient : AsyncFileBase
    {
        
        public class CompletedEventArgs : EventArgs
        { 
            private byte[] Res;

            public CompletedEventArgs(byte[] Res)
            {
                this.Res = Res;
            }

            public byte[] Result
            {
                get
                {
                    return this.Res;
                }
            }
        }
        
        private string Url;
        private string HttpMethod;
        
        private WebRequest Request;

        public WebHeaderCollection Headers;
        public int Tag;

        //事件委托
        public delegate void StartedEventHandler(object sender, StartedEventArgs e);
        public delegate void ProgressChangedEventHandler(object sender, ProgressChangedEventArgs e);
        public delegate void CompletedEventHandler(object sender, CompletedEventArgs e);
        public delegate void ErrorEventHandler(object sender, ErrorEventArgs e);

        //事件
        public event StartedEventHandler StartedEvent;
        public event ProgressChangedEventHandler ProgressChangedEvent;
        public event CompletedEventHandler CompletedEvent;
        public event ErrorEventHandler ErrorEvent;

        public AsyncHttpClient(string Url)
        {
            this.Url = Url;
            this.HttpMethod = WebRequestMethods.Http.Get;

            this.Init();
        }

        public AsyncHttpClient(string Url, string HttpMethod)
        {
            this.Url = Url;
            this.HttpMethod = HttpMethod;

            this.Init();
        }

        private void Init()
        {
            this.SyncContext = SynchronizationContext.Current;

            this.Request = WebRequest.Create(this.Url);

            if (this.Request is HttpWebRequest)
            {
                ((HttpWebRequest)this.Request).UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.85 Safari/537.36";

            }
                        
            this.Request.Method = this.HttpMethod;
            this.Headers = this.Request.Headers;

            this.ReadTotal = 0;
        }

        public override void Handle()
        {
            if (this.HandleStatus == null)
                this.HandleStatus = new WorkStatus();
            else
                this.HandleStatus.ResetData();

            this.HandleThread = new Thread(new ThreadStart(HandleThreadMethod));
            this.HandleThread.IsBackground = true;
            this.HandleThread.Start();

        }

        public override void Cancel()
        {
            //暂无取消功能

        }

        private void HandleThreadMethod()
        {
            this.SyncContext.Post(StartedEventCallBack, null);

            try
            {
                this.HandleStatus.WorkObject = this.Request;
                this.Request.BeginGetResponse(new AsyncCallback(ReqResCallBack), this.HandleStatus);
            }
            catch (Exception ex)
            {
                this.SyncContext.Post(ErrorEventCallBack, ex);
            }
            
        }

        private void ReqResCallBack(IAsyncResult ar)
        {
            try
            {

                HttpWebRequest req = ((WorkStatus)ar.AsyncState).WorkObject as HttpWebRequest;
                HttpWebResponse response = req.EndGetResponse(ar) as HttpWebResponse;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    response.Close();
                    return;
                }
                
                this.HandleStatus.WorkObject = response;
                Stream repsonseStream = response.GetResponseStream();
                this.HandleStatus.OtherObject.Add("OrginalStream", repsonseStream);

                this.ContentTotal = response.ContentLength;

                repsonseStream.BeginRead(this.HandleStatus.Buffer, 0, this.HandleStatus.BufferSize, new AsyncCallback(ReqLoadCallBack), this.HandleStatus);
            }
            catch (Exception ex)
            {
                this.SyncContext.Post(ErrorEventCallBack, ex);
            }

        }

        private void ReqLoadCallBack(IAsyncResult ar)
        {
            try
            {
                WorkStatus ws = (WorkStatus)ar.AsyncState;
                int read = ((Stream)ws.OtherObject["OrginalStream"]).EndRead(ar);

                if (read > 0)
                {
                    ws.MStream.Write(ws.Buffer, 0, read);

                    this.ReadTotal += read;

                    //获取不了总长度的话，就不触发进度事件
                    if (((HttpWebResponse)ws.WorkObject).ContentLength > 0)
                        this.SyncContext.Post(ProgressChangedEventCallBack, ws);

                    ((Stream)ws.OtherObject["OrginalStream"]).BeginRead(ws.Buffer, 0, ws.BufferSize, new AsyncCallback(ReqLoadCallBack), ws);

                    //Thread.Sleep(200); //test
                }
                else
                {
                    
                    ((HttpWebResponse)ws.WorkObject).Close();
                    ((Stream)ws.OtherObject["OrginalStream"]).Close();
                    ((Stream)ws.OtherObject["OrginalStream"]).Dispose();
                    
                    this.SyncContext.Post(CompletedEventCallBack, ws);
                                        
                }
            }
            catch (Exception ex)
            {
                this.SyncContext.Post(ErrorEventCallBack, ex);
            }

        }

        private void StartedEventCallBack(object curr_data)
        {
            if (this.StartedEvent != null)
                this.StartedEvent(this, new StartedEventArgs());
        }

        private void ProgressChangedEventCallBack(object curr_data)
        {
            if (this.ProgressChangedEvent != null)
            {
                WorkStatus ws = (WorkStatus)curr_data;
                this.ProgressChangedEvent(this, new ProgressChangedEventArgs(this.ReadTotal, this.ContentTotal));
            }            
        }

        private void CompletedEventCallBack(object curr_data)
        {
            if (this.CompletedEvent != null)
            {
                WorkStatus ws = (WorkStatus)curr_data;
                this.CompletedEvent(this, new CompletedEventArgs(ws.MStream.ToArray()));
            }
        }

        private void ErrorEventCallBack(object curr_data)
        {
            if (this.ErrorEvent != null)
            {
                this.ErrorEvent(this, new ErrorEventArgs((Exception)curr_data));
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;

namespace OZGNet.Net
{
    public class HttpAsyncClient
    {

        public class StartedEventArgs : EventArgs
        {

        }

        public class ProgressChangedEventArgs : EventArgs
        {
            private long MCurr;
            private long MTotal;
            private float MPercentage;

            public ProgressChangedEventArgs(long Curr, long Total)
            {
                //获取不了总长度的话，就不触发这个事件

                if (Total > 0)
                {
                    this.MCurr = Curr;
                    this.MTotal = Total;
                    this.MPercentage = (float)((double)Curr / (double)Total);
                }
                else
                {                    
                    this.MCurr = 0;
                    this.MTotal = 0;
                    this.MPercentage = 0;
                }
            }

            public long Curr
            {
                get
                {
                    return this.MCurr;
                }
            }

            public long Total
            {
                get
                {
                    return this.MTotal;
                }
            }

            public float Percentage
            {
                get
                {
                    return this.MPercentage;
                }
            }

        }

        public class CompletedEventArgs : EventArgs
        { 
            private byte[] Res;

            public CompletedEventArgs(byte[] Res)
            {
                this.Res = Res;
            }

            public byte[] Results
            {
                get
                {
                    return this.Res;
                }
            }
        }

        public class ErrorEventArgs : EventArgs
        {
            private string Msg;

            public ErrorEventArgs(Exception ex)
            {
                this.Msg = ex.Message;
                
            }

            public string Message
            {
                get
                {
                    return this.Msg;
                }
            }

        }

        private class WebReqState
        {
            public byte[] Buffer;
            public MemoryStream ms;
            public const int BufferSize = 64;
            public Stream OrginalStream;
            public HttpWebResponse WebResponse;

            public WebReqState()
            {
                Buffer = new byte[64];
                ms = new MemoryStream();
            }
        }

        private string Url;
        private string HttpMethod;
        private long ReadTotal;
        private long ContentTotal;

        private HttpWebRequest Request;
        private Thread ReqThread;
        private SynchronizationContext SyncContext = null;

        private WebReqState ReqState;

        public WebHeaderCollection Headers;

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

        public HttpAsyncClient(string Url)
        {
            this.Url = Url;
            this.HttpMethod = WebRequestMethods.Http.Get;

            this.Init();
        }

        public HttpAsyncClient(string Url, string HttpMethod)
        {
            this.Url = Url;
            this.HttpMethod = HttpMethod;

            this.Init();
        }

        private void Init()
        {
            SyncContext = SynchronizationContext.Current;

            this.Request = (HttpWebRequest)WebRequest.Create(this.Url);
            this.Request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.85 Safari/537.36";
            this.Request.Method = this.HttpMethod;

            this.Headers = this.Request.Headers;

            this.ReadTotal = 0;
        }

        public void Req()
        {            
            this.ReqThread = new Thread(new ThreadStart(ReqThreadMethod));
            this.ReqThread.IsBackground = true;
            this.ReqThread.Start();

        }

        public void Cancel()
        {
            //暂无取消功能

        }

        private void ReqThreadMethod()
        {
            SyncContext.Post(StartedEventCallBack, null);

            try
            {
                this.Request.BeginGetResponse(new AsyncCallback(ReqResCallBack), this.Request);
            }
            catch (Exception ex)
            {
                SyncContext.Post(ErrorEventCallBack, ex);
            }
            
        }

        private void ReqResCallBack(IAsyncResult ar)
        {
            try
            {
                HttpWebRequest req = ar.AsyncState as HttpWebRequest;
                HttpWebResponse response = req.EndGetResponse(ar) as HttpWebResponse;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    response.Close();
                    return;
                }

                this.ReqState = new WebReqState();
                this.ReqState.WebResponse = response;
                Stream repsonseStream = response.GetResponseStream();
                this.ReqState.OrginalStream = repsonseStream;

                this.ContentTotal = response.ContentLength;

                repsonseStream.BeginRead(this.ReqState.Buffer, 0, WebReqState.BufferSize, new AsyncCallback(ReqLoadCallBack), this.ReqState);
            }
            catch (Exception ex)
            {
                SyncContext.Post(ErrorEventCallBack, ex);
            }

        }

        private void ReqLoadCallBack(IAsyncResult ar)
        {
            try
            {
                WebReqState rs = ar.AsyncState as WebReqState;
                int read = rs.OrginalStream.EndRead(ar);

                if (read > 0)
                {
                    rs.ms.Write(rs.Buffer, 0, read);

                    this.ReadTotal += read;

                    //获取不了总长度的话，就不触发进度事件
                    if (rs.WebResponse.ContentLength > 0)
                        SyncContext.Post(ProgressChangedEventCallBack, rs);

                    rs.OrginalStream.BeginRead(rs.Buffer, 0, WebReqState.BufferSize, new AsyncCallback(ReqLoadCallBack), rs);

                    //Thread.Sleep(200); //test
                }
                else
                {
                    rs.OrginalStream.Close();
                    rs.WebResponse.Close();

                    SyncContext.Post(CompletedEventCallBack, rs);
                }
            }
            catch (Exception ex)
            {
                SyncContext.Post(ErrorEventCallBack, ex);
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
                WebReqState rs = (WebReqState)curr_data;
                this.ProgressChangedEvent(this, new ProgressChangedEventArgs(this.ReadTotal, this.ContentTotal));
            }            
        }

        private void CompletedEventCallBack(object curr_data)
        {
            if (this.CompletedEvent != null)
            {
                WebReqState rs = (WebReqState)curr_data;
                this.CompletedEvent(this, new CompletedEventArgs(rs.ms.ToArray()));
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

﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;
using System.Collections;
using System.Collections.Specialized;

/*
        需要支持cookie的话，加入ResponseHeadersEvent时间，然后e.Headers.Get("Set-Cookie")获得cookie数据，然后在下一次请求中加入对应的cookie到RequestHeaders，例如RequestHeaders.Add("Cookie", "PHPSESSID=q7ng2dja9kirktah0dv4hh16k6")

        private void Form1_Load(object sender, EventArgs e)
        {
            NameValueCollection HttpParams = new NameValueCollection();
            HttpParams.Add("a", "A");
            HttpParams.Add("b", "B");
            HttpParams.Add("c", "C");
            
            //AsyncHttpClient Client = new AsyncHttpClient("http://localhost/a.php");
            AsyncHttpClient Client = new AsyncHttpClient("http://localhost/a.php", System.Net.WebRequestMethods.Http.Post, HttpParams);
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
    public class AsyncHttpClient : AsyncBase
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

        public class ResponseHeadersEventArgs : EventArgs
        {
            private WebHeaderCollection MHeaders;

            public ResponseHeadersEventArgs(WebHeaderCollection Headers)
            {
                this.MHeaders = Headers;
            }

            public WebHeaderCollection Headers
            {
                get
                {
                    return this.MHeaders;
                }
            }
        }
        
        private string Url;
        private string HttpMethod;
                
        private NameValueCollection HttpParams;
        private byte[] HttpParamsBytes; //post提交时用到

        private HttpWebRequest Request;

        public WebHeaderCollection RequestHeaders;

        //事件委托
        public delegate void StartedEventHandler(object sender, StartedEventArgs e);
        public delegate void ResponseHeadersEventHandler(object sender, ResponseHeadersEventArgs e);
        public delegate void ProgressChangedEventHandler(object sender, ProgressChangedEventArgs e);
        public delegate void CompletedEventHandler(object sender, CompletedEventArgs e);
        public delegate void ErrorEventHandler(object sender, ErrorEventArgs e);

        //事件
        public event StartedEventHandler StartedEvent;
        public event ResponseHeadersEventHandler ResponseHeadersEvent;
        public event ProgressChangedEventHandler ProgressChangedEvent;
        public event CompletedEventHandler CompletedEvent;
        public event ErrorEventHandler ErrorEvent;

        public AsyncHttpClient(string Url)
        {
            this.Url = Url;
            this.HttpMethod = WebRequestMethods.Http.Get;

            this.HttpParams = null;

            this.Init();
        }

        public AsyncHttpClient(string Url, string HttpMethod, NameValueCollection HttpParams)
        {
            this.Url = Url;
            this.HttpMethod = HttpMethod;
            this.HttpParams = HttpParams;

            this.Init();
        }

        private void Init()
        {
            this.SyncContext = SynchronizationContext.Current;

            //如果没有post的数据的话就转到get方式
            if (this.HttpParams == null && this.HttpMethod.ToLower().Equals("post"))
                this.HttpMethod = System.Net.WebRequestMethods.Http.Get;

            if (this.HttpMethod.ToLower().Equals("post"))
            {
                //post提交

                this.Request = (HttpWebRequest)WebRequest.Create(this.Url);

                //用了headers写入cookie的话，这里就不需要了
                //this.Request.CookieContainer = new CookieContainer();
                //this.Request.CookieContainer.Add(new Cookie("PHPSESSID", "fclsdovsq5ba43ctuqibi4m5m6", "/", "localhost"));

                if (this.Request is HttpWebRequest)
                {
                    this.Request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.85 Safari/537.36";
                }

                this.Request.Method = this.HttpMethod;
                this.RequestHeaders = this.Request.Headers;

                string param = string.Empty;
                if (this.HttpParams != null)
                {
                    if (this.HttpParams.Count > 0)
                    {
                        foreach (string item in this.HttpParams.Keys)
                        {
                            param += item + "=" + this.HttpParams[item];
                            param += "&";
                        }
                        param = param.Substring(0, param.Length - 1);
                    }
                    this.HttpParamsBytes = Encoding.UTF8.GetBytes(param);
                    this.Request.ContentType = "application/x-www-form-urlencoded";
                    this.Request.ContentLength = this.HttpParamsBytes.Length;
                }
            }
            else
            {
                //post以外的提交
                if (this.HttpParams != null && this.HttpParams.Count > 0)
                {
                    this.Url += "?";
                    foreach (string item in this.HttpParams.Keys)
                    {
                        this.Url += item + "=" + this.HttpParams[item];
                        this.Url += "&";
                    }
                    this.Url = this.Url.Substring(0, this.Url.Length - 1);
                }
                this.Request = (HttpWebRequest)WebRequest.Create(this.Url);
                if (this.Request is HttpWebRequest)
                {
                    this.Request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.85 Safari/537.36";
                }

                this.Request.Method = this.HttpMethod;
                this.RequestHeaders = this.Request.Headers;
            }

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
            try
            {
                this.HandleThread.Abort();
                this.Request.Abort();
            }
            catch (Exception)
            {

            }

        }

        private void HandleThreadMethod()
        {
            this.SyncContext.Post(StartedEventCallBack, null);

            try
            {
                this.HandleStatus.WorkObject = this.Request;

                if (this.Request.Method.ToLower().Equals("post"))
                {
                    this.Request.BeginGetRequestStream(new AsyncCallback(ReqPostCallBack), this.Request);
                }
                else
                {
                    this.Request.BeginGetResponse(new AsyncCallback(ReqResCallBack), this.HandleStatus);
                }

            }
            catch (Exception ex)
            {
                this.SyncContext.Post(ErrorEventCallBack, ex);
            }
            
        }

        private void ReqPostCallBack(IAsyncResult ar)
        {
            HttpWebRequest request = (HttpWebRequest)ar.AsyncState;
            Stream reqStream = request.EndGetRequestStream(ar);
            reqStream.Write(this.HttpParamsBytes, 0, this.HttpParamsBytes.Length);
            reqStream.Close();

            request.BeginGetResponse(new AsyncCallback(ReqResCallBack), this.HandleStatus);
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
                                
                this.SyncContext.Post(ResponseHeadersEventCallBack, response.Headers);

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

        private void ResponseHeadersEventCallBack(object curr_data)
        {
            if (this.ResponseHeadersEvent != null)
                this.ResponseHeadersEvent(this, new ResponseHeadersEventArgs((WebHeaderCollection)curr_data));
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

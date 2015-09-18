using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;

/*
        private void Form1_Load(object sender, EventArgs e)
        {
            AsyncFileRead FileRead = new AsyncFileRead(@"D:\1.txt");
            FileRead.StartedEvent += FileRead_StartedEvent;
            FileRead.ProgressChangedEvent += FileRead_ProgressChangedEvent;
            FileRead.CompletedEvent += FileRead_CompletedEvent;
            FileRead.ErrorEvent += FileRead_ErrorEvent;
            FileRead.Handle();

        }

        void FileRead_ErrorEvent(object sender, AsyncFileBase.ErrorEventArgs e)
        {
            label1.Text = "错误:" + e.Message;
        }

        void FileRead_CompletedEvent(object sender, AsyncFileRead.CompletedEventArgs e)
        {
            label1.Text = "完成";
            //label1.Text = System.Text.Encoding.UTF8.GetString(e.Result);
        }

        void FileRead_ProgressChangedEvent(object sender, AsyncFileBase.ProgressChangedEventArgs e)
        {
            label1.Text = e.Percentage.ToString();
        }

        void FileRead_StartedEvent(object sender, AsyncFileBase.StartedEventArgs e)
        {
            label1.Text = "开始";
        }
*/
namespace OZGNet
{
	public class AsyncFileRead : AsyncBase
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
		
		private string TargetPath;

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

        public AsyncFileRead(string TargetPath)
		{
			this.TargetPath = TargetPath;
			
			this.Init();
		}
		
		private void Init()
        {
            this.SyncContext = SynchronizationContext.Current;
			
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
                FileStream targetFile = new FileStream(this.TargetPath, FileMode.Open, FileAccess.Read);
                this.ContentTotal = targetFile.Length;
                this.BeginReadData(targetFile);

            }
            catch (Exception ex)
            {
                this.SyncContext.Post(ErrorEventCallBack, ex);
            }
		}

        private void BeginReadData(FileStream fs)
        {
            this.HandleStatus.WorkObject = fs;
            fs.BeginRead(this.HandleStatus.Buffer, 0, this.HandleStatus.BufferSize, new AsyncCallback(EndReadData), this.HandleStatus);
            
        }

        private void EndReadData(IAsyncResult ar)
        {
            try
            {
                WorkStatus ws = (WorkStatus)ar.AsyncState;
                int read = ((FileStream)ws.WorkObject).EndRead(ar);

                if (read > 0)
                {
                    ws.MStream.Write(ws.Buffer, 0, read);
                    this.ReadTotal += read;

                    this.SyncContext.Post(ProgressChangedEventCallBack, ws);

                    //test
                    //Thread.Sleep(10);

                    //继续下一轮读取
                    this.BeginReadData((FileStream)ws.WorkObject);
                }
                else
                {
                    //完成
                    ((FileStream)ws.WorkObject).Close();
                    ((FileStream)ws.WorkObject).Dispose();

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

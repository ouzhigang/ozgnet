using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;

/*
		private void Form1_Load(object sender, EventArgs e)
        {
            AsyncFileCopy FileCopy = new AsyncFileCopy(@"D:\1.txt", @"D:\2.txt");
            FileCopy.StartedEvent += FileCopy_StartedEvent;
            FileCopy.ProgressChangedEvent += FileCopy_ProgressChangedEvent;
            FileCopy.CompletedEvent += FileCopy_CompletedEvent;
            FileCopy.ErrorEvent += FileCopy_ErrorEvent;
            FileCopy.Handle();

        }

        void FileCopy_ErrorEvent(object sender, AsyncFileBase.ErrorEventArgs e)
        {
            label1.Text = "����:" + e.Message;
        }

        void FileCopy_CompletedEvent(object sender, AsyncFileCopy.CompletedEventArgs e)
        {
            label1.Text = "���";
        }

        void FileCopy_ProgressChangedEvent(object sender, AsyncFileBase.ProgressChangedEventArgs e)
        {
            label1.Text = e.Percentage.ToString();
        }

        void FileCopy_StartedEvent(object sender, AsyncFileBase.StartedEventArgs e)
        {
            label1.Text = "��ʼ";
        }
*/

namespace OZGNet
{
	public class AsyncFileCopy : AsyncFileBase
	{
		public class CompletedEventArgs : EventArgs
        { 
            public CompletedEventArgs()
            {

            }
        }

        private string SrcPath;
        private string DstPath;

        //�¼�ί��
        public delegate void StartedEventHandler(object sender, StartedEventArgs e);
        public delegate void ProgressChangedEventHandler(object sender, ProgressChangedEventArgs e);
        public delegate void CompletedEventHandler(object sender, CompletedEventArgs e);
        public delegate void ErrorEventHandler(object sender, ErrorEventArgs e);

        //�¼�
        public event StartedEventHandler StartedEvent;
        public event ProgressChangedEventHandler ProgressChangedEvent;
        public event CompletedEventHandler CompletedEvent;
        public event ErrorEventHandler ErrorEvent;

		public AsyncFileCopy(string SrcPath, string DstPath)
		{
            this.SrcPath = SrcPath;
            this.DstPath = DstPath;

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
            //����ȡ������

        }

        private void HandleThreadMethod()
        {
            this.SyncContext.Post(StartedEventCallBack, null);
            try
            {
                //���Ŀ���ļ����ڣ�����ɾ��
                if (File.Exists(this.DstPath))
                    File.Delete(this.DstPath);

                FileStream srcFile = new FileStream(this.SrcPath, FileMode.Open, FileAccess.Read);
                FileStream dstFile = new FileStream(this.DstPath, FileMode.Append, FileAccess.Write);
                this.ContentTotal = srcFile.Length;
                this.BeginReadData(srcFile, dstFile);

            }
            catch (Exception ex)
            {
                this.SyncContext.Post(ErrorEventCallBack, ex);
            }
        }

        private void BeginReadData(FileStream srcFile, FileStream dstFile)
        {
            this.HandleStatus.WorkObject = srcFile;

            if (!this.HandleStatus.OtherObject.ContainsKey("DstFile"))
                this.HandleStatus.OtherObject.Add("DstFile", dstFile);
            
            srcFile.BeginRead(this.HandleStatus.Buffer, 0, this.HandleStatus.BufferSize, new AsyncCallback(EndReadData), this.HandleStatus);
        }

        private void EndReadData(IAsyncResult ar)
        {
            try
            {
                WorkStatus ws = (WorkStatus)ar.AsyncState;
                int read = ((FileStream)ws.WorkObject).EndRead(ar);

                if (read > 0)
                {
                    FileStream dstFile = (FileStream)ws.OtherObject["DstFile"];
                    
                    this.ReadTotal += read;

                    this.SyncContext.Post(ProgressChangedEventCallBack, ws);

                    dstFile.BeginWrite(ws.Buffer, 0, ws.BufferSize, new AsyncCallback(EndWriteData), ws);
                }
                else
                {
                    //���
                    ((FileStream)ws.WorkObject).Close();
                    ((FileStream)ws.WorkObject).Dispose();

                    FileStream dstFile = (FileStream)ws.OtherObject["DstFile"];
                    dstFile.Close();
                    dstFile.Dispose();

                    this.SyncContext.Post(CompletedEventCallBack, ws);
                }
            }
            catch (Exception ex)
            {
                this.SyncContext.Post(ErrorEventCallBack, ex);
            }
        }

        private void EndWriteData(IAsyncResult ar)
        {
            try
            {
                WorkStatus ws = (WorkStatus)ar.AsyncState;
                FileStream dstFile = (FileStream)ws.OtherObject["DstFile"];
                dstFile.EndWrite(ar);

                //test
                //Thread.Sleep(10);

                //������һ�ֶ�ȡ
                this.BeginReadData((FileStream)ws.WorkObject, dstFile);
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
                this.CompletedEvent(this, new CompletedEventArgs());
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

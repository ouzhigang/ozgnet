using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;

namespace OZGNet
{	
	public abstract class AsyncBase
	{
		protected class WorkStatus
		{
			private object MWorkObject = null;
			private byte[] MBuffer = null;
			private int MBufferSize = 64;
            private MemoryStream MMStream;
			private Dictionary<string, object> MOtherObject = null;

            public WorkStatus()
			{
				
			}

            ~WorkStatus()
            {
                this.ResetData();

            }
			
			/// <summary>
			/// 获取或设置工作对象
			/// </summary>
			public object WorkObject
			{
				get
				{
					return this.MWorkObject;
				}
				set
				{
					this.MWorkObject = value;
				}
			}
			
			/// <summary>
			/// 获取或设置缓冲区
			/// </summary>        
			public byte[] Buffer
			{
				get
				{
					if (this.MBuffer == null)					
						this.MBuffer = new byte[this.MBufferSize];					
					return this.MBuffer;
				}
				set
				{
					this.MBuffer = value;
				}
			}
			
			/// <summary>
			/// 获取或设置缓冲区大小
			/// </summary>
			public int BufferSize
			{
				get
				{
					return this.MBufferSize;
				}
				set
				{
					this.MBufferSize = value;
				}
			}
			
			/// <summary>
			/// 获取其他对象(此对象集合可修改)
			/// </summary>
			public Dictionary<string, object> OtherObject
			{
				get
				{
					if(this.MOtherObject == null)
						this.MOtherObject = new Dictionary<string, object>();
					return this.MOtherObject;
				}
			}

            public MemoryStream MStream
            {
                get
                {
                    if (this.MMStream == null)
                        this.MMStream = new MemoryStream();
                    return this.MMStream;
                }
            }

			/// <summary>
			/// 重置数据
			/// </summary>
			public void ResetData()
			{
				this.MWorkObject = null;
				this.MBuffer = null;
				this.MBufferSize = 64;
				
				if(this.MOtherObject != null)
					this.MOtherObject.Clear();

                if (this.MMStream != null)
                {
                    this.MMStream.Dispose();
                    this.MMStream = null;
                }
                    
			}

		}
		
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
        
        public class ErrorEventArgs : EventArgs
        {
            private Exception MError;

            public ErrorEventArgs(Exception ex)
            {
                this.MError = ex;
                
            }

            public Exception Error
            {
                get
                {
                    return this.MError;
                }
            }

        }

        protected Thread HandleThread;
        protected SynchronizationContext SyncContext;

        protected WorkStatus HandleStatus;

        protected long ReadTotal;
        protected long ContentTotal;

		public abstract void Handle();

        public abstract void Cancel();

	}
	
}

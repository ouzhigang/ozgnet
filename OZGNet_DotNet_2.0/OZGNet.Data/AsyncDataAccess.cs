using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

/*

        private void Form1_Load(object sender, EventArgs e)
        {

            AsyncDataAccess AsyncData = new AsyncDataAccess(new MySqlHelper("127.0.0.1", "root", "root", "xxx", "utf8"));
            AsyncData.StartedEvent += AsyncData_StartedEvent;
            AsyncData.ProgressChangedEvent += AsyncData_ProgressChangedEvent;
            AsyncData.CompletedEvent += AsyncData_CompletedEvent;
            AsyncData.ErrorEvent += AsyncData_ErrorEvent;
            AsyncData.SqlMethod = AsyncDataAccess.SqlMethodOption.DataTable;
            AsyncData.SqlStrObj = "select * from reg_user order by id desc";
            AsyncData.Handle();
        }

        void AsyncData_ErrorEvent(object sender, OZGNet.AsyncBase.ErrorEventArgs e)
        {
            label1.Text = "错误:" + e.Error.Message;
        }

        void AsyncData_CompletedEvent(object sender, AsyncDataAccess.CompletedEventArgs e)
        {
            dataGridView1.DataSource = e.Result;
            label1.Text = "完成";
        }

        void AsyncData_ProgressChangedEvent(object sender, OZGNet.AsyncBase.ProgressChangedEventArgs e)
        {

        }

        void AsyncData_StartedEvent(object sender, OZGNet.AsyncBase.StartedEventArgs e)
        {
            label1.Text = "正在查询";
        }
 */

namespace OZGNet.Data
{
    public class AsyncDataAccess : AsyncBase
    {
        public class CompletedEventArgs : EventArgs
        {
            private object Res;

            public CompletedEventArgs(object Res)
            {
                this.Res = Res;
            }

            public object Result
            {
                get
                {
                    return this.Res;
                }
            }
        }

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

        private IDataHelper Db;
        private object MSqlStrObj;
        private SqlMethodOption MSqlMethod;

        public int Tag;

        public enum SqlMethodOption
        {
            DataSet,
            DataTable,
            ExecuteNonQuery,
            ExecuteNonQueryTransaction,
            ExecuteReader,
            ExecuteScalar,
            SingleRow
        }
        
        /// <summary>
        /// 本属性为需要执行的sql语句或语句组
        /// </summary>
        public object SqlStrObj
        {
            get
            {
                return this.MSqlStrObj;
            }
            set
            {
                this.MSqlStrObj = value;
            }
        }

        public SqlMethodOption SqlMethod
        {
            get
            {
                return this.MSqlMethod;
            }
            set
            {
                this.MSqlMethod = value;
            }
        }

        public AsyncDataAccess(IDataHelper db)
        {
            this.Db = db;
            this.SqlMethod = SqlMethodOption.ExecuteNonQuery;

            this.Init();
        }

        private void Init()
        {
            this.SyncContext = SynchronizationContext.Current;

            this.ReadTotal = 0;
            this.ContentTotal = 0;
        }

        public override void Handle()
        {
            if (this.HandleStatus == null)
                this.HandleStatus = new WorkStatus();
            else
                this.HandleStatus.ResetData();

            if (this.SqlStrObj == null)
            {
                throw new Exception("SqlStrObj属性不能为空");
            }

            this.HandleThread = new Thread(new ThreadStart(HandleThreadMethod));
            this.HandleThread.IsBackground = true;
            this.HandleThread.Start();
        }

        public override void Cancel()
        {
            try
            {
                this.HandleThread.Abort();

            }
            catch (Exception)
            {

            }
        }

        private void HandleThreadMethod()
        {
            this.SyncContext.Post(StartedEventCallBack, null);
            this.SyncContext.Post(ProgressChangedEventCallBack, null);

            try
            {
                this.HandleStatus.WorkObject = this.Db;

                object res = null;

                switch (this.SqlMethod)
                {
                    case SqlMethodOption.DataSet:
                        res = this.Db.DataSet(this.SqlStrObj.ToString());                        
                        break;
                    case SqlMethodOption.DataTable:
                        res = this.Db.DataTable(this.SqlStrObj.ToString());         
                        break;
                    case SqlMethodOption.ExecuteNonQuery:
                        res = this.Db.ExecuteNonQuery(this.SqlStrObj.ToString());
                        break;
                    case SqlMethodOption.ExecuteNonQueryTransaction:
                        res = this.Db.ExecuteNonQueryTransaction((IList<string>)this.SqlStrObj);
                        break;
                    case SqlMethodOption.ExecuteReader:
                        res = this.Db.ExecuteReader(this.SqlStrObj.ToString());
                        break;
                    case SqlMethodOption.ExecuteScalar:
                        res = this.Db.ExecuteScalar(this.SqlStrObj.ToString());
                        break;
                    case SqlMethodOption.SingleRow:
                        res = this.Db.SingleRow(this.SqlStrObj.ToString());
                        break;
                    default:
                        break;
                }

                this.SyncContext.Post(CompletedEventCallBack, res);
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
                this.ProgressChangedEvent(this, new ProgressChangedEventArgs(0, 0));
            }
        }

        private void CompletedEventCallBack(object curr_data)
        {
            if (this.CompletedEvent != null)
            {
                this.CompletedEvent(this, new CompletedEventArgs(curr_data));
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

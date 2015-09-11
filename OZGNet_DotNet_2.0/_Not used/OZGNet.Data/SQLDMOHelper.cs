using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;
using SQLDMO;

namespace OZGNet.Data
{
    /// <summary>
    /// SQLDMO辅助类
    /// </summary>
    public class SQLDMOHelper
    {
        #region DatabaseInfo
        /// <summary>
        /// 数据库信息
        /// </summary>
        public struct DatabaseInfo
        {
            /// <summary>
            /// 
            /// </summary>
            public string Name;
            /// <summary>
            /// 
            /// </summary>
            public string Owner;
            /// <summary>
            /// 
            /// </summary>
            public string PrimaryFilePath;
            /// <summary>
            /// 
            /// </summary>
            public string CreateDate;
            /// <summary>
            /// 
            /// </summary>
            public int Size;
            /// <summary>
            /// 
            /// </summary>
            public float SpaceAvailable;
            /// <summary>
            /// 
            /// </summary>
            public string PrimaryName;
            /// <summary>
            /// 
            /// </summary>
            public string PrimaryFilename;
            /// <summary>
            /// 
            /// </summary>
            public int PrimarySize;
            /// <summary>
            /// 
            /// </summary>
            public int PrimaryMaxSize;
            /// <summary>
            /// 
            /// </summary>
            public string LogName;
            /// <summary>
            /// 
            /// </summary>
            public string LogFilename;
            /// <summary>
            /// 
            /// </summary>
            public int LogSize;
            /// <summary>
            /// 
            /// </summary>
            public int LogMaxSize;
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                string s = "Name:{0}\r\n" +
                  "Owner:{1}\r\n" +
                  "PrimaryFilePath:{2}\r\n" +
                  "CreateDate:{3}\r\n" +
                  "Size:{4}MB\r\n" +
                  "SpaceAvailable:{5}MB\r\n" +
                  "PrimaryName:{6}\r\n" +
                  "PrimaryFilename:{7}\r\n" +
                  "PrimarySize:{8}MB\r\n" +
                  "PrimaryMaxSize:{9}MB\r\n" +
                  "LogName:{10}\r\n" +
                  "LogFilename:{11}\r\n" +
                  "LogSize:{12}MB\r\n" +
                  "LogMaxSize:{13}MB";

                return string.Format(s, Name, Owner, PrimaryFilePath, CreateDate, Size,
                  SpaceAvailable, PrimaryName, PrimaryFilename, PrimarySize,
                  PrimaryMaxSize, LogName, LogFilename, LogSize, LogMaxSize);
            }
        }

        #endregion
        private SQLServer2 sqlServer;
        private string server;
        private string login;
        private string password;
        /// <summary>
        /// 实例化SQLDMOHelper
        /// </summary>
        /// <param name="server">SqlServer服务器</param>
        /// <param name="login">用户名</param>
        /// <param name="password">密码</param>
        public SQLDMOHelper(string server, string login, string password)
        {
            this.server = server;
            this.login = login;
            this.password = password;

            sqlServer = new SQLServer2Class();
            sqlServer.Connect(server, login, password);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Close()
        {
            sqlServer.Close();
        }

        #region Property

        /// <summary>
        /// 获取主要版本号
        /// </summary>
        public string Version
        {
            get
            {
                return string.Format("{0}.{1}", sqlServer.VersionMajor, sqlServer.VersionMinor);
            }
        }

        /// <summary>
        /// 获取详细版本信息
        /// </summary>
        public string VersionString
        {
            get
            {
                return sqlServer.VersionString;
            }
        }

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        public string ServerTime
        {
            get
            {
                return sqlServer.ServerTime;
            }
        }

        /// <summary>
        /// 获取系统服务名称
        /// </summary>
        public string ServiceName
        {
            get
            {
                return sqlServer.ServiceName;
            }
        }

        /// <summary>
        /// 获取或设置系统服务是否自动启动
        /// </summary>
        public bool AutostartServer
        {
            get
            {
                return sqlServer.Registry.AutostartServer;
            }
            set
            {
                sqlServer.Registry.AutostartServer = value;
            }
        }

        /// <summary>
        /// 获取字符集设置
        /// </summary>
        public string CharacterSet
        {
            get
            {
                return sqlServer.Registry.CharacterSet;
            }
        }

        /// <summary>
        /// 获取服务器物理内存大小(MB)
        /// </summary>
        public int PhysicalMemory
        {
            get
            {
                return sqlServer.Registry.PhysicalMemory;
            }
        }

        /// <summary>
        /// 获取服务器处理器(CPU)数量
        /// </summary>
        public int NumberOfProcessors
        {
            get
            {
                return sqlServer.Registry.NumberOfProcessors;
            }
        }

        #endregion

        #region Public Method

        /// <summary>
        /// 获取网络内所有可用的服务器
        /// </summary>
        /// <returns></returns>
        public static string[] ListAvailableSQLServers()
        {
            NameList servers = new ApplicationClass().ListAvailableSQLServers();
            if (servers.Count <= 0) return new string[0];

            ArrayList list = new ArrayList(servers.Count);
            foreach (object o in servers) list.Add(o);
            return (string[])list.ToArray(typeof(string));
        }

        /// <summary>
        /// 断开数据库所有连接
        /// </summary>
        /// <param name="dbName"></param>
        public void KillAllProcess(string dbName)
        {
            QueryResults qr = sqlServer.EnumProcesses(-1);

            // 获取SPID和DBNAME字段列序号
            int iColPIDNum = -1;
            int iColDbName = -1;
            for (int i = 1; i <= qr.Columns; i++)
            {
                string strName = qr.get_ColumnName(i);

                if (strName.ToUpper().Trim() == "SPID")
                    iColPIDNum = i;
                else if (strName.ToUpper().Trim() == "DBNAME")
                    iColDbName = i;

                if (iColPIDNum != -1 && iColDbName != -1)
                    break;
            }

            // 将指定数据库的连接全部断开
            for (int i = 1; i <= qr.Rows; i++)
            {
                int lPID = qr.GetColumnLong(i, iColPIDNum);
                string strDBName = qr.GetColumnString(i, iColDbName);

                if (string.Compare(strDBName, "test", true) == 0)
                    sqlServer.KillProcess(lPID);
            }
        }

        /// <summary>
        /// 获取数据库信息
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public DatabaseInfo GetDatabaseInfo(string dbName)
        {
            Database db = GetDatabase(dbName);
            if (db == null) throw new Exception("Database not exists!");

            DatabaseInfo info = new DatabaseInfo();

            info.Name = db.Name;
            info.Owner = db.Owner;
            info.PrimaryFilePath = db.PrimaryFilePath;
            info.CreateDate = db.CreateDate;
            info.Size = db.Size;
            info.SpaceAvailable = db.SpaceAvailableInMB;

            DBFile primary = db.FileGroups.Item("PRIMARY").DBFiles.Item(1);
            info.PrimaryName = primary.Name;
            info.PrimaryFilename = primary.PhysicalName.Trim();
            info.PrimarySize = primary.Size;
            info.PrimaryMaxSize = primary.MaximumSize;

            _LogFile log = db.TransactionLog.LogFiles.Item(1);
            info.LogName = log.Name;
            info.LogFilename = log.PhysicalName.Trim();
            info.LogSize = log.Size;
            info.LogMaxSize = log.MaximumSize;

            return info;
        }

        /// <summary>
        /// 分离数据库
        /// </summary>
        /// <param name="dbName"></param>
        /// <remarks>
        /// 分离前最好调用KillAllProcess关闭所有连接，否则分离可能失败。
        /// </remarks>
        public void DetachDB(string dbName)
        {
            sqlServer.DetachDB(dbName, true);
        }

        /// <summary>
        /// 附加数据库
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="dbFile"></param>
        /// <example>
        /// <code>
        /// SqlDmoHelper dmo = new SqlDmoHelper("(local)", "sa", "sa");
        /// dmo.AttachDB("test", @"d:\temp\database\test_data.mdf");
        /// </code>
        /// </example>
        public void AttachDB(string dbName, string dbFile)
        {
            sqlServer.AttachDB(dbName, dbFile);
        }

        /// <summary>
        /// 删除数据库(文件也将被删除)
        /// </summary>
        /// <param name="dbName"></param>
        public void KillDB(string dbName)
        {
            sqlServer.KillDatabase(dbName);
        }

        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="dbName">数据库名称</param>
        /// <param name="path">数据文件保存路径</param>
        /// <param name="primaryFilename">数据库文件名(不含路径)</param>
        /// <param name="logFilename">日志文件名(不含路径)</param>
        /// <example>
        /// <code>
        /// SqlDmoHelper dmo = new SqlDmoHelper("(local)", "sa", "sa");
        /// dmo.CreateDB("test1", @"d:\temp\database", "abc.mdf", "abc1.ldf");
        /// </code>
        /// </example>
        public void CreateDB(string dbName, string path, string primaryFilename, string logFilename)
        {
            // 创建数据库文件
            DBFile dbFile = new DBFileClass();
            dbFile.Name = dbName + "_Data";
            dbFile.PhysicalName = Path.Combine(path, primaryFilename);
            dbFile.PrimaryFile = true;
            //dbFile.Size = 2; // 设置初始化大小(MB)
            //dbFile.FileGrowthType = SQLDMO_GROWTH_TYPE.SQLDMOGrowth_MB; // 设置文件增长方式
            //dbFile.FileGrowth=1; // 设置增长幅度

            // 创建日志文件
            _LogFile logFile = new LogFileClass();
            logFile.Name = dbName + "_Log";
            logFile.PhysicalName = Path.Combine(path, logFilename);
            //logFile.Size = 3;
            //logFile.FileGrowthType=SQLDMO_GROWTH_TYPE.SQLDMOGrowth_MB;
            //logFile.FileGrowth=1;

            // 创建数据库
            Database db = new DatabaseClass();
            db.Name = dbName;
            db.FileGroups.Item("PRIMARY").DBFiles.Add(dbFile);
            db.TransactionLog.LogFiles.Add(logFile);

            // 建立数据库联接，并添加数据库到服务器
            sqlServer.Databases.Add(db);
        }

        /// <summary>
        /// 备份数据库
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="bakFile"></param>
        /// <param name="bakSetName"></param>
        /// <param name="bakDescription"></param>
        /// <example>
        /// <code>
        /// SqlDmoHelper dmo = new SqlDmoHelper("(local)", "sa", "sa");
        /// dmo.BackupDB("test", @"d:\temp\database\test.bak", "手动备份1", "备份说明...");
        /// </code>
        /// </example>
        public void BackupDB(string dbName, string bakFile, string bakSetName, string bakDescription)
        {
            Backup oBackup = new BackupClass();
            oBackup.Action = SQLDMO_BACKUP_TYPE.SQLDMOBackup_Database;
            oBackup.Database = dbName;
            oBackup.Files = bakFile;
            oBackup.BackupSetName = bakSetName;
            oBackup.BackupSetDescription = bakDescription;
            oBackup.Initialize = true;
            oBackup.SQLBackup(sqlServer);
        }

        /// <summary>
        /// 恢复数据库
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="bakFile"></param>
        /// <remarks>
        /// 恢复前最好调用KillAllProcess关闭所有连接，否则恢复可能失败。
        /// </remarks>
        /// <example>
        /// <code>
        /// SqlDmoHelper dmo = new SqlDmoHelper("(local)", "sa", "sa");
        /// dmo.RestoreDB("test", @"d:\temp\database\test.bak");
        /// </code>
        /// </example>
        public void RestoreDB(string dbName, string bakFile)
        {
            Restore oRestore = new RestoreClass();
            oRestore.Action = SQLDMO_RESTORE_TYPE.SQLDMORestore_Database;
            oRestore.Database = dbName;
            oRestore.Files = bakFile;
            oRestore.FileNumber = 1;
            oRestore.ReplaceDatabase = true;
            oRestore.SQLRestore(sqlServer);
        }

        /// <summary>
        /// 收缩数据库
        /// </summary>
        /// <param name="dbName"></param>
        public void ShrinkDB(string dbName)
        {
            Database db = GetDatabase(dbName);
            if (db == null) throw new Exception("Database not exists!");

            db.Shrink(0, SQLDMO_SHRINK_TYPE.SQLDMOShrink_Default);
        }

        /// <summary>
        /// 获取所有的数据库名
        /// </summary>
        /// <returns></returns>
        public string[] ListAllDatabase()
        {
            ArrayList list = new ArrayList();
            foreach (Database d in sqlServer.Databases)
            {
                list.Add(d.Name);
            }

            if (list.Count == 0)
                return new string[0];
            else
                return (string[])list.ToArray(typeof(string));
        }

        /// <summary>
        /// 获取所有登录名
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 管理工具 "安全性->登录"
        /// </remarks>
        public string[] ListAllLogins()
        {
            ArrayList list = new ArrayList();
            foreach (Login d in sqlServer.Logins)
            {
                list.Add(d.Name);
            }

            if (list.Count == 0)
                return new string[0];
            else
                return (string[])list.ToArray(typeof(string));
        }

        /// <summary>
        /// 获取全部数据表名称
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public string[] ListAllTables(string dbName)
        {
            Database db = GetDatabase(dbName);
            if (db == null) throw new Exception("Database not exists!");

            ArrayList list = new ArrayList();
            foreach (Table t in db.Tables)
            {
                list.Add(t.Name);
            }

            if (list.Count == 0)
                return new string[0];
            else
                return (string[])list.ToArray(typeof(string));
        }

        /// <summary>
        /// 获取全部存储过程名称
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public string[] ListAllStoredProcedure(string dbName)
        {
            Database db = GetDatabase(dbName);
            if (db == null) throw new Exception("Database not exists!");

            ArrayList list = new ArrayList();
            foreach (StoredProcedure sp in db.StoredProcedures)
            {
                list.Add(sp.Name);
            }

            if (list.Count == 0)
                return new string[0];
            else
                return (string[])list.ToArray(typeof(string));
        }

        /// <summary>
        /// 获取数据库对象
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        /// <remarks>
        /// 可以通过数据库对象获取数据库内表、存储过程、触发器、数据类型等信息。
        /// </remarks>
        /// <example>
        /// 显示数据库中所有表及其结构
        /// <code>
        /// SqlDmoHelper dmo = new SqlDmoHelper("(local)", "sa", "sa");
        /// SQLDMO.Database db = dmo.GetDatabase("test");
        /// foreach(SQLDMO.Table t in db.Tables)
        /// {
        ///    Console.WriteLine("Table:{0}", t.Name);
        ///    for (int i = 1; i &lt;= t.Columns.Count; i++) // SQLDMO所有索引序号从1开始
        ///    {
        ///      SQLDMO._Column col = t.Columns.Item(i);
        ///      Console.WriteLine(" Column:{0} DataType:{1}", col.Name, col.Datatype);
        ///    }
        ///
        ///    Console.WriteLine("---------------");
        /// }
        /// </code>
        /// </example>
        public Database GetDatabase(string dbName)
        {
            foreach (Database d in sqlServer.Databases)
            {
                if (string.Compare(d.Name, dbName, true) == 0)
                    return d;
            }

            return null;
        }

        #endregion
    }
}

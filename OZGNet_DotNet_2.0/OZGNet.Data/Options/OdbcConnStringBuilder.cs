using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace OZGNet.Data.Options
{
    /// <summary>
    /// Odbc连接字符类
    /// </summary>
    public class OdbcConnStringBuilder
    {
        /// <summary>
        /// Access连接字符
        /// </summary>
        /// <param name="path">数据库路径</param>
        /// <returns></returns>
        public static string Access(string path)
        {
            return "Driver={Microsoft Access Driver (*.mdb)};Dbq=" + path + ";Uid=;Pwd=;";
        }
        /// <summary>
        /// Access连接字符
        /// </summary>
        /// <param name="path">数据库路径</param>
        /// <param name="user">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static string Access(string path, string user, string pwd)
        {
            return "Driver={Microsoft Access Driver (*.mdb)};Dbq=" + path + ";Uid=" + user + ";Pwd=" + pwd + ";";
        }

        /// <summary>
        /// Excel连接字符
        /// </summary>
        /// <param name="path">Excel路径</param>
        /// <returns></returns>
        public static string Excel(string path)
        {
            return "Driver={Microsoft Excel Driver (*.xls)};DriverId=790;Dbq=" + path + ";DefaultDir=" + System.IO.Path.GetDirectoryName(path);
        }
        /// <summary>
        /// Excel连接字符
        /// </summary>
        /// <param name="path">Excel路径</param>
        /// <param name="defaultDir">默认路径</param>
        /// <returns></returns>
        public static string Excel(string path, string defaultDir)
        {
            return "Driver={Microsoft Excel Driver (*.xls)};DriverId=790;Dbq=" + path + ";DefaultDir=" + defaultDir;
        }
        /// <summary>
        /// SqlServer连接字符
        /// </summary>
        /// <param name="server">SqlServer服务器</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="user">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static string SqlServer(string server, string dbName, string user, string pwd)
        {
            return "Driver={SQL Server};Server=" + server + ";Database=" + dbName + ";Uid=" + user + ";Pwd=" + pwd + ";";
        }
        /// <summary>
        /// MySql连接字符（MySQL ODBC 3.51 Driver）
        /// </summary>
        /// <param name="server">MySQL服务器</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="user">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static string MySql351(string server, string dbName, string user, string pwd)
        {
            return "DRIVER={MySQL ODBC 3.51 Driver};SERVER=" + server + ";DATABASE=" + dbName + ";USER=" + user + ";PASSWORD=" + pwd + ";";
        }
        /// <summary>
        /// MySql连接字符（MySQL ODBC 5.1 Driver）
        /// </summary>
        /// <param name="server">MySQL服务器</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="user">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static string MySql51(string server, string dbName, string user, string pwd)
        {
            return "DRIVER={MySQL ODBC 5.1 Driver};SERVER=" + server + ";DATABASE=" + dbName + ";USER=" + user + ";PASSWORD=" + pwd + ";";
        }
        /// <summary>
        /// Firebird连接字符
        /// </summary>
        /// <param name="path">Firebird本地路径</param>
        /// <param name="user">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static string Firebird(string path, string user, string pwd)
        {
            return "DRIVER={Firebird/InterBase(r) Driver};UID=" + user + ";PWD=" + pwd + ";DBNAME=" + path;
        }
        /// <summary>
        /// Firebird连接字符
        /// </summary>
        /// <param name="server">Firebird服务器</param>
        /// <param name="port"></param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="user">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static string Firebird(string server, string port, string dbName, string user, string pwd)
        {
            return Firebird(server, port, dbName, user, pwd, "NONE");
        }
        /// <summary>
        /// Firebird连接字符
        /// </summary>
        /// <param name="server">Firebird服务器</param>
        /// <param name="port"></param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="user">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="Charset">编码</param>
        /// <returns></returns>
        public static string Firebird(string server, string port, string dbName, string user, string pwd, string Charset)
        {
            return Firebird(server, port, dbName, user, pwd, Charset, "0", "50");
        }
        /// <summary>
        /// Firebird连接字符
        /// </summary>
        /// <param name="server">Firebird服务器</param>
        /// <param name="port"></param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="user">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="Charset">编码</param>
        /// <param name="MinPoolSize">连接池最小连接数</param>
        /// <param name="MaxPoolSize">连接池最大连接数</param>
        /// <returns></returns>
        public static string Firebird(string server, string port, string dbName, string user, string pwd, string Charset, string MinPoolSize, string MaxPoolSize)
        {
            return "User=" + user + ";Password=" + pwd + ";Database=" + dbName + ";DataSource=" + server + ";Port=" + port + ";Dialect=3;Charset=" + Charset + ";Role=;Connection lifetime=15;Pooling=true;MinPoolSize=" + MinPoolSize + ";MaxPoolSize=" + MaxPoolSize + ";Packet Size=8192;ServerType=0";
        }
        /// <summary>
        /// PostgreSQL连接字符
        /// </summary>
        /// <param name="server">PostgreSQL服务器</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="user">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static string PostgreSQL(string server, string dbName, string user, string pwd)
        {
            return "Driver={PostgreSQL ANSI};server=" + server + ";uid=" + user + ";pwd=" + pwd + ";database=" + dbName;
        }
        /// <summary>
        /// SQLite连接字符
        /// </summary>
        /// <param name="path">数据库路径</param>
        /// <returns></returns>
        public static string SQLite(string path)
        {
            return "Driver={SQLite ODBC Driver};Database=" + path;
        }
        /// <summary>
        /// SQLite3连接字符
        /// </summary>
        /// <param name="path">数据库路径</param>
        /// <returns></returns>
        public static string SQLite3(string path)
        {
            return "Driver={SQLite3 ODBC Driver};Database=" + path;
        }
        /// <summary>
        /// Sybase连接字符
        /// </summary>
        /// <param name="server">Sybase服务器</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="user">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static string Sybase(string server, string dbName, string user, string pwd)
        {
            return "Driver={SYBASE ASE ODBC Driver};Srvr=" + server + ";Uid=" + user + ";Pwd=" + pwd + ";Database=" + dbName;
        }
        /// <summary>
        /// Sybase连接字符
        /// </summary>
        /// <param name="server">Sybase服务器</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="user">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static string Sybase11(string server, string dbName, string user, string pwd)
        {
            return "Driver={SYBASE SYSTEM 11};Srvr=" + server + ";Uid=" + user + ";Pwd=" + pwd + ";Database=" + dbName;
        }

        /// <summary>
        /// Interbase连接字符
        /// </summary>
        /// <param name="server">Interbase服务器</param>
        /// <param name="db">localhost:C:\mydatabase.gdb</param>
        /// <param name="user">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static string Interbase(string server, string db, string user, string pwd)
        {
            return "Driver={Easysoft Interbase ODBC};Server=" + server + ";Database=" + db + "; Uid=" + user + ";Pwd=" + pwd + ";";
        }

        /// <summary>
        /// DB2连接字符
        /// </summary>
        /// <param name="server">DB2服务器</param>
        /// <param name="port">端口</param>
        /// <param name="db">数据库名称</param>
        /// <param name="user">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static string DB2(string server, string port, string db, string user, string pwd)
        {
            return "Driver={IBM DB2 ODBC DRIVER};Database=" + db + ";Hostname=" + server + ";Port=" + port + "; Protocol=TCPIP;Uid=" + user + ";Pwd=" + pwd + ";";
        }

        /// <summary>
        /// MimerSQL连接字符
        /// </summary>
        /// <param name="db">数据库名称</param>
        /// <param name="user">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static string Mimer(string db, string user, string pwd)
        {
            return "Driver={MIMER};Database=" + db + ";Uid=" + user + ";Pwd=" + pwd + ";";
        }
        /// <summary>
        /// Pervasive连接字符
        /// </summary>
        /// <param name="ServerName"></param>
        /// <param name="dbq"></param>
        /// <returns></returns>
        public static string Pervasive(string ServerName, string dbq)
        {
            return "Driver={Pervasive ODBC Client Interface};ServerName=" + ServerName + ";dbq=@" + dbq + ";";
        }
        /// <summary>
        /// SQLBase连接字符
        /// </summary>
        /// <param name="server">SQLBase服务器</param>
        /// <param name="db">数据库名称</param>
        /// <param name="user">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static string SQLBase(string server, string db, string user, string pwd)
        {
            return "Driver={SQLBaseODBC};ServerName=" + server + ";Database=" + db + ";LogonID=" + user + ";Password=" + pwd + ";";
        }

        /// <summary>
        /// NetezzaSQL连接字符
        /// </summary>
        /// <param name="server">NetezzaSQL服务器</param>
        /// <param name="port">端口</param>
        /// <param name="db">数据库名称</param>
        /// <param name="user">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static string NetezzaSQL(string server, string port, string db, string user, string pwd)
        {
            return "Driver={NetezzaSQL};servername=" + server + ";port=" + port + ";database=" + db + "; username=" + user + ";password=" + pwd + ";";
        }
        /// <summary>
        /// Valentina连接字符
        /// </summary>
        /// <param name="path">数据库路径</param>
        /// <returns></returns>
        public static string Valentina(string path)
        {
            return "Driver={Valentina ODBC Driver};IsDatabaseLocal=yes;Database=" + path + ";";
        }
        /// <summary>
        /// Valentina连接字符
        /// </summary>
        /// <param name="server">NetezzaSQL服务器</param>
        /// <param name="port">端口</param>
        /// <param name="db">数据库名称</param>
        /// <param name="user">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static string Valentina(string server, string port, string db, string user, string pwd)
        {
            return "Driver={Valentina ODBC Driver};Host=" + server + ";Port=" + port + ";Database=" + db + ";UID=" + user + ";PWD=" + pwd + ";";
        }


    }

}

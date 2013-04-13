using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace OZGNet.WebForm.ThirdPart.DalianOA
{
    public class DBHelper
    {
        private string connectionString;
        private DatabaseTypes databaseType;

        public DBHelper()
        {
        }

        public DBHelper(DatabaseTypes type, string connectionString)
        {
            this.databaseType = type;
            this.connectionString = connectionString;
        }

        public DbConnection CreateConnection()
        {
            switch (this.databaseType)
            {
                case DatabaseTypes.MySql:
                    return new MySqlConnection(this.connectionString);

                case DatabaseTypes.Oracle:
                    return new OracleConnection(this.connectionString);

                case DatabaseTypes.OleDb:
                    return new OleDbConnection(this.connectionString);
            }
            return new SqlConnection(this.connectionString);
        }

        public DbParameter CreateDbParameter(string paraName, DbType dbType, int size, object value, ParameterDirection direction)
        {
            DbParameter parameter;
            switch (this.databaseType)
            {
                case DatabaseTypes.MySql:
                    parameter = new MySqlParameter();
                    break;

                case DatabaseTypes.Oracle:
                    parameter = new OracleParameter();
                    break;

                case DatabaseTypes.OleDb:
                    parameter = new OleDbParameter();
                    break;

                default:
                    parameter = new SqlParameter();
                    break;
            }
            parameter.ParameterName = paraName;
            if (size != 0)
            {
                parameter.Size = size;
            }
            parameter.DbType = dbType;
            if (value != null)
            {
                parameter.Value = value;
            }
            parameter.Direction = direction;
            return parameter;
        }

        public DbParameter CreateInDbParameter(string paraName, DbType dbType, object value)
        {
            return this.CreateDbParameter(paraName, dbType, 0, value, ParameterDirection.Input);
        }

        public DbParameter CreateInDbParameter(string paraName, DbType dbType, int size, object value)
        {
            return this.CreateDbParameter(paraName, dbType, size, value, ParameterDirection.Input);
        }

        public DbParameter CreateOutDbParameter(string paraName, DbType dbType)
        {
            return this.CreateDbParameter(paraName, dbType, 0, null, ParameterDirection.Output);
        }

        public DbParameter CreateOutDbParameter(string paraName, DbType dbType, int size)
        {
            return this.CreateDbParameter(paraName, dbType, size, null, ParameterDirection.Output);
        }

        public DbParameter CreateReturnDbParameter(string paraName, DbType dbType)
        {
            return this.CreateDbParameter(paraName, dbType, 0, null, ParameterDirection.ReturnValue);
        }

        public DbParameter CreateReturnDbParameter(string paraName, DbType dbType, int size)
        {
            return this.CreateDbParameter(paraName, dbType, size, null, ParameterDirection.ReturnValue);
        }

        public int ExecuteNonQuery(CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            return this.dbHelper.ExecuteNonQuery(this.connectionString, cmdType, cmdText, cmdParms);
        }

        public int ExecuteNonQuery(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            return this.dbHelper.ExecuteNonQuery(trans, cmdType, cmdText, cmdParms);
        }

        public DataSet ExecuteQuery(CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            return this.dbHelper.ExecuteQuery(this.connectionString, cmdType, cmdText, cmdParms);
        }

        public DataSet ExecuteQuery(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            return this.dbHelper.ExecuteQuery(trans, cmdType, cmdText, cmdParms);
        }

        public DbDataReader ExecuteReader(CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            return this.dbHelper.ExecuteReader(this.connectionString, cmdType, cmdText, cmdParms);
        }

        public DbDataReader ExecuteReader(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            return this.dbHelper.ExecuteReader(trans, cmdType, cmdText, cmdParms);
        }

        public object ExecuteScalar(CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            return this.dbHelper.ExecuteScalar(this.connectionString, cmdType, cmdText, cmdParms);
        }

        public object ExecuteScalar(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            return this.dbHelper.ExecuteScalar(trans, cmdType, cmdText, cmdParms);
        }

        public byte[] GetBinary(object obj)
        {
            if (obj.ToString() != "")
            {
                return (byte[]) obj;
            }
            return null;
        }

        public bool GetBool(object obj)
        {
            return ((obj.ToString() == "1") || (obj.ToString().ToLower() == "true"));
        }

        public byte GetByte(object obj)
        {
            if (obj.ToString() != "")
            {
                return byte.Parse(obj.ToString());
            }
            return 0;
        }

        public int GetCount(string tblName, string condition)
        {
            return this.dbHelper.GetCount(this.connectionString, tblName, condition);
        }

        public DateTime GetDateTime(object obj)
        {
            if ((obj.ToString() != "") && (obj.ToString() != "0000-0-0 0:00:00"))
            {
                return DateTime.Parse(obj.ToString());
            }
            return DateTime.MinValue;
        }

        public decimal GetDecimal(object obj)
        {
            if (obj.ToString() != "")
            {
                return decimal.Parse(obj.ToString());
            }
            return 0M;
        }

        public double GetDouble(object obj)
        {
            if (obj.ToString() != "")
            {
                return double.Parse(obj.ToString());
            }
            return 0.0;
        }

        public float GetFloat(object obj)
        {
            if (obj.ToString() != "")
            {
                return float.Parse(obj.ToString());
            }
            return 0f;
        }

        public Guid GetGuid(object obj)
        {
            if (obj.ToString() != "")
            {
                return new Guid(obj.ToString());
            }
            return Guid.Empty;
        }

        public int GetInt(object obj)
        {
            if (obj.ToString() != "")
            {
                return int.Parse(obj.ToString());
            }
            return 0;
        }

        public short GetInt16(object obj)
        {
            if (obj.ToString() != "")
            {
                return short.Parse(obj.ToString());
            }
            return 0;
        }

        public long GetLong(object obj)
        {
            if (obj.ToString() != "")
            {
                return long.Parse(obj.ToString());
            }
            return 0L;
        }

        public DbDataReader GetPageList(string tblName, int pageSize, int pageIndex, string fldSort, bool sort, string condition)
        {
            return this.dbHelper.GetPageList(this.connectionString, tblName, pageSize, pageIndex, fldSort, sort, condition);
        }

        public string GetString(object obj)
        {
            return obj.ToString();
        }

        public uint GetUInt(object obj)
        {
            if (obj.ToString() != "")
            {
                return uint.Parse(obj.ToString());
            }
            return 0;
        }

        public ushort GetUInt16(object obj)
        {
            if (obj.ToString() != "")
            {
                return ushort.Parse(obj.ToString());
            }
            return 0;
        }

        public ulong GetULong(object obj)
        {
            if (obj.ToString() != "")
            {
                return ulong.Parse(obj.ToString());
            }
            return 0L;
        }

        public string ConnectionString
        {
            get
            {
                return this.connectionString;
            }
            set
            {
                this.connectionString = value;
            }
        }

        public DatabaseTypes DatabaseType
        {
            get
            {
                return this.databaseType;
            }
            set
            {
                this.databaseType = value;
            }
        }

        private IDBHelper dbHelper
        {
            get
            {
                switch (this.databaseType)
                {
                    case DatabaseTypes.MySql:
                        return new MySqlHelper();

                    case DatabaseTypes.Oracle:
                        return new MySqlHelper();

                    case DatabaseTypes.OleDb:
                        return new OleDbHelper();
                }
                return new SqlHelper();
            }
        }

        public enum DatabaseTypes
        {
            Sql,
            MySql,
            Oracle,
            OleDb
        }
    }
}


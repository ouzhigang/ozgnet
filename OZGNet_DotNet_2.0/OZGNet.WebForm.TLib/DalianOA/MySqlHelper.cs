﻿using System;
using System.Data;
using System.Data.Common;
using System.Text;
using MySql.Data.MySqlClient;

namespace OZGNet.WebForm.ThirdPart.DalianOA
{
    public class MySqlHelper : PageHelper, IDBHelper
    {
        public int ExecuteNonQuery(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            MySqlCommand cmd = new MySqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            int num = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return num;
        }

        public int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            MySqlCommand cmd = new MySqlCommand();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, cmdParms);
                int num = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return num;
            }
        }

        public DataSet ExecuteQuery(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            MySqlCommand cmd = new MySqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "ds");
            cmd.Parameters.Clear();
            return dataSet;
        }

        public DataSet ExecuteQuery(string connectionString, CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            DataSet set2;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    PrepareCommand(command, connection, null, cmdType, cmdText, cmdParms);
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet, "ds");
                        command.Parameters.Clear();
                        set2 = dataSet;
                    }
                }
            }
            return set2;
        }

        public DbDataReader ExecuteReader(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            MySqlCommand cmd = new MySqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return reader;
        }

        public DbDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            DbDataReader reader2;
            MySqlCommand cmd = new MySqlCommand();
            MySqlConnection conn = new MySqlConnection(connectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                reader2 = reader;
            }
            catch
            {
                conn.Close();
                throw;
            }
            return reader2;
        }

        public object ExecuteScalar(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            MySqlCommand cmd = new MySqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            object obj2 = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return obj2;
        }

        public object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            MySqlCommand cmd = new MySqlCommand();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, cmdParms);
                object obj2 = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return obj2;
            }
        }

        public int GetCount(string connectionString, string tblName, string condition)
        {
            StringBuilder builder = new StringBuilder("select count(*) from " + tblName);
            if (!string.IsNullOrEmpty(condition))
            {
                builder.Append(" where " + condition);
            }
            return int.Parse(this.ExecuteScalar(connectionString, CommandType.Text, builder.ToString(), null).ToString());
        }

        public DbDataReader GetPageList(string connectionString, string tblName, int pageSize, int pageIndex, string fldSort, bool sort, string condition)
        {
            string cmdText = base.GetPagerSQL(condition, pageSize, pageIndex, fldSort, tblName, sort);
            return this.ExecuteReader(connectionString, CommandType.Text, cmdText, null);
        }

        private static void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction trans, CommandType cmdType, string cmdText, DbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText.Replace("@", "?");
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                foreach (MySqlParameter parameter in cmdParms)
                {
                    parameter.ParameterName = parameter.ParameterName.Replace("@", "?");
                    cmd.Parameters.Add(parameter);
                }
            }
        }
    }
}


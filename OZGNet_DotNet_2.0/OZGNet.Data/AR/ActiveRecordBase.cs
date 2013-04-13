using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Configuration;

namespace OZGNet.Data.AR
{
    /// <summary>
    /// 未完成功能： Odbc未支持，未做连表，Join Group Having 未测试，各字段未映射类型，目前因为技术原因没有办法将此类的实例序列化，希望有这方面经验的人与本人联系，不胜感激！
    /// 
    /// config文件配置
    /// <connectionStrings>
    /// <add name="SqlConn" connectionString="server=127.0.0.1;database=ceshi;user=sa;password=123456;Connect Timeout=30" providerName="OZGNet.Data.SqlHelper" />
    /// <!-- 本地路径 -->
    /// <add name="SqlConn" connectionString="Provider=Microsoft.Jet.OLEDB.4.0; Data Source={$WEB_PATH}App_Data/db1.mdb" providerName="OZGNet.Data.OleDbHelper" />
    /// </connectionStrings>
    /// 
    /// 实体类声明
    ///[ActiveRecord("admin")]
    ///public class Admin : ActiveRecordBase
    ///{
    ///    public Admin()
    ///    {
    ///
    ///    }

    ///    [PrimaryKey("id")]
    ///    public int ID
    ///    {
    ///        get;
    ///        set;
    ///    }

    ///    [Property("name")]
    ///    public string Name
    ///    {
    ///        get;
    ///        set;
    ///    }

    ///    [Property("pwd")]
    ///    public string Pwd
    ///    {
    ///        get;
    ///        set;
    ///    }
    ///    
    ///     public static Admin CreateInstance()
    ///    {
    ///        return (Admin)ActiveRecordBase.CreateInstance(typeof(Admin));
    ///    }
    ///}
    /// 
    /// 
    /// </summary>
    public class ActiveRecordBase
    {
        private OZGNet.Data.IDataHelper db = null;
        private string primary_key;
        private string tableName;
		
        /// <summary>
        /// 是否为新的记录
        /// </summary>
        public bool IsNewData = true;		

        public ActiveRecordBase()
        {
            string config_string_name = "SqlConn";
            KeyValuePair<string, string> result = this.GetPathAndClass(ConfigurationManager.ConnectionStrings[config_string_name].ProviderName);
            db = (OZGNet.Data.IDataHelper)Assembly.Load(result.Key).CreateInstance(result.Key + "." + result.Value);
            db.ConnString = ConfigurationManager.ConnectionStrings[config_string_name].ConnectionString;
            if (db.ConnString.Contains("{$WEB_PATH}"))
            {
                db.ConnString = db.ConnString.Replace("{$WEB_PATH}", System.Web.HttpContext.Current.Server.MapPath("~/"));
                db.ConnString = db.ConnString.Replace("/", "\\");
            }

            //获取表名称
            tableName = ((ActiveRecord)System.Attribute.GetCustomAttribute(this.GetType(), typeof(ActiveRecord))).TableName();
            //获取主键名称
            foreach (System.Reflection.PropertyInfo item in this.GetType().GetProperties())
            {
                foreach (System.Attribute attr in item.GetCustomAttributes(true))
                {
                    if (attr is PrimaryKey)
                    {
                        primary_key = (attr as PrimaryKey).GetPrimaryKey();
                        break;
                    }
                }
            }
        }

        ~ActiveRecordBase()
        {

        }

        public IDbConnection GetConnection()
        {
            return db.GetConnection();
        }

        public IDataHelper GetHelper()
        {
            return db;
        }

        public int ExecuteSql(string sql)
        {
            return db.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// FindOne方法前执行
        /// </summary>
        protected virtual void FindOneBeforce() { }
        /// <summary>
        /// FindOne方法后执行
        /// </summary>
        protected virtual void FindOneAfter() { }

        /// <summary>
        /// 获取第一行第一列
        /// </summary>
        /// <returns></returns>
        public Object FindOne()
        {
            Criteria condition = new Criteria();
            condition.Func = "count";
            condition.Select = primary_key;
            return this.FindOne(condition);
        }

        /// <summary>
        /// 获取第一行第一列
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public Object FindOne(Criteria condition)
        {
            this.FindOneBeforce();

            if (condition.Func == null)
            {
                condition.Func = "count";
            }
            if (condition.Select == null)
            {
                condition.Select = primary_key;
            }

            string sql = OZGNet.Data.SqlText.Func(condition.Func, condition.Select, tableName, condition.Where);
            object result = db.ExecuteScalar(sql);

            this.FindOneAfter();
            return result;
        }

        /// <summary>
        /// FindOneBySql方法前执行
        /// </summary>
        protected virtual void FindOneBySqlBeforce() { }
        /// <summary>
        /// FindOneBySql方法后执行
        /// </summary>
        protected virtual void FindOneBySqlAfter() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Object FindOneBySql(string sql)
        {
            this.FindOneBySqlBeforce();

            object result = db.ExecuteScalar(sql);

            this.FindOneBySqlAfter();
            return result;
        }

        /// <summary>
        /// Save方法前执行
        /// </summary>
        protected virtual void SaveBeforce() { }
        /// <summary>
        /// Save方法后执行
        /// </summary>
        protected virtual void SaveAfter() { }

        /// <summary>
        /// 新数据则新增，旧数据则更新
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            this.SaveBeforce();
                                    
            Hashtable fields = new Hashtable();
            string where = "";
            foreach (System.Reflection.PropertyInfo item in this.GetType().GetProperties())
            {
                foreach (System.Attribute attr in item.GetCustomAttributes(true))
                {
                    object primary_key_value = item.GetValue(this, null);
                    if (primary_key_value != null)
                    {
                        if (attr is Property)
                        {
                            if (primary_key_value is DateTime)
                            {
                                primary_key_value = primary_key_value.ToString();
                            }
                            else if (primary_key_value is bool)
                            {
                                primary_key_value = primary_key_value.ToString().ToLower() == "true" ? 1 : 0;
                            }

                            fields.Add((attr as Property).GetFieldName(), primary_key_value);
                        }
                        else if (!IsNewData && attr is PrimaryKey)
                        {
                            primary_key = (attr as PrimaryKey).GetPrimaryKey();                            
                            if (primary_key_value is string || primary_key_value is DateTime)
                            {
                                where = primary_key + " = '" + this.ReplaceSqlString(primary_key_value.ToString()) + "'";
                            }
                            else if (primary_key_value is bool)
                            {
                                where = primary_key + " = " + ((Convert.ToBoolean(primary_key_value)) ? "1" : "0");
                            }
                            else
                            {
                                where = primary_key + " = " + primary_key_value.ToString();
                            }
                        }
                    }

                }
            }

            string sql = this.IsNewData ? OZGNet.Data.SqlText.Insert(fields, tableName) : OZGNet.Data.SqlText.Update(fields, tableName, where);
            
            //调试
            //System.IO.File.WriteAllText(@"c:\1.txt", sql);

            int result = db.ExecuteNonQuery(sql);

            this.SaveAfter();
            return result > 0 ? true : false;
        }

        /// <summary>
        /// Find方法前执行
        /// </summary>
        protected virtual void FindBeforce() { }
        /// <summary>
        /// Find方法后执行
        /// </summary>
        protected virtual void FindAfter() { }

        /// <summary>
        /// 获取一行数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Object Find(int id)
        {
            Criteria criteria = new Criteria();
            criteria.Where = primary_key + " = " + id.ToString();
            return this.Find(criteria);
        }

        /// <summary>
        /// 获取一行数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public Object Find(Criteria criteria)
        {
            this.FindBeforce();

            string sql = OZGNet.Data.SqlText.Select(null, criteria.Select, tableName, criteria.Where, criteria.Order);
            DataRow dr = db.SingleRow(sql);
            if (dr == null)
            {
                return null;
            }

            KeyValuePair<string, string> result = this.GetPathAndClass(this.GetType().ToString());
            Object obj = Assembly.Load(result.Key).CreateInstance(result.Key + "." + result.Value);
            foreach (System.Reflection.PropertyInfo item in obj.GetType().GetProperties())
            {
                foreach (System.Attribute attr in item.GetCustomAttributes(true))
                {
                    if (attr is Property)
                    {
                        if (dr[(attr as Property).GetFieldName()] != DBNull.Value)
                        {
                            item.SetValue(obj, dr[(attr as Property).GetFieldName()], null);
                        }
                    }
                    else if (attr is PrimaryKey)
                    {
                        if (dr[(attr as PrimaryKey).GetPrimaryKey()] != DBNull.Value)
                        {
                            item.SetValue(obj, dr[(attr as PrimaryKey).GetPrimaryKey()], null);
                        }
                    }
                }
            }
            obj.GetType().GetField("IsNewData").SetValue(obj, false);

            this.FindAfter();
            return obj;
        }

        /// <summary>
        /// FindBySql方法前执行
        /// </summary>
        protected virtual void FindBySqlBeforce() { }
        /// <summary>
        /// FindBySql方法后执行
        /// </summary>
        protected virtual void FindBySqlAfter() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Object FindBySql(string sql)
        {
            this.FindBySqlBeforce();

            DataRow dr = db.SingleRow(sql);
            if (dr == null)
            {
                return null;
            }

            KeyValuePair<string, string> result = this.GetPathAndClass(this.GetType().ToString());
            Object obj = Assembly.Load(result.Key).CreateInstance(result.Key + "." + result.Value);
            foreach (System.Reflection.PropertyInfo item in obj.GetType().GetProperties())
            {
                foreach (System.Attribute attr in item.GetCustomAttributes(true))
                {
                    if (attr is Property)
                    {
                        try
                        {
                            if (dr[(attr as Property).GetFieldName()] != DBNull.Value)
                            {
                                item.SetValue(obj, dr[(attr as Property).GetFieldName()], null);
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            item.SetValue(obj, null, null);
                        }
                        
                    }
                    else if (attr is PrimaryKey)
                    {
                        try
                        {
                            if (dr[(attr as PrimaryKey).GetPrimaryKey()] != DBNull.Value)
                            {
                                item.SetValue(obj, dr[(attr as PrimaryKey).GetPrimaryKey()], null);
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            item.SetValue(obj, 0, null);
                        }
                        
                    }
                }
            }
            obj.GetType().GetField("IsNewData").SetValue(obj, false);

            this.FindBySqlAfter();
            return obj;
        }

        /// <summary>
        /// FindAll方法前执行
        /// </summary>
        protected virtual void FindAllBeforce() { }
        /// <summary>
        /// FindAll方法前执行
        /// </summary>
        protected virtual void FindAllAfter() { }

        public IList FindAll()
        {
            Criteria criteria = new Criteria();
            return this.FindAll(criteria);
        }

        public IList FindAll(Criteria criteria)
        {
            this.FindAllBeforce();

            IList list = new ArrayList();

            /*
            目前发现：
            支持limit的数据库：sqlite, mysql, postgre        
            不支持limit的数据库：access, mssql
            都不支持的数据库: firebird, oracle, db2, mimer, pervasive, sybase, teradata, vistadb             
           
            */

            int mode = 1;
            if (db is OZGNet.Data.SQLiteHelper || db is OZGNet.Data.MySqlHelper || db is OZGNet.Data.NpgsqlHelper)
            {
                mode = 2;
            }
            else if (db is OZGNet.Data.FirebirdHelper || db is OZGNet.Data.OracleHelper || db is OZGNet.Data.DB2Helper || db is OZGNet.Data.MimerHelper || db is OZGNet.Data.PervasiveHelper || db is OZGNet.Data.SybaseHelper || db is OZGNet.Data.TeradataHelper || db is OZGNet.Data.VistaDBHelper)
            {
                mode = 3;
            }
            else if (db is OZGNet.Data.OdbcHelper)
            {
                //Odbc暂不做处理
                mode = 3;
            }

            //别名
            string AliasSelect = "";

            //这里使用数据库连接类型来判定使用mode1还是mode2
            string sql = "";
            switch(mode)
            {
                case 1:
                    //使用top
                    if (criteria.Offset > 0 && criteria.Limit > 0)
                    {
                        sql = "select top " + criteria.Limit.ToString();

                        
                        if (!string.IsNullOrEmpty(criteria.Alias))
                        {
                            AliasSelect = criteria.Alias + ".";
                            criteria.Alias = " as " + criteria.Alias;
                        }
                        else
                        {
                            AliasSelect = tableName + ".";
                            criteria.Alias = "";
                        }

                        if (!string.IsNullOrEmpty(criteria.Select))
                        {
                            sql += " " + criteria.Select;
                        }
                        else
                        {
                            sql += " " + AliasSelect + "*";
                        }

                        if (string.IsNullOrEmpty(criteria.Join))
                        {
                            criteria.Join = "";
                        }
                        
                        string where = "";
                        if (!string.IsNullOrEmpty(criteria.Where))
                        {
                            where = " and " + criteria.Where;
                        }

                        if (!string.IsNullOrEmpty(criteria.Group))
                        {
                            criteria.Group = "group by " + criteria.Group;
                        }
                        else
                        {
                            criteria.Group = "";
                        }

                        if (!string.IsNullOrEmpty(criteria.Having))
                        {
                            criteria.Having = " having " + criteria.Having;
                        }
                        else
                        {
                            criteria.Having = "";
                        }

                        if (!string.IsNullOrEmpty(criteria.Order))
                        {
                            criteria.Order = " order by " + criteria.Order;
                        }
                        else
                        {
                            criteria.Order = "";
                        }

                        sql += " from " + tableName + " " + criteria.Alias + " " + criteria.Join + " where " + AliasSelect + primary_key + " not in (select top " + criteria.Offset.ToString() + " " + AliasSelect + primary_key + "  from " + tableName + " " + criteria.Alias + " " + criteria.Join + " where 1 = 1 " + where + " " + criteria.Group + " " + criteria.Having + " " + criteria.Order + ") " + where + " " + criteria.Group + " " + criteria.Having + " " + criteria.Order;
                    }
                    else
                    {
                        sql = "select ";
                        if (criteria.Limit > 0)
                        {
                            sql += " top " + criteria.Limit.ToString();
                        }

                        if (!string.IsNullOrEmpty(criteria.Alias))
                        {
                            AliasSelect = criteria.Alias + ".";
                            criteria.Alias = " as " + criteria.Alias;
                        }
                        else
                        {
                            AliasSelect = tableName + ".";
                            criteria.Alias = "";
                        }

                        if (!string.IsNullOrEmpty(criteria.Select))
                        {
                            sql += " " + criteria.Select;
                        }
                        else
                        {
                            sql += " " + AliasSelect + "*";
                        }                 

                        if (string.IsNullOrEmpty(criteria.Join))
                        {
                            criteria.Join = "";
                        }

                        if (!string.IsNullOrEmpty(criteria.Where))
                        {
                            criteria.Where = " where " + criteria.Where;
                        }
                        else
                        {
                            criteria.Where = "";
                        }

                        if (!string.IsNullOrEmpty(criteria.Group))
                        {
                            criteria.Group = "group by " + criteria.Group;
                        }
                        else
                        {
                            criteria.Group = "";
                        }

                        if (!string.IsNullOrEmpty(criteria.Order))
                        {
                            criteria.Order = " order by " + criteria.Order;
                        }
                        else
                        {
                            criteria.Order = "";
                        }

                        if (!string.IsNullOrEmpty(criteria.Having))
                        {
                            criteria.Having = " having " + criteria.Having;
                        }
                        else
                        {
                            criteria.Having = "";
                        }

                        sql += " from " + tableName + criteria.Alias + " " + criteria.Join + " " + criteria.Where + " " + criteria.Group + " " + criteria.Having + " " + criteria.Order;
                    }
                    break;
                case 2:
                    //使用limit

                    if (!string.IsNullOrEmpty(criteria.Alias))
                    {
                        AliasSelect = criteria.Alias + ".";
                        criteria.Alias = " as " + criteria.Alias;
                    }
                    else
                    {
                        AliasSelect = tableName + ".";
                        criteria.Alias = "";
                    }
           
                    if (string.IsNullOrEmpty(criteria.Select))
                    {
                        criteria.Select = AliasSelect + "*";
                    }

                    if (string.IsNullOrEmpty(criteria.Join))
                    {
                        criteria.Join = "";
                    }

                    if (!string.IsNullOrEmpty(criteria.Where))
                    {
                        criteria.Where = " where " + criteria.Where;
                    }
                    else
                    {
                        criteria.Where = "";
                    }

                    if (!string.IsNullOrEmpty(criteria.Group))
                    {
                        criteria.Group = "group by " + criteria.Group;
                    }
                    else
                    {
                        criteria.Group = "";
                    }

                    if (!string.IsNullOrEmpty(criteria.Order))
                    {
                        criteria.Order = " order by " + criteria.Order;
                    }
                    else
                    {
                        criteria.Order = "";
                    }

                    if (!string.IsNullOrEmpty(criteria.Having))
                    {
                        criteria.Having = " having " + criteria.Having;
                    }
                    else
                    {
                        criteria.Having = "";
                    }

                    string limit = "";
                    if (criteria.Offset > 0 && criteria.Limit > 0)
                    {
						if(db is OZGNet.Data.NpgsqlHelper)
						{
							limit = "limit " + criteria.Offset.ToString() + " offset " + criteria.Limit.ToString();
						}
						else
						{
							limit = "limit " + criteria.Offset.ToString() + ", " + criteria.Limit.ToString();
						}                        
                    }
                    else if (criteria.Limit > 0)
                    {
                        limit = "limit " + criteria.Limit.ToString();
                    }

                    sql = "select " + criteria.Select + " from " + tableName + criteria.Alias + " " + criteria.Join + " " + criteria.Where + " " + criteria.Group + " " + criteria.Having + " " + criteria.Order + " " + limit;

                    break;
                default:
                    //目前mode3表示不支持
                    throw new Exception(db.GetType().Name + " 目前不支持该方法，请与作者联系：ouzhigangGBA@gmail.com");
            }

            //调试
            //System.IO.File.WriteAllText(@"c:\1.txt", sql);

            DataTable dt = db.DataTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                KeyValuePair<string, string> result = this.GetPathAndClass(this.GetType().ToString());
                Object obj = Assembly.Load(result.Key).CreateInstance(result.Key + "." + result.Value);
                foreach (System.Reflection.PropertyInfo item in obj.GetType().GetProperties())
                {
                    foreach (System.Attribute attr in item.GetCustomAttributes(true))
                    {
                        if (attr is Property)
                        {
                            try
                            {
                                if (dr[(attr as Property).GetFieldName()] != DBNull.Value)
                                {
                                    item.SetValue(obj, dr[(attr as Property).GetFieldName()], null);
                                }
                            }
                            catch (ArgumentException ex)
                            {
                                item.SetValue(obj, null, null);
                            }
                        }
                        else if (attr is PrimaryKey)
                        {
                            try
                            {
                                if (dr[primary_key] != DBNull.Value)
                                {
                                    item.SetValue(obj, Convert.ToInt32(dr[primary_key]), null);
                                }
                            }
                            catch (ArgumentException ex)
                            {
                                item.SetValue(obj, 0, null);
                            }
                        }
                    }
                }
                obj.GetType().GetField("IsNewData").SetValue(obj, false);
                list.Add(obj);
            }

            this.FindAllAfter();
            return list;
        }

        /// <summary>
        /// FindAllBySql方法前执行
        /// </summary>
        protected virtual void FindAllBySqlBeforce() { }
        /// <summary>
        /// FindAllBySql方法前执行
        /// </summary>
        protected virtual void FindAllBySqlAfter() { }

        public IList FindAllBySql(string sql)
        {
            this.FindAllBySqlBeforce();

            IList list = new ArrayList();

            DataTable dt = db.DataTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                KeyValuePair<string, string> result = this.GetPathAndClass(this.GetType().ToString());
                Object obj = Assembly.Load(result.Key).CreateInstance(result.Key + "." + result.Value);
                foreach (System.Reflection.PropertyInfo item in obj.GetType().GetProperties())
                {
                    foreach (System.Attribute attr in item.GetCustomAttributes(true))
                    {
                        if (attr is Property)
                        {
                            try
                            {
                                if (dr[(attr as Property).GetFieldName()] != DBNull.Value)
                                {
                                    item.SetValue(obj, dr[(attr as Property).GetFieldName()], null);
                                }
                            }
                            catch (ArgumentException ex)
                            {
                                item.SetValue(obj, null, null);
                            }
                        }
                        else if (attr is PrimaryKey)
                        {
                            try
                            {
                                if (dr[primary_key] != DBNull.Value)
                                {
                                    item.SetValue(obj, Convert.ToInt32(dr[primary_key]), null);
                                }
                            }
                            catch (ArgumentException ex)
                            {
                                item.SetValue(obj, 0, null);
                            }
                        }
                    }
                }
                obj.GetType().GetField("IsNewData").SetValue(obj, false);
                list.Add(obj);
            }

            this.FindAllBySqlAfter();
            return list;
        }

        /// <summary>
        /// Delete方法前执行
        /// </summary>
        protected virtual void DeleteBeforce() { }
        /// <summary>
        /// Delete方法前执行
        /// </summary>
        protected virtual void DeleteAfter() { }

        /// <summary>
        /// 删除该数据
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            this.DeleteBeforce();

            bool result = false;
            if (!Convert.ToBoolean(this.GetType().GetField("IsNewData").GetValue(this)))
            {
                int id = 0;
                Hashtable fields = new Hashtable();
                foreach (System.Reflection.PropertyInfo item in this.GetType().GetProperties())
                {
                    foreach (System.Attribute attr in item.GetCustomAttributes(true))
                    {
                        if (attr is PrimaryKey)
                        {
                            id = Convert.ToInt32(item.GetValue(this, null));
                            break;
                        }
                    }
                }
                Criteria criteria = new Criteria();
                criteria.Where = primary_key + " = " + id.ToString();
                string sql = OZGNet.Data.SqlText.Delete(tableName, criteria.Where);
                result = db.ExecuteNonQuery(sql) > 0 ? true : false;
            }
            this.DeleteAfter();
            return result;
        }

        /// <summary>
        /// DeleteAll方法前执行
        /// </summary>
        protected virtual void DeleteAllBeforce() { }
        /// <summary>
        /// DeleteAll方法前执行
        /// </summary>
        protected virtual void DeleteAllAfter() { }

        /// <summary>
        /// 以where条件删除数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public int DeleteAll(Criteria criteria)
        {
            this.DeleteAllBeforce();

            string sql = OZGNet.Data.SqlText.Delete(tableName, criteria.Where);
            int result =  db.ExecuteNonQuery(sql);

            this.DeleteAllAfter();
            return result;
        }

        /// <summary>
        /// Update方法前执行
        /// </summary>
        protected virtual void UpdateBeforce() { }
        /// <summary>
        /// Update方法前执行
        /// </summary>
        protected virtual void UpdateAfter() { }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            this.UpdateBeforce();

            bool result = false;
            if (!Convert.ToBoolean(this.GetType().GetField("IsNewData").GetValue(this)))
            {
                int id = 0;
                Hashtable fields = new Hashtable();
                foreach (System.Reflection.PropertyInfo item in this.GetType().GetProperties())
                {
                    foreach (System.Attribute attr in item.GetCustomAttributes(true))
                    {
                        if (attr is Property)
                        {
                            fields.Add((attr as Property).GetFieldName(), item.GetValue(this, null));
                        }
                        else if (attr is PrimaryKey)
                        {
                            id = Convert.ToInt32(item.GetValue(this, null));
                        }
                    }
                }
                Criteria criteria = new Criteria();
                criteria.Where = primary_key + " = " + id.ToString();
                string sql = OZGNet.Data.SqlText.Update(fields, tableName, criteria.Where);
                result = db.ExecuteNonQuery(sql) > 0 ? true : false;
            }            

            this.UpdateAfter();
            return result;
        }

        /// <summary>
        /// UpdateAll方法前执行
        /// </summary>
        protected virtual void UpdateAllBeforce() { }
        /// <summary>
        /// UpdateAll方法前执行
        /// </summary>
        protected virtual void UpdateAllAfter() { }

        /// <summary>
        /// 以where条件更新数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public int UpdateAll(Criteria criteria)
        {
            this.UpdateAllBeforce();            

            Hashtable fields = new Hashtable();
            foreach (System.Reflection.PropertyInfo item in this.GetType().GetProperties())
            {
                object val = item.GetValue(this, null);
                if (val != null)
                {
                    foreach (System.Attribute attr in item.GetCustomAttributes(true))
                    {
                        if (attr is Property)
                        {
                            fields.Add((attr as Property).GetFieldName(), val);
                        }
                    }
                }
            }            

            string sql = OZGNet.Data.SqlText.Update(fields, tableName, criteria.Where);
            int result = db.ExecuteNonQuery(sql); ;

            this.UpdateAllAfter();
            return result;
        }

        /// <summary>
        /// Query方法前执行
        /// </summary>
        protected virtual void QueryBeforce() { }
        /// <summary>
        /// Query方法前执行
        /// </summary>
        protected virtual void QueryAfter() { }

        public DataTable QueryDataTable(string sql)
        {
            this.QueryBeforce();
            DataTable dt = db.DataTable(sql);
            this.QueryAfter();
            return dt;
        }

        public DataRow QueryDataRow(string sql)
        {
            this.QueryBeforce();
            DataRow dr = db.SingleRow(sql);
            this.QueryAfter();
            return dr;
        }

        public object QueryScalar(string sql)
        {
            this.QueryBeforce();
            object result = db.ExecuteScalar(sql);
            this.QueryAfter();
            return result;
        }

        /// <summary>
        /// 这个方法不可继承
        /// </summary>
        /// <param name="full_class_name"></param>
        /// <returns></returns>
        private KeyValuePair<string, string> GetPathAndClass(string full_class_name)
        {
            return GetPathAndClassStatic(full_class_name);
        }

        /// <summary>
        /// 这个方法不可继承
        /// </summary>
        /// <param name="full_class_name"></param>
        /// <returns></returns>
        private static KeyValuePair<string, string> GetPathAndClassStatic(string full_class_name)
        {
            string[] str_split = full_class_name.Split('.');
            string path = "";
            string className = "";
            for (int i = 0; i < str_split.Length; i++)
            {
                if (i + 1 < str_split.Length)
                {
                    path += str_split[i];
                    path += ".";
                }
                else
                {
                    path = path.Substring(0, path.Length - 1);
                    className = str_split[i];
                }
            }

            if (str_split.Length == 1)
            {
                path = "OZGNet.Data";
            }

            KeyValuePair<string, string> result = new KeyValuePair<string, string>(path, className);
            return result;
        }

        /// <summary>
        /// 这个方法不可继承
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private string ReplaceSqlString(string content)
        {
            if (content == null)
            {
                return null;
            }
            else
            {
                content = content.Replace("'", "’");

                return content;
            }
        } 

    }
}

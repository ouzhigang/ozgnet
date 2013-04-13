using System;
using System.Collections.Generic;
using System.Text;

namespace OZGNet.Data.AR
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class ActiveRecord : System.Attribute
    {
        private string tableName;

        public ActiveRecord()
        { 
        
        }

        public ActiveRecord(string tableName)
        {
            this.tableName = tableName;
        }

        ~ActiveRecord()
        {

        }

        public string TableName()
        { 
            return this.tableName;
        }

    }
}

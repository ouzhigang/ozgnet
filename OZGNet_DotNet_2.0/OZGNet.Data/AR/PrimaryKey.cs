using System;
using System.Collections.Generic;
using System.Text;

namespace OZGNet.Data.AR
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class PrimaryKey : System.Attribute
    {
        string primaryKey;

        public PrimaryKey()
        { 
        
        }

        public PrimaryKey(string primaryKey)
        {
            this.primaryKey = primaryKey;
        }

        ~PrimaryKey()
        {

        }

        public string GetPrimaryKey()
        {
            return this.primaryKey;
        }

    }
}

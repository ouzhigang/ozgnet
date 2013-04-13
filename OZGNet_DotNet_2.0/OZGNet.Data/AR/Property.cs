using System;
using System.Collections.Generic;
using System.Text;

namespace OZGNet.Data.AR
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class Property : System.Attribute
    {
        string fieldName;

        public Property()
        { 
        
        }

        public Property(string fieldName)
        {
            this.fieldName = fieldName;
        }

        ~Property()
        {

        }

        public string GetFieldName()
        {
            return this.fieldName;
        }

    }
}

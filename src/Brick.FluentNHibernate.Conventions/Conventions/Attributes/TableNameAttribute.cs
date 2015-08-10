using System;

namespace Brick.FluentNHibernate.Conventions.Conventions.Attributes
{
    public class TableNameAttribute : Attribute
    {
        public TableNameAttribute(string name)
        {
            TableName = name;
        }

        public string TableName { get; set; }
    }
}
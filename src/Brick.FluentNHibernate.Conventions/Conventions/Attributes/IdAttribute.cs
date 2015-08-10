using System;

namespace Brick.FluentNHibernate.Conventions.Conventions.Attributes
{
    public class IdAttribute : Attribute
    {
        public IdAttribute()
        {
            ColumnName = "Id";
        }

        public string ColumnName { get; set; }
    }
}
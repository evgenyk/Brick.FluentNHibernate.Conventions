using System;
using FluentNHibernate;

namespace Brick.FluentNHibernate.Conventions.Conventions
{
    public class ForeignKeyConvention : global::FluentNHibernate.Conventions.ForeignKeyConvention
    {
        protected override string GetKeyName(Member property, Type type)
        {
            return property == null ? string.Format("{0}Id", type.Name) : string.Format("{0}Id", property.Name);
        }
    }
}
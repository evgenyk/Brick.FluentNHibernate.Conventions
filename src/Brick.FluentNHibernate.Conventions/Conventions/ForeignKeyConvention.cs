using System;
using FluentNHibernate;

namespace Brick.FluentNHibernate.Conventions.Conventions
{
    public class ForeignKeyConvention : global::FluentNHibernate.Conventions.ForeignKeyConvention
    {
        protected override string GetKeyName(Member property, Type type)
        {
            if (property == null)
                return type.Name + "Fk";

            return property.Name + "Fk";
        }
    }
}
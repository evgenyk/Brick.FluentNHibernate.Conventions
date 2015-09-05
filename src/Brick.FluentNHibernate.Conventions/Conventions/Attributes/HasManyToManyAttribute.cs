using System;

namespace Brick.FluentNHibernate.Conventions.Conventions.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class HasManyToManyAttribute : Attribute
    {
    }
}
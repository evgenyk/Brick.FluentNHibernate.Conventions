using System;

namespace Brick.FluentNHibernate.Conventions.Conventions.Attributes
{
    /// <summary>
    /// This is not finished yet. To degree, re-model your stuff to do one-to-many
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    [Obsolete]
    public class HasManyToManyAttribute : Attribute
    {
    }
}
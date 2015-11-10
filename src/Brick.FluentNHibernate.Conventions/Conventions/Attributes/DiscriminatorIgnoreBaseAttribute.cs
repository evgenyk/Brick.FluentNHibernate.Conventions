using System;

namespace Brick.FluentNHibernate.Conventions.Conventions.Attributes
{
    /// <summary>
    /// Mark a base abstract class with this if you want a table per hierarchy mapping
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DiscriminatorIgnoreBaseAttribute : Attribute
    {
    }
}
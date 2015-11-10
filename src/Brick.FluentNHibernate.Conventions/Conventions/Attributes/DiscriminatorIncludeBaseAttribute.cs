using System;

namespace Brick.FluentNHibernate.Conventions.Conventions.Attributes
{
    /// <summary>
    /// Mark a base abstract class with this if you want a single table per hierarchy mapping
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DiscriminatorIncludeBaseAttribute : Attribute
    {
    }
}
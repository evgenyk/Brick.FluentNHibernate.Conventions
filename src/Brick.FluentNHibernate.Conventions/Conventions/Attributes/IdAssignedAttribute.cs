using System;

namespace Brick.FluentNHibernate.Conventions.Conventions.Attributes
{
    /// <summary>
    /// Indicates that primary is not going to be generated with a generator, but rather client app would assign it manually.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class IdAssignedAttribute : Attribute
    {
        
    }
}
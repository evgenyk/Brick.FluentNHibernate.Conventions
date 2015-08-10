using System;

namespace Brick.FluentNHibernate.Conventions.Conventions.Attributes
{
    public class DiscriminatorValueAttribute : Attribute
    {
        public DiscriminatorValueAttribute(string name)
        {
            Name = name.ToLowerInvariant();
        }

        public string Name { get; set; }
    }
}
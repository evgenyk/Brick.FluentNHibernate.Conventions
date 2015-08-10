using System.Reflection;
using Brick.FluentNHibernate.Conventions.Conventions.Attributes;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Brick.FluentNHibernate.Conventions.Conventions
{
    public class DiscriminatedSubclassNamingConvention : ISubclassConvention
    {
        public void Apply(ISubclassInstance instance)
        {
            var attr = instance.Type.GetCustomAttribute<DiscriminatorValueAttribute>();
            instance.DiscriminatorValue(attr != null ? attr.Name : instance.Type.FullName.ToLowerInvariant());
        }
    }
}
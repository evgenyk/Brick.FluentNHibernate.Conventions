using System.Reflection;
using Brick.FluentNHibernate.Conventions.Conventions.Attributes;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Brick.FluentNHibernate.Conventions.Conventions
{
    public class OneToManyConvention : IHasManyConvention
    {
        public void Apply(IOneToManyCollectionInstance instance)
        {
            var attr = instance.EntityType.GetCustomAttribute<TableNameAttribute>();

            instance.Key.Column((attr != null ? attr.TableName : instance.EntityType.Name) + "Id");

            instance.Inverse();
        }
    }
}
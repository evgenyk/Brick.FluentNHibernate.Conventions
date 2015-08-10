using System.Reflection;
using Brick.FluentNHibernate.Conventions.Conventions.Attributes;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using Pluralize;

namespace Brick.FluentNHibernate.Conventions.Conventions
{
    public class TableNameConvention : IClassConvention
    {
        private static readonly Pluralizer _pluralizer = new Pluralizer();

        public void Apply(IClassInstance instance)
        {
            var attr = instance.EntityType.GetCustomAttribute<TableNameAttribute>();
            var tableName = _pluralizer.Pluralize("{" + instance.EntityType.Name + "}", 2);
            instance.Table(attr != null ? attr.TableName : tableName);
        }
    }
}
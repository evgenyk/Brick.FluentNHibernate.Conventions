using System.Reflection;
using Brick.FluentNHibernate.Conventions.Conventions.Attributes;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Brick.FluentNHibernate.Conventions.Conventions
{
    public class ManyToOneConvention : IReferenceConvention
    {
        public void Apply(IManyToOneInstance instance)
        {
            var attr = instance.Property.PropertyType.GetCustomAttribute<TableNameAttribute>();

            if (instance.Property.MemberInfo.IsDefined(typeof (NotNullAttribute), false))
                instance.Not.Nullable();

            var columnName = string.Format("{0}Id", (attr != null ? attr.TableName : instance.Property.PropertyType.Name));
            instance.Column(columnName);
        }
    }
}
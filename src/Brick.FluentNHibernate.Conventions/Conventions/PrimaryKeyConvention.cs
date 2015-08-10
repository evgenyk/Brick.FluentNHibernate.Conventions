using System;
using System.Linq;
using System.Reflection;
using Brick.FluentNHibernate.Conventions.Conventions.Attributes;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Brick.FluentNHibernate.Conventions.Conventions
{
    public class PrimaryKeyConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            var columnName = "Id";

            var idAttribute = instance.Property.MemberInfo.GetCustomAttribute<IdAttribute>();
            if (idAttribute != null)
                columnName = idAttribute.ColumnName;

            instance.Column(columnName);
        }

        public static PropertyInfo TryFindIdProperty(Type target)
        {
            var props = target.GetProperties(BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Public);

            var idProp = props.FirstOrDefault(prop => prop.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase));
            if (idProp != null)
                return idProp;

            var idPropsWithAttributes = props.Where(x => x.GetCustomAttribute<IdAttribute>() != null).ToList();

            if (idPropsWithAttributes.Count() > 1)
                throw new Exception(string.Format("More than one IdAttribute was defined on {0}", target.GetType()));

            return idPropsWithAttributes.FirstOrDefault();
        }
    }
}
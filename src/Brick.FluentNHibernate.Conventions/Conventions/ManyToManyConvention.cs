using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Brick.FluentNHibernate.Conventions.Conventions
{
    public class ManyToManyConvention : IHasManyConvention
    {
        public void Apply(IOneToManyCollectionInstance instance)
        {
            var firstName = instance.EntityType.Name;
            var secondName = instance.ChildType.Name;

            if (StringComparer.OrdinalIgnoreCase.Compare(firstName, secondName) > 0)
            {
                instance.Table(string.Format("{0}{1}", secondName, firstName));
                instance.Inverse();
            }
            else
            {
                instance.Table(string.Format("{0}{1}", firstName, secondName));
                instance.Not.Inverse();
            }

            instance.Cascade.All();
        }
    }
}
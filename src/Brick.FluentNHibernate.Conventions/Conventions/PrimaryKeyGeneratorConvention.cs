using System;
using System.Reflection;
using Brick.FluentNHibernate.Conventions.Conventions.Attributes;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Brick.FluentNHibernate.Conventions.Conventions
{
    public class PrimaryKeyGeneratorConvention : IIdConvention
    {
        public void Apply(IIdentityInstance id)
        {
            var isAssigned = id.EntityType.GetCustomAttribute<IdAssignedAttribute>();
            if (isAssigned != null)
            {
                id.GeneratedBy.Assigned();
                return;
            }

            if (id.Type == typeof (Guid)) id.GeneratedBy.GuidComb();
            else if (id.Type == typeof (int)) id.GeneratedBy.Identity();
            else if (id.Type == typeof (long)) id.GeneratedBy.Identity();

            else if (id.Type == typeof(string)) id.GeneratedBy.Assigned();
            else id.GeneratedBy.Assigned();
        }

        public bool Accept(IIdentityInstance id)
        {
            return true;
        }
    }
}
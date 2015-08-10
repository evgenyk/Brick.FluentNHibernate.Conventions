using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Brick.FluentNHibernate.Conventions.Conventions
{
    public class PrimaryKeyGeneratorConvention : IIdConvention
    {
        public void Apply(IIdentityInstance id)
        {
            if (id.Type == typeof (Guid)) id.GeneratedBy.GuidComb();
            if (id.Type == typeof (string)) id.GeneratedBy.UuidHex("N");
            else if (id.Type == typeof (int)) id.GeneratedBy.Identity();
            else if (id.Type == typeof (long)) id.GeneratedBy.Identity();
            else id.GeneratedBy.Assigned();
        }

        public bool Accept(IIdentityInstance id)
        {
            return true;
        }
    }
}
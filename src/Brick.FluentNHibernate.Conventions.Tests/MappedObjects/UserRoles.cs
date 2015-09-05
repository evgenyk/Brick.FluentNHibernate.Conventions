using System;
using System.Collections.Generic;
using Brick.FluentNHibernate.Conventions.Conventions.Attributes;

namespace Brick.FluentNHibernate.Conventions.Tests.MappedObjects
{
    public class UserRoles : Identity<Guid>
    {
        public virtual User User { get; set; }
        [HasManyToMany]
        public virtual IList<Role> Roles { get; set; }
    }
}
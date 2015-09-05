using System;
using System.Collections.Generic;

namespace Brick.FluentNHibernate.Conventions.Tests.MappedObjects
{
    public class User: Identity<Guid>
    {
        public virtual Account Account { get; set; }
        public virtual List<UserRoles> UserRoleses { get; set; }
    }
}
using System;
using System.Collections.Generic;
using Brick.FluentNHibernate.Conventions.Tests.InterfaceBasedMapping.NameSpace1;

namespace Brick.FluentNHibernate.Conventions.Tests.InterfaceBasedMapping.NameSpace2
{
    public class User : Identity<Guid>
    {
        public virtual Account Account { get; set; }
        public virtual List<UserRoles> UserRoleses { get; set; }
    }
}
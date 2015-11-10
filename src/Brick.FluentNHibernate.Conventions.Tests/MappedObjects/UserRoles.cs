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



    [DiscriminatorIncludeBase]
    [TableName("FancyBlahs")]
    public abstract class BaseBlah : Identity<Guid>
    {
        protected BaseBlah()
        {
        }
    }

    [DiscriminatorValue("blah1")]
    public class Blah1 : BaseBlah
    {
        protected Blah1()
        {
        }
    }

    [DiscriminatorValue("blah2")]
    public class Blah2 : BaseBlah
    {
        protected Blah2()
        {
        }
    }
}
using System;

namespace Brick.FluentNHibernate.Conventions.Tests.MappedObjects
{
    public class Image: Identity<Guid>
    {
        public virtual Account Account { get; set; }
        public virtual User Owner { get; set; }
    }
}
using System;
using Brick.FluentNHibernate.Conventions.Tests.InterfaceBasedMapping.NameSpace1;

namespace Brick.FluentNHibernate.Conventions.Tests.InterfaceBasedMapping.NameSpace2
{
    public class Image : Identity<Guid>, IMapThisNamespaceMarkerInterface
    {
        public virtual Account Account { get; set; }
        public virtual User Owner { get; set; }
    }
}
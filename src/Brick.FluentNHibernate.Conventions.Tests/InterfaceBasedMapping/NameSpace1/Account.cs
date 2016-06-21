using System;
using Brick.FluentNHibernate.Conventions.Conventions.Attributes;

namespace Brick.FluentNHibernate.Conventions.Tests.InterfaceBasedMapping.NameSpace1
{
    [IdAssigned]
    public class Account : Identity<Guid>, IMapThisNamespaceMarkerInterface
    {
    }
}
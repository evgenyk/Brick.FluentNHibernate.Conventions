using System;
using Brick.FluentNHibernate.Conventions.Conventions.Attributes;

namespace Brick.FluentNHibernate.Conventions.Tests.MappedObjects
{
    [IdAssigned]
    public class Account : Identity<Guid>
    {
    }
}
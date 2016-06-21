using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Automapping;

namespace Brick.FluentNHibernate.Conventions.Fluent
{
    public interface IBrickFluentConfiguration : IAutomappingConfiguration
    {
        IEnumerable<Assembly> AssembliesToScan { get; }
    }
}
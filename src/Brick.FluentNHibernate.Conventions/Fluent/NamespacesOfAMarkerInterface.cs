using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Brick.FluentNHibernate.Conventions.Fluent
{
    public class NamespacesOfAMarkerInterface<T, TStartFromAssemblyOf> : FluentConfigurationBase, IBrickFluentConfiguration where TStartFromAssemblyOf: class
    {
        private readonly IList<Assembly> _assembliesToScan;
        private readonly List<string> _namespacesToMap;

        public NamespacesOfAMarkerInterface()
        {
            var entryAssembly = typeof(TStartFromAssemblyOf).Assembly;
            var assemblies = entryAssembly.ThisIncludingReferences();
            var markedTypes = assemblies.LoadedAssemblies.SelectMany(asm => asm.GetAllImplementationsOf<T>()).ToList();
            var assembliesToScan = markedTypes.Select(type => type.Assembly).Distinct();

            _assembliesToScan = assembliesToScan.ToList();
            _namespacesToMap = markedTypes.Select(x => x.Namespace).Distinct().ToList();
        }

        public IEnumerable<Assembly> AssembliesToScan => _assembliesToScan;

        public override bool ShouldMap(Type type)
        {
            var shouldMap = type.Namespace != null && _namespacesToMap.Contains(type.Namespace, StringComparer.InvariantCultureIgnoreCase);
            return shouldMap;
        }
    }
}
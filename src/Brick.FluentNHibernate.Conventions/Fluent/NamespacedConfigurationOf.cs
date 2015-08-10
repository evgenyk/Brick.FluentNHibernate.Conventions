using System;

namespace Brick.FluentNHibernate.Conventions.Fluent
{
    public class NamespacedConfigurationOf<T> : FluentConfigurationBase
    {
        private readonly string _ns;

        public NamespacedConfigurationOf()
        {
            _ns = typeof (T).Namespace;
        }

        public override bool ShouldMap(Type type)
        {
            var shouldMap = type.Namespace != null &&
                            type.Namespace.StartsWith(_ns, StringComparison.InvariantCultureIgnoreCase);
            return shouldMap;
        }
    }
}
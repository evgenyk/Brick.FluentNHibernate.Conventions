using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Brick.FluentNHibernate.Conventions.Conventions;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.Conventions;

namespace Brick.FluentNHibernate.Conventions.Fluent
{
    public class NamespacedConfigurationOf<T> : FluentConfigurationBase, IBrickFluentConfiguration
    {
        private readonly string _ns;

        public NamespacedConfigurationOf()
        {
            _ns = typeof (T).Namespace;
        }

        public override IEnumerable<IAutomappingStep> GetMappingSteps(AutoMapper mapper, IConventionFinder conventionFinder)
        {
            return base.GetMappingSteps(mapper, conventionFinder).Select(GetDecoratedStep);
        }

        IAutomappingStep GetDecoratedStep(IAutomappingStep step)
        {
            if (step is HasManyToManyStep)
            {
                return new ExplicitHasManyToManyStep(this, step);
            }

            return step;
        }

        public override bool ShouldMap(Type type)
        {
            var shouldMap = type.Namespace != null &&
                            type.Namespace.StartsWith(_ns, StringComparison.InvariantCultureIgnoreCase);
            return shouldMap;
        }

        public IEnumerable<Assembly> AssembliesToScan
        {
            get { yield return typeof(T).Assembly; }
        }
    }

}
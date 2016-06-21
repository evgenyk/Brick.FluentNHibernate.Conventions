using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Brick.FluentNHibernate.Conventions.Fluent
{
    internal static class AssemblyExtensions
    {
        public static Type[] GetAllImplementationsOf<TInterface>(this Assembly assembly)
        {
            var result = assembly.GetTypes().GetAllImplementationsOf<TInterface>();
            return result;
        }

        public static Type[] GetAllImplementationsOf<TInterface>(this IEnumerable<Type> types)
        {
            var result = types.Where(x => x.GetInterfaces().Contains(typeof(TInterface))).Distinct().ToArray();
            return result;
        }

        public static Type[] ExcludingOpenGeneric(this IEnumerable<Type> types)
        {
            var excludingOpenGeneric = types.Where(type => type.IsGenericType && type.GenericTypeArguments.Any() || !type.IsGenericType).ToArray();
            return excludingOpenGeneric;
        }

        public static Type[] GetAllGenericImplementationsOf(this IEnumerable<Type> types, Type genericInterfaceType)
        {
            var result = types.Where(type => type.GetInterfaces().Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericInterfaceType)).Distinct().ToArray();
            return result;
        }

        public static AssemblyEnumerationResult ThisIncludingReferences(this Assembly assembly)
        {
            var result = assembly.References();
            result.AddSuccess(new[] { assembly });
            return result;
        }

        public static AssemblyEnumerationResult References(this Assembly assembly)
        {
            var result = new AssemblyEnumerationResult();
            Func<AssemblyName, Assembly> selector = name =>
            {
                try
                {
                    if (assembly.FullName.StartsWith("System.") || assembly.FullName.StartsWith("mscorlib"))
                        return null;

                    var loadedAssembly = Assembly.Load(name);
                    return loadedAssembly;
                }
                catch (Exception e)
                {
                    result.AddException(name, e);
                }
                return null;
            };

            var ret = selector;
            var referencedAssemblies = assembly.GetReferencedAssemblies().ToList();
            var assemblies = new List<Assembly>().Concat(referencedAssemblies.Select(ret).Where(x => x != null)).Distinct().ToList();
            result.AddSuccess(assemblies);

            foreach (var assemblyWithAttribute in assemblies)
            {
                var recursed = assemblyWithAttribute.References();
                result.AddAll(recursed);
            }

            return result;
        }

        public class AssemblyEnumerationResult
        {
            private readonly List<AssemblyLoadFailure> _failedAssemblies;

            private readonly List<Assembly> _loadedAssemblies;

            public AssemblyEnumerationResult()
            {
                _loadedAssemblies = new List<Assembly>();
                _failedAssemblies = new List<AssemblyLoadFailure>();
            }

            private AssemblyEnumerationResult(AssemblyEnumerationResult first, AssemblyEnumerationResult second) : this()
            {
                AddSuccess(first.LoadedAssemblies);
                AddExceptions(first.FailedAssemblies);
                AddSuccess(second.LoadedAssemblies);
                AddExceptions(second.FailedAssemblies);
            }

            public IEnumerable<Assembly> LoadedAssemblies => _loadedAssemblies.Distinct();

            public IEnumerable<AssemblyLoadFailure> FailedAssemblies => _failedAssemblies;

            private void AddExceptions(IEnumerable<AssemblyLoadFailure> failed)
            {
                _failedAssemblies.AddRange(failed);
            }


            public void AddException(AssemblyName name, Exception exception)
            {
                _failedAssemblies.Add(new AssemblyLoadFailure(name, exception));
            }

            public void AddSuccess(IEnumerable<Assembly> asms)
            {
                _loadedAssemblies.AddRange(asms);
            }

            public void AddAll(AssemblyEnumerationResult other)
            {
                _loadedAssemblies.AddRange(other.LoadedAssemblies);
                _failedAssemblies.AddRange(other.FailedAssemblies);
            }

            public AssemblyEnumerationResult CombineWith(AssemblyEnumerationResult assemblyEnumerationResult)
            {
                return new AssemblyEnumerationResult(this, assemblyEnumerationResult);
            }

            public class AssemblyLoadFailure
            {
                public AssemblyLoadFailure(AssemblyName name, Exception exception)
                {
                    Name = name;
                    Exception = exception;
                }

                public AssemblyName Name { get; private set; }
                public Exception Exception { get; private set; }
            }
        }


    }
}
using System;
using System.Linq;
using System.Reflection;
using Brick.FluentNHibernate.Conventions.Conventions;
using Brick.FluentNHibernate.Conventions.Conventions.Attributes;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Helpers;
using NHibernate.Driver;
using NHibernate.Util;

namespace Brick.FluentNHibernate.Conventions.Fluent
{
    public static class FluentConfiguation
    {
        public static FluentConfiguration CreateBasedOn<T>(string connectionString,
            DatabaseDialect dialect = DatabaseDialect.MsSql2012) where T : class
        {
            var configuration = Fluently.Configure();

            switch (dialect)
            {
                case DatabaseDialect.MsSql2012:
                    configuration.Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString)
                        .Driver<SqlClientDriver>().ShowSql().FormatSql());
                    break;
                case DatabaseDialect.Sqlite:
                    configuration.Database(SQLiteConfiguration.Standard.ConnectionString(connectionString)
                        .Driver<SQLite20Driver>().ShowSql().FormatSql());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dialect), dialect, null);
            }

            configuration.Mappings(m => m.FluentMappings.AddFromAssemblyOf<T>()).Mappings(m =>
            {
                var namespacedConfigurationOf = new NamespacedConfigurationOf<T>();
                var mappedAssembly = typeof (T).Assembly;

                var model = AutoMap.Assemblies(namespacedConfigurationOf, mappedAssembly);

                var autoPersistenceModel = model
                    .IgnoreBase(typeof (Identity<>))
                    .IgnoreBase(typeof (Identity)
                    );

                var typesToMap = mappedAssembly.GetTypes()
                    .Where(type => namespacedConfigurationOf.ShouldMap(type)).ToList();

                typesToMap
                    .Where(x => x.GetCustomAttributes<DiscriminatorIncludeBaseAttribute>(false).Any())
                    .ForEach(type => model.IncludeBase(type));

                typesToMap
                    .Where(x => x.GetCustomAttributes<DiscriminatorIgnoreBaseAttribute>(false).Any())
                    .ForEach(type => model.IgnoreBase(type));

                var persistenceModel = autoPersistenceModel.Conventions.Add(
                    new TableNameConvention(),
                    new OneToManyConvention(),
                    new PrimaryKeyGeneratorConvention(),
                    new ManyToOneConvention(),
                    new DiscriminatedSubclassNamingConvention(),
                    new PrimaryKeyConvention(),
                    new ColumnNullConvention(),
                    DefaultCascade.All(),
                    DefaultLazy.Always(),
                    DynamicInsert.AlwaysTrue(),
                    DynamicUpdate.AlwaysTrue()
                    );

                m.AutoMappings.Add(persistenceModel);
            });
            return configuration;
        }
    }
}
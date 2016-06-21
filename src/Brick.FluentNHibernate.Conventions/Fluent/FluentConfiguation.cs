using System;
using System.Collections.Generic;
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
        public static FluentConfiguration CreateBasedOn<TConfiguration>(string connectionString,
            DatabaseDialect dialect = DatabaseDialect.MsSql2012, IEnumerable<IConvention> additionalConventions = null) 
            where TConfiguration: IBrickFluentConfiguration, new()
        {
            if (additionalConventions == null)
                additionalConventions = new IConvention[] {};
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

            var config = new TConfiguration();

            var fluentConfiguration = configuration.Mappings(m =>
            {
                foreach (var assembly in config.AssembliesToScan)
                    m.FluentMappings.AddFromAssembly(assembly);
            });

            fluentConfiguration.Mappings(m =>
            {

                var model = AutoMap.Assemblies(config, config.AssembliesToScan);

                var autoPersistenceModel = model
                    .IgnoreBase(typeof (Identity<>))
                    .IgnoreBase(typeof (Identity)
                    );

                var typesToMap = config.AssembliesToScan.SelectMany(asm => asm.GetTypes())
                    .Where(type => config.ShouldMap(type)).ToList();

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

                foreach (var additionalConvention in additionalConventions)
                    autoPersistenceModel.Conventions.Add(additionalConvention);

                m.AutoMappings.Add(persistenceModel);
            });
            return configuration;
        }
    }
}
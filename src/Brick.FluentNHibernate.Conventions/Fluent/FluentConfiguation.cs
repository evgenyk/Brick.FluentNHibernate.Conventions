using Brick.FluentNHibernate.Conventions.Conventions;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate.Driver;

namespace Brick.FluentNHibernate.Conventions.Fluent
{
    public static class FluentConfiguation
    {
        public static FluentConfiguration CreateBasedOn<T>(string connectionString) where T : class
        {
            var configuration = Fluently.Configure();
            configuration
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString)
                    .Driver<SqlClientDriver>().ShowSql().FormatSql())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<T>())
                .Mappings(m =>
                {
                    var autoPersistenceModel = AutoMap.Assemblies(new NamespacedConfigurationOf<T>(),
                        typeof (T).Assembly)
                        .IgnoreBase(typeof (Identity<>))
                        .IgnoreBase(typeof (Identity))
                        ;

                    var persistenceModel = autoPersistenceModel.Conventions.Add(
                        new TableNameConvention(),
                        new OneToManyConvention(),
                        new PrimaryKeyGeneratorConvention(),
                        new ManyToOneConvention(),
                        new DiscriminatedSubclassNamingConvention(),
                        new PrimaryKeyConvention(),
                        new ColumnNullConvention(),
                        new ManyToManyConvention(),
                        new ForeignKeyConvention(),
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
# Brick.FluentNHibernate.Conventions

### A non-discriptive sample omitting details
```
// maps all classes in the namespace of `<T>` and applies default conventions
var configuration = FluentConfiguation.CreateBasedOn<T>(sql.ToConnectionString());

//builds NHibernate session factory
var sessionFactory = configuration.BuildSessionFactory();

//Opens an NH session to do whatever NHibernate provides
var session = sessionFactory.OpenSession();
```


### A bit of automatic schema migrations

When the first `ISession` is requested, the `schemaUpdate` (you need to create this one) object would contain all scripts you could run to get database schema up to speed.
```
var configuration = FluentConfiguation.CreateBasedOn<T>(sql.ToConnectionString());

configuration.ExposeConfiguration(x =>
            {
                var schemaUpdate = new SchemaUpdate(x);
                schemaUpdate.Execute(schemaChanges.AddScript, false);
            });

var sessionFactory = configuration.BuildSessionFactory();

scriptRunner.MigrateDatabase(schemaChanges); //you need to implement ScriptRunner yourself, if you like more control

var session = sessionFactory.OpenSession();
```

### IoC container registration

Generally looks like this in a hypothetical IoC container.
`SessionFactory` is singleton and `ISession` is scopped per unit of work .
```
container.ConfigureFactory(() => CreateSessionFactory<MyDomainObject>(), ComponentLifecycle.Singleton);

container.ConfigureFactory(() => container.Resolve<ISessionFactory>().OpenSession(), ComponentLifecycle.Scoped);

```
After all that dancing, you can depend on `ISession` anywhere in your app.

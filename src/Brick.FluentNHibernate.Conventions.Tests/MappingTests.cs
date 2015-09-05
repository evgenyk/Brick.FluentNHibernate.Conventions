﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brick.FluentNHibernate.Conventions.Fluent;
using Brick.FluentNHibernate.Conventions.Tests.MappedObjects;
using NHibernate.Tool.hbm2ddl;
using Should;
using Xunit;

namespace Brick.FluentNHibernate.Conventions.Tests
{
    public class MappingTests
    {
        [Fact]
        public void TestMapping()
        {
            var configuration = FluentConfiguation.CreateBasedOn<Account>("Data Source=:memory:;Version=3;New=True;", DatabaseDialect.Sqlite);
            List<string> individualStrings = new List<string>();

            configuration.ExposeConfiguration(x =>
            {
                var schemaUpdate = new SchemaUpdate(x);
                schemaUpdate.Execute(s =>
                {
                    individualStrings.Add(s);
                }, false);
            });

            var sessionFactory = configuration.BuildSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {

                individualStrings.ForEach(script =>
                {
                    var command = session.Connection.CreateCommand();
                    command.CommandText = script;
                    command.ExecuteNonQuery();
                });

                using (var transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(new Account());
                    session.Flush();
                    var accounts = session.QueryOver<Account>().List();
                    accounts.Count().ShouldBeGreaterThan(0);
                    transaction.Commit();
                }

            }

        }
    }
}
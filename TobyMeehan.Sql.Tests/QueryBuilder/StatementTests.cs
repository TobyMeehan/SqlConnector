﻿using Autofac.Extras.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobyMeehan.Sql.QueryBuilder;
using Xunit;

namespace TobyMeehan.Sql.Tests.QueryBuilder
{
    public class StatementTests
    {
        [Fact]
        public void Select()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDbConnection>();

                var factory = mock.Create<QueryFactory>();
                var query = factory.Query<EntityModel>()
                    .Select();

                string actual = query.AsSql();

                Assert.Equal("SELECT * FROM entities", actual);
            }
        }

        [Fact]
        public void Insert()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDbConnection>();

                var factory = mock.Create<QueryFactory>();
                var query = factory.Query<EntityModel>()
                    .Insert(new
                    {
                        Id = new SqlString("UUID()"),
                        Title = "Foo",
                        Description = "Bar"
                    });

                string actual = query.AsSql(out Dictionary<string, object> param);

                Assert.Equal("INSERT INTO entities (Id, Title, Description) VALUES (UUID(),@1,@2)", actual);

                Assert.Equal("Foo", param["1"]);
                Assert.Equal("Bar", param["2"]);
            }
        }

        [Fact]
        public void Update()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDbConnection>();

                var factory = mock.Create<QueryFactory>();
                var query = factory.Query<EntityModel>()
                    .Update(new
                    {
                        Title = "Bar",
                        Description = "Foo"
                    });

                string actual = query.AsSql(out Dictionary<string, object> param);

                Assert.Equal("UPDATE entities SET Title = @1, Description = @2", actual);

                Assert.Equal("Bar", param["1"]);
                Assert.Equal("Foo", param["2"]);
            }
        }

        [Fact]
        public void Delete()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDbConnection>();

                var factory = mock.Create<QueryFactory>();
                var query = factory.Query<EntityModel>()
                    .Delete();

                string actual = query.AsSql();

                Assert.Equal("DELETE FROM entities", actual);
            }
        }
    }
}

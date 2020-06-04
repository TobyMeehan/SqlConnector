using Autofac.Extras.Moq;
using Dapper;
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
    public class WhereClauseTests
    {
        [Fact]
        public void ShouldGenerateSimpleClause()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var query = new SqlQuery<EntityModel>()
                    .Where(x => x.Id == "E");

                string actual = query.ToSql(out IDictionary<string, object> param);

                Assert.Equal("WHERE (entities.Id = @1)", actual);

                Assert.Equal("E", param["1"]);
            }
        }

        [Fact]
        public void ShouldGenerateComplexClause()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var query = new SqlQuery<EntityModel>()
                    .Where(x => x.Title == "Foo" && (x.Description == "Bar" || x.Description == "E"));

                string actual = query.ToSql(out IDictionary<string, object> param);

                Assert.Equal("WHERE ((entities.Title = @1) AND ((entities.Description = @2) OR (entities.Description = @3)))", actual);

                Assert.Equal("Foo", param["1"]);
                Assert.Equal("Bar", param["2"]);
                Assert.Equal("E", param["3"]);
            }
        }

        [Fact]
        public void ShouldGenerateForeignClause()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var query = new SqlQuery<EntityModel>()
                    .Where<UserModel>((e, u) => u.Id == "Id");

                string actual = query.ToSql(out IDictionary<string, object> param);

                Assert.Equal("WHERE (users.Id = @1)", actual);

                Assert.Equal("Id", param["1"]);
            }
        }

        private string GetId(EntityBase entity) => entity.Id;
        [Fact]
        public void ShouldEvaluateMethodCall()
        {
            EntityModel entity = new EntityModel { Id = "8" };

            using (var mock = AutoMock.GetLoose())
            {
                var query = new SqlQuery<EntityModel>()
                    .Where(x => x.Id == GetId(entity));

                string actual = query.ToSql(out IDictionary<string, object> param);

                Assert.Equal("WHERE (entities.Id = @1)", actual);
                Assert.Equal("8", param["1"]);
            }
        }

        [Fact]
        public void ShouldParameteriseCorrectly()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var query = new SqlQuery<EntityModel>()
                    .Where(x => x.Id == "8");

                string actual = query.ToSql(out object param);

                DynamicParameters dynamic = param as DynamicParameters;

                Assert.Equal("8", dynamic.Get<string>("1"));
            }
        }
    }
}

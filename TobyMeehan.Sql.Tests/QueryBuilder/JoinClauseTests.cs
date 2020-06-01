using Autofac.Extras.Moq;
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
    public class JoinClauseTests
    {
        [Fact]
        public void ShouldGenerateInnerJoin()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDbConnection>();

                var factory = mock.Create<QueryFactory>();
                var query = factory.Query<EntityModel>()
                    .InnerJoin<UserModel>((e, u) => u.Name == e.Title);

                string actual = query.AsSql();

                Assert.Equal("INNER JOIN users ON (users.Name = entities.Title)", actual);
            }
        }

        [Fact]
        public void ShouldGenerateLeftJoin()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDbConnection>();

                var factory = mock.Create<QueryFactory>();
                var query = factory.Query<EntityModel>()
                    .LeftJoin<UserModel>((e, u) => u.Name == e.Title);

                string actual = query.AsSql();

                Assert.Equal("LEFT JOIN users ON (users.Name = entities.Title)", actual);
            }
        }

        [Fact]
        public void ShouldGenerateRightJoin()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDbConnection>();

                var factory = mock.Create<QueryFactory>();
                var query = factory.Query<EntityModel>()
                    .RightJoin<UserModel>((e, u) => u.Name == e.Title);

                string actual = query.AsSql();

                Assert.Equal("RIGHT JOIN users ON (users.Name = entities.Title)", actual);
            }
        }

        [Fact]
        public void ShouldGenerateFullJoin()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDbConnection>();

                var factory = mock.Create<QueryFactory>();
                var query = factory.Query<EntityModel>()
                    .FullJoin<UserModel>((e, u) => u.Name == e.Title);

                string actual = query.AsSql();

                Assert.Equal("FULL OUTER JOIN users ON (users.Name = entities.Title)", actual);
            }
        }

        [Fact]
        public void ShouldGenerateComplexJoin()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDbConnection>();

                var factory = mock.Create<QueryFactory>();
                var query = factory.Query<EntityModel>()
                    .Select()
                    .LeftJoin<UserEntity>((e, ue) => e.Id == ue.EntityId)
                    .LeftJoin<UserEntity, UserModel>((ue, u) => ue.UserId == u.Id)
                    .Where(e => e.Id == "qwertyasdf");

                string actual = query.AsSql(out Dictionary<string, object> param);

                Assert.Equal("SELECT * FROM entities " +
                "LEFT JOIN userentities ON (entities.Id = userentities.EntityId) " +
                "LEFT JOIN users ON (userentities.UserId = users.Id) " +
                "WHERE (entities.Id = @1)", actual);

                Assert.Equal("qwertyasdf", param["1"]);
            }
        }
    }
}

using Autofac.Extras.Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TobyMeehan.Sql.QueryBuilder;
using Xunit;

namespace TobyMeehan.Sql.Tests.QueryBuilder
{
    public class MapTests
    {
        [Fact]
        public void ShouldGenerateQueryMapWhenMapLastCalled()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var query = new SqlQuery<EntityModel>()
                    .Select()
                    .Map<UserModel>((entity, user) =>
                    {
                        return entity;
                    });

                Assert.NotNull(query.QueryMap);
                Assert.Collection(query.QueryMap.Types, x => Assert.True(x == typeof(EntityModel)), x => Assert.True(x == typeof(UserModel)));
            }
        }

        [Fact]
        public void ShouldGenerateQueryMapWhenNotLastCalled()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var query = new SqlQuery<EntityModel>()
                    .Select()
                    .Map<UserModel>((entity, user) =>
                    {
                        return entity;
                    })
                    .Where(x => x.Id == "t");

                Assert.NotNull(query.QueryMap);
            }
        }
    }
}

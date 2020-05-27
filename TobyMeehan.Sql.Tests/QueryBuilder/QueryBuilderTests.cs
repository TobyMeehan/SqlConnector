using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TobyMeehan.Sql.QueryBuilder;
using Xunit;

namespace TobyMeehan.Sql.Tests.QueryBuilder
{
    public class QueryBuilderTests
    {
        [Theory]
        [InlineData("SELECT * FROM entities", "*")]
        [InlineData("SELECT Title, Description FROM entities", "Title, Description")]
        public void Select_ShouldGenerateSimpleSelect(string expected, params string[] columns)
        {
            string actual = new SqlQuery<EntityModel>()
                .Select(columns)
                .AsSql();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("SELECT * FROM entities WHERE (entities.Id = @1)", "7")]
        public void Select_ShouldGenerateSimpleWhereClause(string expected, string id)
        {
            string actual = new SqlQuery<EntityModel>()
                .Select()
                .Where(x => x.Id == id)
                .AsSql(out Dictionary<string, object> parameters);

            Assert.Equal(expected, actual);
            Assert.Equal(parameters["1"], id);
        }

        [Theory]
        [InlineData("SELECT * FROM entities WHERE ((entities.Title = @1) AND (entities.Description = @2))", "Title", "Description", "*")]
        public void Select_ShouldGenerateComplexWhereClause(string expected, string title, string description, params string[] columns)
        {
            string actual = new SqlQuery<EntityModel>()
                .Select(columns)
                .Where(x => x.Title == title && x.Description == description)
                .AsSql(out Dictionary<string, object> parameters);

            Assert.Equal(expected, actual);
            Assert.Equal(parameters["1"], title);
            Assert.Equal(parameters["2"], description);
        }

        [Fact]
        public void Where_ShouldNotFailWithMethodCall()
        {
            UserModel user = new UserModel { Id = "8" };

            string actual = new SqlQuery<EntityModel>()
                .Where(x => x.Id == GetId(user))
                .AsSql(out Dictionary<string, object> parameters);

            Assert.Equal("WHERE (entities.Id = @1)", actual);
            Assert.Equal("8", parameters["1"]);
        }

        private string GetId(UserModel user)
        {
            return user.Id;
        }

        [Fact]
        public void Join_ShouldGenerateJoinClause()
        {
            string actual = new SqlQuery<EntityModel>()
                .Select()
                .InnerJoin<UserModel>((e, u) => u.Name == e.Title)
                .LeftJoin<UserModel>((e, u) => u.Id == e.Description)
                .Where(e => e.Id == "1")
                .AsSql(out Dictionary<string, object> parameters);

            Assert.Equal("SELECT * FROM entities INNER JOIN users ON (users.Name = entities.Title) LEFT JOIN users ON (users.Id = entities.Description) WHERE (entities.Id = @1)", actual);
            Assert.Equal("1", parameters["1"]);
        }

        [Fact]
        public void Join_ShouldGenerateComplexJoin()
        {
            string actual = new SqlQuery<EntityModel>()
                .Select()
                .LeftJoin<UserEntity>((e, ue) => e.Id == ue.EntityId)
                .LeftJoin<UserEntity, UserModel>((ue, u) => ue.UserId == u.Id)
                .Where(e => e.Id == "qwertyasdf")
                .AsSql(out Dictionary<string, object> parameters);

            Assert.Equal("SELECT * FROM entities " +
                "LEFT JOIN userentities ON (entities.Id = userentities.EntityId) " +
                "LEFT JOIN users ON (userentities.UserId = users.Id) " +
                "WHERE (entities.Id = @1)", actual);
            Assert.Equal("qwertyasdf", parameters["1"]);
        }

        [Theory]
        [InlineData("INSERT INTO entities (Id, Title, Description) VALUES (UUID(),@1,@2)")]
        public void Insert_ShouldGenerateQuery(string expected)
        {
            string actual = new SqlQuery<EntityModel>()
                .Insert(new
                {
                    Id = new SqlString("UUID()"),
                    Title = "Foo",
                    Description = "Bar"
                })
                .AsSql(out Dictionary<string, object> parameters);

            Assert.Equal(expected, actual);
            Assert.Equal("Foo", parameters["1"]);
            Assert.Equal("Bar", parameters["2"]);
        }

        [Theory]
        [InlineData("UPDATE entities SET Title = @1, Description = @2", "Foo", "Bar")]
        public void Update_ShouldGenerateSimpleQuery(string expected, string title, string description)
        {
            string actual = new SqlQuery<EntityModel>()
                .Update(new
                {
                    Title = title,
                    Description = description
                })
                .AsSql(out Dictionary<string, object> parameters);

            Assert.Equal(expected, actual);
            Assert.Equal(title, parameters["1"]);
            Assert.Equal(description, parameters["2"]);
        }

        [Theory]
        [InlineData("UPDATE entities SET Title = @1, Description = @2 WHERE (entities.Id = @3)", "36484", "Foo", "Bar")]
        public void Update_ShouldGenerateComplexQuery(string expected, string id, string title, string description)
        {
            string actual = new SqlQuery<EntityModel>()
                .Update(new
                {
                    Title = title,
                    Description = description
                })
                .Where(x => x.Id == id)
                .AsSql(out Dictionary<string, object> parameters);

            Assert.Equal(expected, actual);
            Assert.Equal(title, parameters["1"]);
            Assert.Equal(description, parameters["2"]);
            Assert.Equal(id, parameters["3"]);
        }

        [Theory]
        [InlineData("DELETE FROM entities")]
        public void Delete_ShouldGenerateSimpleQuery(string expected)
        {
            string actual = new SqlQuery<EntityModel>()
                .Delete()
                .AsSql();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("DELETE FROM entities WHERE (entities.Id = @1)", "373")]
        public void Delete_ShouldGenerateComplexQuery(string expected, string id)
        {
            string actual = new SqlQuery<EntityModel>()
                .Delete()
                .Where(x => x.Id == id)
                .AsSql(out Dictionary<string, object> parameters);

            Assert.Equal(expected, actual);
            Assert.Equal(id, parameters["1"]);
        }
    }
}

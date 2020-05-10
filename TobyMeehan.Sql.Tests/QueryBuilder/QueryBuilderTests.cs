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
        [InlineData("SELECT * FROM Entities", "Entities", "*")]
        [InlineData("SELECT Title, Description FROM Entities", "Entities", "Title, Description")]
        public void Select_ShouldGenerateSimpleSelect(string expected, string tableName, params string[] columns)
        {
            string actual = new SqlQuery(tableName)
                .Select(columns)
                .AsSql();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("SELECT * FROM Entities WHERE (Id = @1)", "Entities", "7")]
        [InlineData("SELECT * FROM User WHERE (Id = @1)", "User", "qwerty")]
        public void Select_ShouldGenerateSimpleWhereClause(string expected, string tableName, string id)
        {
            string actual = new SqlQuery(tableName)
                .Select()
                .Where<EntityModel>(x => x.Id == id)
                .AsSql(out Dictionary<string, object> parameters);

            Assert.Equal(expected, actual);
            Assert.Equal(parameters["1"], id);
        }

        [Theory]
        [InlineData("SELECT * FROM Entities WHERE ((Title = @1) AND (Description = @2))", "Entities", "Title", "Description", "*")]
        [InlineData("SELECT Id, Title, Description FROM Things WHERE ((Title = @1) AND (Description = @2))", "Things", "Foo", "Bar", "Id", "Title", "Description")]
        public void Select_ShouldGenerateComplexWhereClause(string expected, string tableName, string title, string description, params string[] columns)
        {
            string actual = new SqlQuery(tableName)
                .Select(columns)
                .Where<EntityModel>(x => x.Title == title && x.Description == description)
                .AsSql(out Dictionary<string, object> parameters);

            Assert.Equal(expected, actual);
            Assert.Equal(parameters["1"], title);
            Assert.Equal(parameters["2"], description);
        }

        [Theory]
        [InlineData("INSERT INTO Entities (Id, Title, Description) VALUES (UUID(),@1,@2)", "Entities")]
        public void Insert_ShouldGenerateQuery(string expected, string tableName)
        {
            string actual = new SqlQuery(tableName)
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
        [InlineData("UPDATE Entities SET Title = @1, Description = @2", "Entities", "Foo", "Bar")]
        public void Update_ShouldGenerateSimpleQuery(string expected, string tableName, string title, string description)
        {
            string actual = new SqlQuery(tableName)
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
        [InlineData("UPDATE Entities SET Title = @1, Description = @2 WHERE (Id = @3)", "Entities", "36484", "Foo", "Bar")]
        public void Update_ShouldGenerateComplexQuery(string expected, string tableName, string id, string title, string description)
        {
            string actual = new SqlQuery(tableName)
                .Update(new
                {
                    Title = title,
                    Description = description
                })
                .Where<EntityModel>(x => x.Id == id)
                .AsSql(out Dictionary<string, object> parameters);

            Assert.Equal(expected, actual);
            Assert.Equal(title, parameters["1"]);
            Assert.Equal(description, parameters["2"]);
            Assert.Equal(id, parameters["3"]);
        }

        [Theory]
        [InlineData("DELETE FROM Entities", "Entities")]
        public void Delete_ShouldGenerateSimpleQuery(string expected, string tableName)
        {
            string actual = new SqlQuery(tableName)
                .Delete()
                .AsSql();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("DELETE FROM Entities WHERE (Id = @1)", "Entities", "373")]
        public void Delete_ShouldGenerateComplexQuery(string expected, string tableName, string id)
        {
            string actual = new SqlQuery(tableName)
                .Delete()
                .Where<EntityModel>(x => x.Id == id)
                .AsSql(out Dictionary<string, object> parameters);

            Assert.Equal(expected, actual);
            Assert.Equal(id, parameters["1"]);
        }
    }
}

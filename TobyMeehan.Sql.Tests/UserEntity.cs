using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobyMeehan.Sql.QueryBuilder;

namespace TobyMeehan.Sql.Tests
{
    [SqlName("userentities")]
    public class UserEntity
    {
        public string UserId { get; set; }
        public string EntityId { get; set; }

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobyMeehan.Sql.QueryBuilder;

namespace TobyMeehan.Sql.Tests
{
    [SqlName("users")]
    public class UserModel : EntityBase
    {
        public string Name { get; set; }

    }
}

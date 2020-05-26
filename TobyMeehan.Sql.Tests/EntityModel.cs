﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobyMeehan.Sql.QueryBuilder;

namespace TobyMeehan.Sql.Tests
{
    [SqlName("entities")]
    public class EntityModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

    }
}

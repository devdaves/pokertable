﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokerTable.Web.Models.JsonModels
{
    public class CreateTableJson : JsonBase
    {
        public Guid TableId { get; set; }
    }
}
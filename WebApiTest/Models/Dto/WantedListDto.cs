﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest.Models.Dto
{
    public class WantedListDto
    {
        public int Id { get; set; }
        public string NameProduct { get; set; }
        public bool BoughtStatus { get; set; }
    }
}
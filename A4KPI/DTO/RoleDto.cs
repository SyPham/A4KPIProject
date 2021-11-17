﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class RoleDto
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class ScreenFunctionAndActionRequest
    {
        public List<int> RoleIDs { get; set; }
        public string lang { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.DTOs.Marzban.Requests
{
    public class AddUserRequestDTO
    {
        public long data_limit { get; set; } = 0;
        public string data_limit_reset_strategy { get; set; } = "no_reset";
        public long expire { get; set; } = 0;
        public Inbounds inbounds { get; set; }
        public string note { get; set; } = "";
        public Proxies proxies { get; set; } = new();
        public string status { get; set; } = "active";
        public string username { get; set; }

        public class Inbounds
        {
            public string[] vless { get; set; } = [];
        }

        public class Proxies
        {
            public Vless vless { get; set; } = new();
        }

        public class Vless
        {
            public string id { get; set; } = Guid.NewGuid().ToString().ToLower();
        }
    }
}

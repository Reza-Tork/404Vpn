using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.DTOs.Marzban.Requests
{
    public class AddUserRequestDTO
    {
        [JsonPropertyName("data_limit")]
        public long Bandwidth { get; set; } = 0;
        public string data_limit_reset_strategy { get; set; } = "no_reset";

        [JsonPropertyName("expire")]
        public long ExpireTimestamp { get; set; } = 0;

        [JsonPropertyName("inbounds")]
        public Inbounds Service { get; set; }
        public object? next_plan { get; set; } = null;
        public string note { get; set; } = "";
        public int on_hold_expire_duration { get; set; } = 0;
        public DateTime on_hold_timeout { get; set; }
        public Proxies proxies { get; set; } = new();
        public string status { get; set; } = "active";
        public string username { get; set; }

        public class Inbounds
        {
            [JsonPropertyName("vless")]
            public string[] Tags { get; set; } = [];
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

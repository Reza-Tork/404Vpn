using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Marzban.Responses
{

    public class AddUserResponseDTO
    {
        public Admin admin { get; set; }
        public object auto_delete_in_days { get; set; }
        public DateTime created_at { get; set; }
        public int data_limit { get; set; }
        public string data_limit_reset_strategy { get; set; }
        public Excluded_Inbounds excluded_inbounds { get; set; }
        public object expire { get; set; }
        public Inbounds inbounds { get; set; }
        public int lifetime_used_traffic { get; set; }
        public string[] links { get; set; }
        public object next_plan { get; set; }
        public string note { get; set; }
        public object on_hold_expire_duration { get; set; }
        public object on_hold_timeout { get; set; }
        public object online_at { get; set; }
        public Proxies proxies { get; set; }
        public string status { get; set; }
        public object sub_last_user_agent { get; set; }
        public object sub_updated_at { get; set; }
        public string subscription_url { get; set; }
        public int used_traffic { get; set; }
        public string username { get; set; }
    }

    public class Admin
    {
        public object discord_webhook { get; set; }
        public bool is_sudo { get; set; }
        public int telegram_id { get; set; }
        public string username { get; set; }
        public long users_usage { get; set; }
    }

    public class Excluded_Inbounds
    {
        public string[] vless { get; set; }
    }

    public class Inbounds
    {
        public string[] vless { get; set; }
    }

    public class Proxies
    {
        public Vless vless { get; set; }
    }

    public class Vless
    {
        public string flow { get; set; }
        public string id { get; set; }
    }

}

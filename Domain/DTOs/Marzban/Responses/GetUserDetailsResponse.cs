using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Marzban.Responses
{

    public class GetUserDetailsResponse
    {
        public long expire { get; set; }
        public long data_limit { get; set; }
        public string note { get; set; }
        public string status { get; set; }
        public long used_traffic { get; set; }
        public string[] links { get; set; }
        public string subscription_url { get; set; }
    }
}

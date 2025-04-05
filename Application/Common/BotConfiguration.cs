using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public class BotConfiguration
    {
        public string Domain { get; set; }
        public string BotToken { get; set; }
        public long[] AdminUserIds { get; set; }

    }
}

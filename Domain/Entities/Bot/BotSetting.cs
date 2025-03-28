using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Bot
{
    public class BotSetting
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Enums;

namespace Domain.Entities.Bot
{
    public class BotMessage
    {
        public int Id { get; set; }
        public BotCommand Command { get; set; }
        public string Message { get; set; }
    }
}

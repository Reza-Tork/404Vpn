using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Telegram.Bot.Types;

namespace Application.Helpers.Handlers
{
    public class CallbackQueryHandler : IUpdateHandler
    {
        public Task HandleUpdate(Update update)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Enums;

namespace Application.Helpers
{
    public static class MessageExtractor
    {
        private static Dictionary<string, BotCommand> BotCommands = new()
        {
            { "/start", BotCommand.Start },
            { "💸 خرید سرویس", BotCommand.BuyService },
            { "⚡️ تمدید سرویس", BotCommand.RenewService },
            { "♾️ حجم اضافه", BotCommand.ExtraBandwidth },
            { "🐉 سرویس ها", BotCommand.MyServices },
            { "📣 تعرفه ها", BotCommand.Plans },
            { "💰 کیف پول", BotCommand.Wallet },
            { "📞 پشتیبانی", BotCommand.Support },
            { "🔗 راهنمای اتصال", BotCommand.Help },
        };
        public static bool IsCommand(this string inputText, out BotCommand command)
        {
            if (BotCommands.TryGetValue(inputText, out BotCommand value))
            {
                command = value;
                return true;
            }
            command = default;
            return false;
        }
    }
}

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
            { "🔙 بازگشت به منوی اصلی", BotCommand.MainMenu }
        };
        private static Dictionary<string, BotCommand> InlineBotCommands = new()
        {
            { "MyServiceDetails", BotCommand.MyServiceDetails },
            { "BuyBandwidth", BotCommand.BuyBandwidth },
            { "RenewMyService", BotCommand.RenewMyService },
            { "ChargeWallet", BotCommand.ChargeWallet },
            { "PaymentMethod", BotCommand.PaymentMethod },
            { "CardToCard", BotCommand.CardToCard },
            { "Factor", BotCommand.Factor },

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

        public static bool IsInlineCommand(this string inputText, out BotCommand inlineCommand)
        {
            if (InlineBotCommands.TryGetValue(inputText, out BotCommand value))
            {
                inlineCommand = value;
                return true;
            }
            inlineCommand = default;
            return false;
        }

        public static string GetText(BotCommand command)
        {
            return BotCommands.FirstOrDefault(x => x.Value == command).Key;
        }
        public static string GetCallbackData(BotCommand command)
        {
            return InlineBotCommands.FirstOrDefault(x => x.Value == command).Key;
        }
    }
}

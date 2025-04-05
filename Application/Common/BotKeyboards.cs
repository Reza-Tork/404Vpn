using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Telegram.Bot.Types.ReplyMarkups;

namespace Application.Common
{
    public static class BotKeyboards
    {
        public static ReplyKeyboardMarkup MainKeyboard()
        {
            return new ReplyKeyboardMarkup()
            {
                IsPersistent = true,
                ResizeKeyboard = true,
                Keyboard = new List<List<KeyboardButton>>()
                {
                    new()
                    {
                        new("⚡️ تمدید سرویس"),
                        new("💸 خرید سرویس"),
                    },
                    new()
                    {
                        new("📣 تعرفه ها"),
                        new("🐉 سرویس ها"),
                        new("♾️ حجم اضافه"),
                    },
                    new()
                    {
                        new("💰 کیف پول")
                    },
                    new()
                    {
                        new("🔗 راهنمای اتصال"),
                        new("📞 پشتیبانی"),
                    }
                }
            };
        }
    }
}

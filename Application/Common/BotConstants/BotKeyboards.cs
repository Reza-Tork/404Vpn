using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Helpers;
using Domain.Entities.Enums;
using Domain.Entities.Vpn;
using Microsoft.AspNetCore.Mvc.Formatters;
using Telegram.Bot.Types.ReplyMarkups;

namespace Application.Common.BotConstants
{
    public static class BotKeyboards
    {
        public static ReplyKeyboardMarkup MainKeyboard(bool isAdmin)
        {
            var keyboardButtons = new List<List<KeyboardButton>>()
                {
                    new()
                    {
                        MessageExtractor.GetText(BotCommand.RenewService),
                        MessageExtractor.GetText(BotCommand.BuyService),
                    },
                    new()
                    {
                        MessageExtractor.GetText(BotCommand.Plans),
                        MessageExtractor.GetText(BotCommand.MyServices),
                        MessageExtractor.GetText(BotCommand.ExtraBandwidth),
                    },
                    new()
                    {
                        MessageExtractor.GetText(BotCommand.Wallet),
                    },
                    new()
                    {
                        MessageExtractor.GetText(BotCommand.Help),
                        MessageExtractor.GetText(BotCommand.Support),
                    }
                };

            if (isAdmin)
            {
                keyboardButtons.Add(new()
                {
                    MessageExtractor.GetText(BotCommand.AdminPanel)
                });
            }

            return new ReplyKeyboardMarkup()
            {
                ResizeKeyboard = true,
                Keyboard = keyboardButtons
            };
        }

        public static InlineKeyboardMarkup MainAdminKeyboard()
        {
            return new InlineKeyboardMarkup()
            {
                InlineKeyboard = new List<List<InlineKeyboardButton>>()
                {
                    new()
                    {
                        new(InlineButtonsTexts.GetAllServices)
                        {
                            CallbackData = "GetAllServices"
                        },
                    },
                    new()
                    {
                        new(InlineButtonsTexts.AddService)
                        {
                            CallbackData = "AddService"
                        },
                    }
                }
            };
        }
        public static InlineKeyboardMarkup GetServicesAdminKeyboard(List<Service> services)
        {
            var keyboard = new List<List<InlineKeyboardButton>>();
            foreach (var service in services)
            {
                var index = service.Index;
                keyboard[index].Add(new(service.Title)
                {

                });
            }
            return new InlineKeyboardMarkup()
            {
                InlineKeyboard = new List<List<InlineKeyboardButton>>()
                {
                    new()
                    {
                        new(InlineButtonsTexts.GetAllServices)
                        {
                            CallbackData = "GetAllServices"
                        },
                    },
                    new()
                    {
                        new(InlineButtonsTexts.AddService)
                        {
                            CallbackData = "AddService"
                        },
                    }
                }
            };
        }
    }
}

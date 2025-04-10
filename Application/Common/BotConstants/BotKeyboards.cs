﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public static ReplyKeyboardMarkup BackToMainMenu()
        {
            return new ReplyKeyboardMarkup()
            {
                ResizeKeyboard = true,
                Keyboard = new List<List<KeyboardButton>>()
                {
                    new()
                    {
                        MessageExtractor.GetText(BotCommand.MainMenu),
                    }
                }
            };
        }
        public static ReplyKeyboardMarkup Main()
        {
            return new ReplyKeyboardMarkup()
            {
                ResizeKeyboard = true,
                Keyboard = new List<List<KeyboardButton>>()
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
                }
            };
        }
        public static InlineKeyboardMarkup? Services(List<Service> services)
        {
            if (services.Count < 1)
                return null;
            var keyboard = new List<List<InlineKeyboardButton>>();
            var distinctedServices = services.DistinctBy(x => x.Index);

            while (keyboard.Count <= distinctedServices.Count())
                keyboard.Add(new List<InlineKeyboardButton>());

            foreach (var service in services)
            {
                var index = service.Index;
                keyboard[index].Add(new(service.Title)
                {
                    CallbackData = $"BuyService|{service.Id}"
                });
            }
            return new InlineKeyboardMarkup()
            {
                InlineKeyboard = keyboard
            };
        }
        public static InlineKeyboardMarkup? UserSubscriptions(List<UserSubscription> userSubscriptions, string callbackData)
        {
            if (userSubscriptions.Count < 1)
                return null;
            var keyboard = new List<List<InlineKeyboardButton>>();
            foreach (var subscription in userSubscriptions)
            {
                keyboard.Add(new()
                {
                    new($"{subscription.Service.Title}")
                    {
                        CallbackData = $"{callbackData}|{subscription.Id}"
                    }
                });
            }
            return new InlineKeyboardMarkup()
            {
                InlineKeyboard = keyboard
            };
        }
        public static InlineKeyboardMarkup Wallet(int walletBalance)
        {
            return new InlineKeyboardMarkup()
            {
                InlineKeyboard = new List<List<InlineKeyboardButton>>()
                {
                    new()
                    {
                        new($"{walletBalance:##,#0} تومان")
                        {
                            CallbackData = "None"
                        },
                        new("🔹 موجودی فعلی")
                        {
                            CallbackData = "None"
                        },
                    },
                    new()
                    {
                        new("💰 شارژ حساب")
                        {
                            CallbackData = $"{MessageExtractor.GetCallbackData(BotCommand.ChargeWallet)}|1"
                        }
                    }
                }
            };
        }
        public static InlineKeyboardMarkup PaymentMethod(int amount)
        {
            return new InlineKeyboardMarkup()
            {
                InlineKeyboard = new List<List<InlineKeyboardButton>>()
                {
                    new()
                    {
                        new("💳 کارت به کارت")
                        {
                            CallbackData = $"PaymentMethod|{amount}|CardToCard"
                        }
                    }
                }
            };
        }

        public static InlineKeyboardMarkup SetFactorState(Guid id, int userId)
        {
            return new InlineKeyboardMarkup()
            {
                InlineKeyboard = new List<List<InlineKeyboardButton>>()
                {
                    new()
                    {
                        new("✅ تایید")
                        {
                            CallbackData = $"Factor|{id}|{userId}|Confirm"
                        },
                        new("❌ رد")
                        {
                            CallbackData = $"Factor|{id}|{userId}|Reject"
                        }
                    }
                }
            };
        }
        public static InlineKeyboardMarkup? FactorState(int state)
        {
            switch (state)
            {
                case 0:
                    return new InlineKeyboardMarkup()
                    {
                        InlineKeyboard = new List<List<InlineKeyboardButton>>()
                        {
                            new()
                            {
                                new("❌ رد شده")
                                {
                                    CallbackData = $"None"
                                }
                            }
                        }
                    };
                case 1:
                    return new InlineKeyboardMarkup()
                    {
                        InlineKeyboard = new List<List<InlineKeyboardButton>>()
                        {
                            new()
                            {
                                new("✅ تایید شده")
                                {
                                    CallbackData = $"None"
                                },
                            }
                        }
                    };
                default:
                    return null;
            }
        }
    }
}

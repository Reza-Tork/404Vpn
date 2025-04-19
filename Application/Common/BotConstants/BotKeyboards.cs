using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Application.Helpers;
using Domain.Entities.Bot;
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
                keyboard.Add([]);

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
        public static InlineKeyboardMarkup? MonthPlans(int serviceId, List<MonthPlan> monthPlans)
        {
            if (monthPlans.Count < 1)
                return null;
            var keyboard = new List<List<InlineKeyboardButton>>();

            foreach (var monthPlan in monthPlans.OrderBy(x => x.Month))
            {
                keyboard.Add(new()
                {
                    new($"{monthPlan.Month} ماهه")
                    {
                        CallbackData = $"SelectMonth|{serviceId}|{monthPlan.Id.ToString().Replace("-", string.Empty)}"
                    }
                });
            }
            return new InlineKeyboardMarkup()
            {
                InlineKeyboard = keyboard
            };
        }
        public static InlineKeyboardMarkup? TrafficPlans(int serviceId, int monthPlanId, List<TrafficPlan> trafficPlans)
        {
            if (trafficPlans.Count < 1)
                return null;
            var keyboard = new List<List<InlineKeyboardButton>>();

            foreach (var trafficPlan in trafficPlans.OrderBy(x => x.Bandwidth))
            {
                keyboard.Add(new()
                {
                    new($"{trafficPlan.Bandwidth} گیگ")
                    {
                        CallbackData = $"SelectTraffic|{serviceId}|{monthPlanId}|{trafficPlan.Id}"
                    }
                });
            }
            return new InlineKeyboardMarkup()
            {
                InlineKeyboard = keyboard
            };
        }
        public static InlineKeyboardMarkup ServicePayment(int amount, string factorId)
        {
            return new InlineKeyboardMarkup()
            {
                InlineKeyboard = new List<List<InlineKeyboardButton>>()
                {
                    new()
                    {
                        new("💰 پرداخت از کیف پول")
                        {
                            CallbackData = $"PaymentMethod|{amount}|PayFromWallet|{factorId}"
                        }
                    }
                }
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
                    new($"{subscription.Service.Title} | {subscription.Username}")
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
        public static InlineKeyboardMarkup SubscriptionManagement(int subscriptionId)
        {
            return new InlineKeyboardMarkup()
            {
                InlineKeyboard = new List<List<InlineKeyboardButton>>()
                {
                    new()
                    {
                        new("تغییر نام سرویس")
                        {
                            CallbackData = $"RenameSubscription|{subscriptionId}"
                        },
                        new("مشخصات سرویس")
                        {
                            CallbackData = $"SubscriptionDetails|{subscriptionId}"
                        }
                    },
                    new()
                    {
                        new("تغییر لینک اتصال")
                        {
                            CallbackData = $"ChangeSubscriptionLink|{subscriptionId}"
                        },
                        new("تغییر پلن سرویس")
                        {
                            CallbackData = $"ChangeService|{subscriptionId}"
                        },
                    },
                    new()
                    {
                        new("بازگشت")
                        {
                            CallbackData = "BackToServices"
                        }
                    },
                },
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

        public static InlineKeyboardMarkup SetFactorState(string uniqueKey, int userId)
        {
            return new InlineKeyboardMarkup()
            {
                InlineKeyboard = new List<List<InlineKeyboardButton>>()
                {
                    new()
                    {
                        new("✅ تایید")
                        {
                            CallbackData = $"Factor|{uniqueKey}|{userId}|Confirm"
                        },
                        new("❌ رد")
                        {
                            CallbackData = $"Factor|{uniqueKey}|{userId}|Reject"
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

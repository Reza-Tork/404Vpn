using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Application.Common.BotConstants;
using Application.Interfaces;
using Domain.Entities.Bot;
using Domain.Entities.Enums;
using Domain.Entities.Vpn;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Application.Helpers.Handlers
{
    public class CallbackQueryHandler : IUpdateHandler
    {
        private readonly IUserService userService;
        private readonly IBotService botService;
        private readonly ITelegramBotClient botClient;
        private readonly ILogger<CallbackQueryHandler> logger;
        private readonly IVpnService vpnService;

        public CallbackQueryHandler(
            IUserService userService,
            IBotService botService,
            ITelegramBotClient botClient,
            ILogger<CallbackQueryHandler> logger,
            IVpnService vpnService)
        {
            this.userService = userService;
            this.botService = botService;
            this.botClient = botClient;
            this.logger = logger;
            this.vpnService = vpnService;
        }

        public async Task HandleUpdate(Update update)
        {
            if (update.CallbackQuery == null)
            {
                logger.LogInformation("Handle callbackqury: Callback is null");
                return;
            }
            var allBotMessages = (await botService.GetAllMessages()).Data!.ToList();
            var message = update.CallbackQuery.Message!;
            var user = (await userService.CheckUserExists(update.CallbackQuery.From)).Data!;
            var chatId = message.Chat.Id;
            var callbackData = update.CallbackQuery.Data;
            var callbackId = update.CallbackQuery.Id;
            if (callbackData == "None" || callbackData == null || !callbackData.Contains('|'))
                return;
            var callbackDataSplitted = callbackData.Split('|');
            if (callbackDataSplitted[0].IsInlineCommand(out Domain.Entities.Enums.BotCommand command))
            {
                var commandData = allBotMessages.FirstOrDefault(x => x.Command == command)!;
                switch (command)
                {
                    case Domain.Entities.Enums.BotCommand.BackToServices:
                        {
                            var allSubscriptions = await vpnService.GetUserSubscriptions(user.Id);
                            if (!allSubscriptions.IsSuccess || allSubscriptions.Data == null || allSubscriptions.Data.Count == 0)
                            {
                                await botClient.DeleteMessage(chatId, message.Id);
                                await botClient.SendMessage(chatId, "❌ شما سرویس فعالی ندارید", parseMode: ParseMode.Html, replyMarkup: BotKeyboards.Main());
                                return;
                            }

                            var subscriptionsKeyboard = BotKeyboards.UserSubscriptions([.. allSubscriptions.Data.OrderBy(x => x.CreationTime)], callbackData);
                            await botClient.EditMessageText(chatId, message.Id, commandData.Message, replyMarkup: subscriptionsKeyboard);
                        }
                        break;
                    case Domain.Entities.Enums.BotCommand.BuyServiceCallback:
                        {
                            var serviceId = callbackDataSplitted[1];
                            var serviceResult = await vpnService.GetService(int.Parse(serviceId));
                            if (!serviceResult.IsSuccess || serviceResult.Data is null)
                            {
                                await botClient.AnswerCallbackQuery(callbackId, "❌ سرویس مورد نظر وجود ندارد", true);
                                return;
                            }
                            var service = serviceResult.Data;
                            var returnMessage = commandData.Message
                                .Replace("<NAME>", service.Title);
                            var monthPlansResult = await vpnService.GetAllMonthPlans();
                            if (!monthPlansResult.IsSuccess || monthPlansResult.Data is null || monthPlansResult.Data.Count < 1)
                            {
                                await botClient.AnswerCallbackQuery(callbackId, "❌ خطا در دریافت لیست دوره های سرویس", true);
                                return;
                            }
                            await botClient.EditMessageText(chatId, message.Id, returnMessage, ParseMode.Html, replyMarkup: BotKeyboards.MonthPlans(service.Id, [.. monthPlansResult.Data]));
                        }
                        break;
                    case Domain.Entities.Enums.BotCommand.SelectMonthCallback:
                        {
                            var serviceId = callbackDataSplitted[1];
                            var monthPlanId = int.Parse(callbackDataSplitted[2]);
                            var serviceResult = await vpnService.GetService(int.Parse(serviceId));
                            if (!serviceResult.IsSuccess || serviceResult.Data is null)
                            {
                                await botClient.AnswerCallbackQuery(callbackId, "❌ سرویس مورد نظر وجود ندارد", true);
                                return;
                            }
                            var service = serviceResult.Data;
                            var monthPlanResult = await vpnService.GetMonthPlanById(monthPlanId);
                            if (!monthPlanResult.IsSuccess || monthPlanResult.Data is null)
                            {
                                await botClient.AnswerCallbackQuery(callbackId, "❌ دوره ی مورد نظر وجود ندارد", true);
                                return;
                            }
                            var monthPlan = monthPlanResult.Data;
                            var returnMessage = commandData.Message
                                .Replace("<NAME>", service.Title)
                                .Replace("<MONTH>", $"{monthPlan.Month}");
                            await botClient.EditMessageText(chatId, message.Id, returnMessage, ParseMode.Html, replyMarkup: BotKeyboards.TrafficPlans(service.Id, monthPlanId, [.. monthPlan.TrafficPlans]));
                        }
                        break;
                    case Domain.Entities.Enums.BotCommand.SelectTrafficCallback:
                        {
                            var serviceId = callbackDataSplitted[1];
                            var monthPlanId = int.Parse(callbackDataSplitted[2]);
                            var trafficPlanId = int.Parse(callbackDataSplitted[3]);
                            var serviceResult = await vpnService.GetService(int.Parse(serviceId));
                            if (!serviceResult.IsSuccess || serviceResult.Data is null)
                            {
                                await botClient.AnswerCallbackQuery(callbackId, "❌ سرویس مورد نظر وجود ندارد", true);
                                return;
                            }
                            var service = serviceResult.Data;

                            var monthPlanResult = await vpnService.GetMonthPlanById(monthPlanId);
                            if (!monthPlanResult.IsSuccess || monthPlanResult.Data is null)
                            {
                                await botClient.AnswerCallbackQuery(callbackId, "❌ دوره ی مورد نظر وجود ندارد", true);
                                return;
                            }
                            var monthPlan = monthPlanResult.Data;
                            var trafficPlan = monthPlan.TrafficPlans.FirstOrDefault(x => x.Id == trafficPlanId);
                            if (trafficPlan is null)
                            {
                                await botClient.AnswerCallbackQuery(callbackId, "❌ ترافیک مورد نظر وجود ندارد", true);
                                return;
                            }
                            var totalPrice = (trafficPlan.Bandwidth * trafficPlan.PricePerGb) + (monthPlan.PricePerMonth * monthPlan.Month) + 5000;
                            var returnMessage = commandData.Message
                                .Replace("<NAME>", service.Title)
                                .Replace("<MONTH>", $"{monthPlan.Month} ماهه")
                                .Replace("<TRAFFIC>", $"{trafficPlan.Bandwidth} گیگ")
                                .Replace("<PRICE>", $"{totalPrice:N0} تومان");

                            var factor = new Factor()
                            {
                                Amount = totalPrice,
                                CreatedAt = DateTime.UtcNow,
                                State = FactorState.Pending,
                                UniqueKey = StringHelpers.GenerateUsername(8),
                                UserId = user.Id,
                                UserSubscription = new UserSubscription()
                                {
                                    Bandwidth = trafficPlan.Bandwidth,
                                    Username = StringHelpers.GenerateUsername(),
                                    UserId = user.Id,
                                    ServiceId = service.Id,
                                    Status = Status.Pending,
                                    ExpireTime = DateTime.Now.AddMonths(monthPlan.Month).ToUniversalTime(),
                                    CreationTime = DateTime.UtcNow
                                }
                            };
                            var factorResult = await botService.CreateFactor(factor);
                            if (!factorResult.IsSuccess || factorResult.Data is null)
                            {
                                await botClient.AnswerCallbackQuery(callbackId, "❌ خطایی در ایجاد فاکتور رخ داد", true);
                                return;
                            }
                            await botClient.EditMessageText(chatId, message.Id, returnMessage, ParseMode.Html, replyMarkup: BotKeyboards.ServicePayment(totalPrice, factorResult.Data.UniqueKey));
                        }
                        break;
                    case Domain.Entities.Enums.BotCommand.MyServiceDetails:
                        {
                            var subscriptionResult = await vpnService.GetSubscription(int.Parse(callbackDataSplitted[1]));
                            if (!subscriptionResult.IsSuccess || subscriptionResult.Data is null)
                            {
                                await botClient.AnswerCallbackQuery(callbackId, "❌ سرویس درخواست شده یافت نشد", true);
                                return;
                            }
                            var subscriptionDetails = subscriptionResult.Data;
                            var note = string.IsNullOrEmpty(subscriptionDetails.Note) ? "تنظیم نشده" : subscriptionDetails.Note;
                            var returnMessage = commandData.Message
                                .Replace("<TITLE>", $"{subscriptionDetails.Username}")
                                .Replace("<SERVICE>", $"{subscriptionDetails.Service.Title}")
                                .Replace("<STATUS>", $"{subscriptionDetails.Status}")
                                .Replace("<NOTE>", $"{note}");
                            await botClient.EditMessageText(chatId, message.Id, returnMessage, ParseMode.Html, replyMarkup: BotKeyboards.SubscriptionManagement(subscriptionDetails.Id));
                        }
                        break;
                    case Domain.Entities.Enums.BotCommand.SubscriptionDetails:
                        {
                            var subscriptionResult = await vpnService.GetSubscription(int.Parse(callbackDataSplitted[1]));
                            if (!subscriptionResult.IsSuccess || subscriptionResult.Data is null)
                            {
                                await botClient.AnswerCallbackQuery(callbackId, "❌ سرویس درخواست شده یافت نشد", true);
                                return;
                            }
                            var subscriptionDetails = subscriptionResult.Data;
                            var note = string.IsNullOrEmpty(subscriptionDetails.Note) ? "تنظیم نشده" : subscriptionDetails.Note;
                            var returnMessage = commandData.Message
                                .Replace("<TITLE>", $"{subscriptionDetails.Username}")
                                .Replace("<SERVICE>", $"{subscriptionDetails.Service.Title}")
                                .Replace("<STATUS>", $"{subscriptionDetails.Status}")
                                .Replace("<NOTE>", $"{note}");
                            await botClient.EditMessageText(chatId, message.Id, returnMessage, ParseMode.Html, replyMarkup: BotKeyboards.SubscriptionManagement(subscriptionDetails.Id));
                        }
                        break;
                    case Domain.Entities.Enums.BotCommand.ChargeWallet:
                        {
                            user.Step = Step.ChargeWallet;
                            await userService.UpdateUser(user);
                            await botClient.SendMessage(chatId, commandData.Message, ParseMode.Html, replyMarkup: BotKeyboards.BackToMainMenu());
                            await botClient.DeleteMessage(chatId, message.Id);
                        }
                        break;
                    case Domain.Entities.Enums.BotCommand.PaymentMethod:
                        {
                            var amount = int.Parse(callbackData.Split('|')[1]);
                            if (callbackData.Split('|')[2].IsInlineCommand(out Domain.Entities.Enums.BotCommand method))
                            {
                                switch (method)
                                {
                                    case Domain.Entities.Enums.BotCommand.CardToCard:
                                        {
                                            var cardSettings = await botService.GetSetting("CARD");
                                            if (!cardSettings.IsSuccess || cardSettings.Data == null)
                                            {
                                                await botClient.AnswerCallbackQuery(update.CallbackQuery.Id, "❌ در حال حاضر این روش پرداخت غیرفعال است.");
                                                return;
                                            }
                                            var methodCommand = allBotMessages.FirstOrDefault(x => x.Command == method);
                                            var returnMessage = methodCommand.Message
                                                .Replace("<AMOUNT>", amount.ToString("##,#"))
                                                .Replace("<CARD>", cardSettings.Data.Value);
                                            user.Step = Step.ReceiptSending;
                                            user.StepData = $"{amount}";
                                            await userService.UpdateUser(user);
                                            await botClient.DeleteMessage(chatId, message.Id);
                                            await botClient.SendMessage(chatId, returnMessage, ParseMode.Html, replyMarkup: BotKeyboards.BackToMainMenu());
                                        }
                                        break;
                                    case Domain.Entities.Enums.BotCommand.PayFromWallet:
                                        {
                                            if (user.Wallet.Balance < amount)
                                            {
                                                await botClient.AnswerCallbackQuery(callbackId, "❌ موجودی کیف پول شما کافی نیست ، لطفا بعد از افزایش موجودی مجددا تلاش کنید", true);
                                                return;
                                            }
                                            var factorId = callbackDataSplitted[3];
                                            var factorResult = await botService.GetFactorByUniqueKey(factorId);
                                            if (factorResult.Data is null || !factorResult.IsSuccess)
                                            {
                                                await botClient.AnswerCallbackQuery(callbackId, "❌ فاکتور یافت نشد ، لطفا مجددا اقدام به ساخت سرویس یا پرداخت کنید", true);
                                                return;
                                            }
                                            var factor = factorResult.Data;
                                            if (factor.UserSubscription is not null)
                                            {
                                                switch (factor.UserSubscription.Status)
                                                {
                                                    case Status.Pending:
                                                        {
                                                            await botClient.AnswerCallbackQuery(callbackId, "لطفا صبر کنید ...");
                                                            
                                                            factor.UserSubscription.Status = Status.Active;
                                                            var service = await vpnService.GetService(factor.UserSubscription.ServiceId);
                                                            var days = ((factor.UserSubscription.ExpireTime - DateTime.UtcNow).Days + (DateTime.UtcNow - factor.CreatedAt).Days) + 2;
                                                            var locations = service.Data!.Tags.Contains(',') ? service.Data!.Tags.Split(',') : [service.Data!.Tags];
                                                            var creationResult = await vpnService.AddSubscription(user.Id, factor.UserSubscription.ServiceId, days, factor.UserSubscription.Bandwidth, locations);

                                                            if (creationResult.IsSuccess)
                                                            {
                                                                var userSubResult = await vpnService.UpdateSubscription(factor.UserSubscription);
                                                                user.Wallet.Balance -= amount;
                                                                var balanceResult = await userService.UpdateUser(user);
                                                                if (balanceResult.Data is null || !balanceResult.IsSuccess)
                                                                {
                                                                    await botClient.AnswerCallbackQuery(callbackId, "❌ موجودی کیف پول شما کافی نیست", true);
                                                                    return;
                                                                }
                                                                await botClient.SendMessage(chatId, @"🚀 پرداخت شما باموفقیت انجام شد و سرویس شما فعال شد.
🔹مشخصات سرویس خود را از طریق سرویس های من دریافت کنید.", ParseMode.Html, replyMarkup: BotKeyboards.Main());
                                                                await botClient.DeleteMessage(chatId, message.Id);
                                                            }
                                                            else
                                                            {
                                                                await botClient.AnswerCallbackQuery(callbackId, "خطایی رخ داده است ، لطفا به پشتیبانی اطلاع دهید ❌");
                                                            }
                                                        }
                                                        break;
                                                }
                                            }
                                            else
                                            {

                                            }


                                        }
                                        break;
                                    default:
                                        {
                                            await botClient.AnswerCallbackQuery(update.CallbackQuery.Id, "❌ این روش پرداخت وجود ندارد.");
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                    case Domain.Entities.Enums.BotCommand.Factor:
                        {
                            if (!user.IsAdmin())
                                return;

                            var callDataSplitted = callbackData.Split('|');
                            var factorId = callDataSplitted[1];
                            var targetUserId = callDataSplitted[2];
                            switch (callDataSplitted[3])
                            {
                                case "Confirm":
                                    {
                                        var factor = (await botService.GetFactorByUniqueKey(factorId)).Data;
                                        if (factor == null)
                                        {
                                            await botClient.AnswerCallbackQuery(update.CallbackQuery.Id, "❌ این فاکتور وجود ندارد");
                                            return;
                                        }

                                        factor.State = FactorState.Confirmed;
                                        await botService.UpdateFactor(factor);
                                        var factorUser = await userService.GetUserById(int.Parse(targetUserId));
                                        if (factorUser == null || factorUser.Data == null)
                                        {
                                            await botClient.AnswerCallbackQuery(update.CallbackQuery.Id, "❌ کاربر متصل به این فاکتور وجود ندارد");
                                            return;
                                        }

                                        factorUser.Data.Wallet.Balance += factor.Amount;
                                        await userService.UpdateUser(factorUser.Data);
                                        await botClient.SendMessage(factorUser.Data.UserId, $@"🔹 فاکتور زیر توسط مدیریت تایید شد:

✏️ کد فاکتور: <code>{factor.UniqueKey}</code>
💰 مبلغ: <code>{factor.Amount:##,#}</code>
〰️〰️〰️
💳 موجودی جدید کیف پول: <code>{factorUser.Data.Wallet.Balance:##,#}</code>", ParseMode.Html);
                                        await botClient.EditMessageReplyMarkup(chatId, message.Id, BotKeyboards.FactorState(1));
                                    }
                                    break;
                                case "Reject":
                                    {
                                        var factor = (await botService.GetFactorByUniqueKey(factorId)).Data;
                                        if (factor == null)
                                        {
                                            await botClient.AnswerCallbackQuery(update.CallbackQuery.Id, "❌ این فاکتور وجود ندارد");
                                            return;
                                        }

                                        factor.State = FactorState.Confirmed;
                                        await botService.UpdateFactor(factor);
                                        var factorUser = await userService.GetUserById(int.Parse(targetUserId));
                                        if (factorUser == null || factorUser.Data == null)
                                        {
                                            await botClient.AnswerCallbackQuery(update.CallbackQuery.Id, "❌ کاربر متصل به این فاکتور وجود ندارد");
                                            return;
                                        }
                                        await botClient.SendMessage(factorUser.Data.UserId, $@"🔺 فاکتور زیر توسط مدیریت رد شد:

✏️ کد فاکتور: <code>{factor.UniqueKey}</code>
💰 مبلغ: <code>{factor.Amount:##,#}</code>", ParseMode.Html);
                                        await botClient.EditMessageReplyMarkup(chatId, message.Id, BotKeyboards.FactorState(0));
                                    }
                                    break;
                            }
                        }
                        break;
                    default:
                        {
                            await botClient.AnswerCallbackQuery(update.CallbackQuery.Id, "دستور یافت نشد");
                        }
                        break;

                }
            }
            else
            {
                await botClient.AnswerCallbackQuery(update.CallbackQuery.Id, "دستور در لیست وجود ندارد");
            }
        }
    }
}

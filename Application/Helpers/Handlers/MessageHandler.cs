using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.BotConstants;
using Application.Interfaces;
using Domain.Entities.Bot;
using Domain.Entities.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Application.Helpers.Handlers
{
    public class MessageHandler : IUpdateHandler
    {
        private readonly IUserService userService;
        private readonly IBotService botService;
        private readonly ITelegramBotClient botClient;
        private readonly IVpnService vpnService;

        public MessageHandler(IUserService userService, IBotService botService, ITelegramBotClient botClient, IVpnService vpnService)
        {
            this.userService = userService;
            this.botService = botService;
            this.botClient = botClient;
            this.vpnService = vpnService;
        }
        public async Task HandleUpdate(Update update)
        {

            switch (update.Message!.Type)
            {
                case MessageType.Text:
                    {
                        await HandleTextMessage(update);
                    }
                    break;
                case MessageType.Photo:
                    {
                        await HandlePhotoMessage(update);
                    }
                    break;
            }
        }
        private async Task HandlePhotoMessage(Update update)
        {
            var allBotMessages = (await botService.GetAllMessages()).Data!.ToList();
            var user = (await userService.CheckUserExists(update.Message!.From!)).Data!;
            var message = update.Message;
            var chatId = update.Message.Chat.Id;
            switch (user.Step)
            {
                case Step.ReceiptSending:
                    {
                        var receiptSettings = await botService.GetSetting("RECEIPT_CHATID");
                        if (!receiptSettings.IsSuccess || receiptSettings.Data == null)
                            return;
                        var receiptChatId = receiptSettings.Data.Value;

                        var factor = await botService.CreateFactor(new Factor()
                        {
                            UserId = user.Id,
                            UniqueKey = StringHelpers.GenerateUsername(8),
                            Amount = int.Parse(user.StepData!),
                            State = FactorState.Pending,
                            CreatedAt = DateTime.UtcNow,
                        });

                        await botClient.SendPhoto(long.Parse(receiptChatId), message.Photo!.Last(), @$"🚀 درخواست واریز جدید:

💰 مبلغ: <code>{int.Parse(user.StepData):##,#} تومان</code>

💳 تاریخ: {DateTime.Now.ToPersianDate()}

🔹 کاربر: {user.UserId}
〰️〰️〰️〰️
⚙️ کد فاکتور: 
<code>{factor.Data!.UniqueKey}</code>", parseMode: ParseMode.Html, replyMarkup: BotKeyboards.SetFactorState(factor.Data!.UniqueKey, user.Id));

                        user.StepData = "";
                        user.Step = Step.None;
                        await userService.UpdateUser(user);
                        await botClient.SendMessage(chatId, $@"🚀 رسید پرداخت  شما ارسال شد.
▫️ پس از تایید توسط مدیریت مبلغ به کیف پول شما واریز خواهد شد.

⚙️ کد فاکتور: <code>{factor.Data!.UniqueKey}</code>", ParseMode.Html, replyMarkup: BotKeyboards.Main());
                    }
                    break;
            }
        }
        private async Task HandleTextMessage(Update update)
        {
            var allBotMessages = (await botService.GetAllMessages()).Data!.ToList();
            var user = (await userService.CheckUserExists(update.Message!.From!)).Data!;
            var message = update.Message;
            var chatId = update.Message.Chat.Id;
            if (message.Text!.IsCommand(out Domain.Entities.Enums.BotCommand command))
            {
                var commandData = allBotMessages.FirstOrDefault(x => x.Command == command);
                switch (command)
                {
                    case Domain.Entities.Enums.BotCommand.Start or Domain.Entities.Enums.BotCommand.MainMenu:
                        {
                            if (user.Step != Step.None)
                            {
                                user.Step = Step.None;
                                user.StepData = "";
                                await userService.UpdateUser(user);
                            }
                            var returnMessage = commandData.Message
                                .Replace("<NAME>", $"{user.FirstName} {user.LastName}");
                            await botClient.SendMessage(chatId, returnMessage, parseMode: ParseMode.Html, replyMarkup: BotKeyboards.Main());
                        }
                        break;
                    case Domain.Entities.Enums.BotCommand.BuyService:
                        {
                            var allServices = await vpnService.GetAllServices();
                            if (allServices.Data is null || !allServices.IsSuccess || allServices.Data.Count < 1)
                            {
                                await botClient.SendMessage(chatId, "❌ در حال حاضر سرویسی برای فروش موجود نیست!", replyMarkup: BotKeyboards.Main());
                                return;
                            }
                            var servicesKeyboard = BotKeyboards.Services([.. allServices.Data]);
                            await botClient.SendMessage(chatId, commandData.Message, parseMode: ParseMode.Html, replyMarkup: servicesKeyboard);
                        }
                        break;
                    case Domain.Entities.Enums.BotCommand.RenewService or Domain.Entities.Enums.BotCommand.MyServices or Domain.Entities.Enums.BotCommand.ExtraBandwidth:
                        {
                            var callbackData = command switch
                            {
                                Domain.Entities.Enums.BotCommand.MyServices => MessageExtractor.GetCallbackData(Domain.Entities.Enums.BotCommand.MyServiceDetails),
                                Domain.Entities.Enums.BotCommand.ExtraBandwidth => MessageExtractor.GetCallbackData(Domain.Entities.Enums.BotCommand.BuyBandwidth),
                                Domain.Entities.Enums.BotCommand.RenewService => MessageExtractor.GetCallbackData(Domain.Entities.Enums.BotCommand.RenewMyService)
                            };

                            var allSubscriptionsResult = await vpnService.GetUserSubscriptions(user.Id);
                            var allServices = allSubscriptionsResult.Data;
                            if (!allSubscriptionsResult.IsSuccess || allServices is null || allServices.Count == 0)
                            {
                                await botClient.SendMessage(chatId, $@"🚫 در حال حاضر سرویس فعالی نداری، {user.FirstName} {user.LastName} عزیز!

اما نگران نباش! فقط چند کلیک با یه اتصال پرسرعت و بدون محدودیت فاصله داری! 🌍
الان وقتشه که یکی از پلن‌های پرطرفدارمون رو انتخاب کنی و اینترنت رو همون‌طوری تجربه کنی که باید باشه
آزاد، سریع و امن! 🔐⚡️

🎯 همین حالا سرویس خودتو از طریق خرید سرویس فعال کن و به دنیای بدون محدودیت وصل شو", parseMode: ParseMode.Html, replyMarkup: BotKeyboards.Main());
                                return;
                            }
                            if (command == Domain.Entities.Enums.BotCommand.MyServices)
                                allServices = allServices.Where(x => x.Status == Domain.Entities.Vpn.Status.Active).ToList();
                            var subscriptionsKeyboard = BotKeyboards.UserSubscriptions([.. allServices.OrderBy(x => x.CreationTime)], callbackData);
                            await botClient.SendMessage(chatId, commandData.Message, replyMarkup: subscriptionsKeyboard);
                        }
                        break;
                    case Domain.Entities.Enums.BotCommand.Wallet:
                        {
                            await botClient.SendMessage(chatId, commandData.Message, parseMode: ParseMode.Html, replyMarkup: BotKeyboards.Wallet(user.Wallet.Balance));
                        }
                        break;
                    case Domain.Entities.Enums.BotCommand.Plans:
                        {
                            var result = await vpnService.GetAllMonthPlans();

                            if (!result.IsSuccess || result.Data is null || result.Data.Count == 0)
                            {
                                await botClient.SendMessage(chatId, "❌  پلنی جهت فروش وجود ندارد");
                                return;
                            }

                            var messageBuilder = new StringBuilder(
                                "📣 لیست تعرفه سرویس های 404 نت\n\n" +
                                "📌 شما میتوانید قبل از خرید سرویس تعرفه تمامی سرویس های موجود را مشاهده کنید و طبق آن اقدام به خرید سرویس کنید\n"
                            );

                            foreach (var plan in result.Data.OrderBy(x => x.Month))
                            {
                                messageBuilder.AppendLine($"\n🕰 سرویس های {plan.Month} ماهه 👇");

                                for (int i = 0; i < plan.TrafficPlans.Count; i++)
                                {
                                    var trafficPlan = plan.TrafficPlans.OrderBy(x => x.Bandwidth).ToList()[i];
                                    var prefix = (i % 2 == 0) ? "🔸" : "🔹";
                                    var totalPrice = (trafficPlan.Bandwidth * trafficPlan.PricePerGb) + (plan.PricePerMonth * plan.Month) + 5000;

                                    messageBuilder.AppendLine($"{prefix} {trafficPlan.Bandwidth} گیگ | {totalPrice:N0} تومان");
                                }
                            }

                            await botClient.SendMessage(chatId, messageBuilder.ToString());
                        }
                        break;
                    default:
                        {
                            await botClient.SendMessage(chatId, commandData.Message, parseMode: ParseMode.Html, replyMarkup: BotKeyboards.Main());
                        }
                        break;
                }
            }
            else
            {
                switch (user.Step)
                {
                    case Step.ChargeWallet:
                        {
                            var minAmount = await botService.GetSetting("MIN_AMOUNT");
                            var maxAmount = await botService.GetSetting("MAX_AMOUNT");
                            var invalidAmountMessage = @$"❌ مقدار وارد شده صحیح نیست ، لطفا فقط عدد انگلیسی و بیشتر از {int.Parse(minAmount.Data!.Value).ToString("##,#")} تومان و کمتر از {int.Parse(maxAmount.Data!.Value).ToString("##,#")} تومان وارد کنید:";
                            if (int.TryParse(message.Text, out int amount))
                            {
                                if (amount < int.Parse(minAmount.Data!.Value) || amount > int.Parse(maxAmount.Data!.Value))
                                {
                                    await botClient.SendMessage(chatId, invalidAmountMessage, ParseMode.Html, replyMarkup: BotKeyboards.BackToMainMenu());
                                    return;
                                }
                                var paymentMethodCommand = allBotMessages.FirstOrDefault(x => x.Command == Domain.Entities.Enums.BotCommand.PaymentMethod);
                                user.Step = Step.None;
                                await userService.UpdateUser(user);
                                await botClient.SendMessage(chatId, paymentMethodCommand.Message.Replace("<AMOUNT>", amount.ToString("##,#")), parseMode: ParseMode.Html, replyMarkup: BotKeyboards.PaymentMethod(amount));
                            }
                            else
                            {
                                await botClient.SendMessage(chatId, invalidAmountMessage, ParseMode.Html, replyMarkup: BotKeyboards.BackToMainMenu());
                            }
                        }
                        break;
                    case Step.ExtraBandwidth:
                        {

                        }
                        break;
                }
            }
        }
    }
}

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
<code>{factor.Data!.Id.ToString().ToLower().Replace("-", "")}</code>",parseMode: ParseMode.Html, replyMarkup: BotKeyboards.SetFactorState(factor.Data!.Id, user.Id));

                        user.StepData = "";
                        user.Step = Step.None;
                        await userService.UpdateUser(user);
                        await botClient.SendMessage(chatId, $@"🚀 رسید پرداخت  شما ارسال شد.
▫️ پس از تایید توسط مدیریت مبلغ به کیف پول شما واریز خواهد شد.

⚙️ کد فاکتور: <code>{factor.Data!.Id.ToString().ToLower().Replace("-", "")}</code>", ParseMode.Html, replyMarkup: BotKeyboards.Main());
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
                            await botClient.SendMessage(chatId, commandData.Message, parseMode: ParseMode.Html, replyMarkup: BotKeyboards.Main());
                        }
                        break;
                    case Domain.Entities.Enums.BotCommand.BuyService:
                        {
                            var allServices = await vpnService.GetAllServices();
                            if (allServices.Data == null || !allServices.IsSuccess)
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

                            var allSubscriptions = await vpnService.GetUserSubscriptions(user.Id);
                            if (!allSubscriptions.IsSuccess || allSubscriptions.Data == null || allSubscriptions.Data.Count == 0)
                            {
                                await botClient.SendMessage(chatId, "❌ شما سرویس فعالی ندارید", parseMode: ParseMode.Html, replyMarkup: BotKeyboards.Main());
                                return;
                            }

                            var subscriptionsKeyboard = BotKeyboards.UserSubscriptions([.. allSubscriptions.Data], callbackData);
                            await botClient.SendMessage(chatId, commandData.Message, replyMarkup: subscriptionsKeyboard);
                        }
                        break;
                    case Domain.Entities.Enums.BotCommand.Wallet:
                        {
                            await botClient.SendMessage(chatId, commandData.Message, parseMode: ParseMode.Html, replyMarkup: BotKeyboards.Wallet(user.Wallet.Balance));
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

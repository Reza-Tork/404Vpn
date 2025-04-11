using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.BotConstants;
using Application.Interfaces;
using Domain.Entities.Bot;
using Domain.Entities.Enums;
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

        public CallbackQueryHandler(IUserService userService, IBotService botService, ITelegramBotClient botClient, ILogger<CallbackQueryHandler> logger)
        {
            this.userService = userService;
            this.botService = botService;
            this.botClient = botClient;
            this.logger = logger;
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
            if (callbackData == "None" || callbackData == null || !callbackData.Contains('|'))
                return;

            if(callbackData.Split('|')[0].IsInlineCommand(out Domain.Entities.Enums.BotCommand command))
            {
                var commandData = allBotMessages.FirstOrDefault(x => x.Command == command);
                switch (command)
                {
                    case Domain.Entities.Enums.BotCommand.MyServiceDetails:
                        {

                        }
                        break;
                    case Domain.Entities.Enums.BotCommand.ChargeWallet:
                        {
                            user.Step = Step.ChargeWallet;
                            await userService.UpdateUser(user);
                            await botClient.DeleteMessage(chatId, message.Id);
                            await botClient.SendMessage(chatId, commandData.Message, ParseMode.Html, replyMarkup: BotKeyboards.BackToMainMenu());
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
                            var factorId = Guid.Parse(callDataSplitted[1]);
                            var targetUserId = callDataSplitted[2];
                            switch (callDataSplitted[3])
                            {
                                case "Confirm":
                                    {
                                        var factor = (await botService.GetFactor(factorId)).Data;
                                        if(factor == null)
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

✏️ کد فاکتور: <code>{factor.Id.ToString().Replace("-", "")}</code>
💰 مبلغ: <code>{factor.Amount:##,#}</code>
〰️〰️〰️
💳 موجودی جدید کیف پول: <code>{factorUser.Data.Wallet.Balance:##,#}</code>", ParseMode.Html);
                                        await botClient.EditMessageReplyMarkup(chatId, message.Id, BotKeyboards.FactorState(1));
                                    }
                                    break;
                                case "Reject":
                                    {
                                        var factor = (await botService.GetFactor(factorId)).Data;
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

✏️ کد فاکتور: <code>{factor.Id.ToString().ToLower().Replace("-", "")}</code>
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

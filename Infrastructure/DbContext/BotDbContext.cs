﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Bot;
using Domain.Entities.Enums;
using Domain.Entities.Vpn;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContext
{
    public class BotDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<BotSetting> BotSettings { get; set; }
        public DbSet<ApiInfo> ApiInfos { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<UserSubscription> UsersSubscriptions { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Factor> Factors { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<BotMessage> BotMessages { get; set; }
        public DbSet<MonthPlan> MonthPlans { get; set; }
        public DbSet<TrafficPlan> TrafficPlans { get; set; }

        public BotDbContext(DbContextOptions<BotDbContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MonthPlan>().HasData(
            [
                new MonthPlan()
                {
                    Id = 1,
                    Month = 1,
                    PricePerMonth = 5000
                },
                new MonthPlan()
                {
                    Id = 2,
                    Month = 3,
                    PricePerMonth = 3000
                },
                new MonthPlan()
                {
                    Id = 3,
                    Month = 6,
                    PricePerMonth = 2000
                },
            ]);
            modelBuilder.Entity<TrafficPlan>().HasData(
            [
                new()
                {
                    Id = 7,
                    Bandwidth = 150,
                    PricePerGb = 2000,
                    MonthPlanId = 3
                },
                new()
                {
                    Id = 8,
                    Bandwidth = 200,
                    PricePerGb = 1800,
                    MonthPlanId = 3
                },
                new()
                {
                    Id = 9,
                    Bandwidth = 450,
                    PricePerGb = 1500,
                    MonthPlanId = 3
                },
                new()
                {
                    Id = 4,
                    Bandwidth = 30,
                    PricePerGb = 2750,
                    MonthPlanId = 2
                },
                new()
                {
                    Id = 5,
                    Bandwidth = 60,
                    PricePerGb = 2650,
                    MonthPlanId = 2
                },
                new()
                {
                    Id = 6,
                    Bandwidth = 90,
                    PricePerGb = 2500,
                    MonthPlanId = 2
                },
                new()
                {
                    Id = 1,
                    Bandwidth = 15,
                    PricePerGb = 3000,
                    MonthPlanId = 1
                },
                new()
                {
                    Id = 2,
                    Bandwidth = 30,
                    PricePerGb = 2850,
                    MonthPlanId =1
                },
                new()
                {
                    Id = 3,
                    Bandwidth = 45,
                    PricePerGb = 2750,
                    MonthPlanId = 1
                },
            ]);

            modelBuilder.Entity<BotSetting>().HasData(
            [
                new BotSetting()
                {
                    Id = 1,
                    Key = "DOMAIN",
                    Value = "https://library98.ir/"
                },
                new BotSetting()
                {
                    Id = 2,
                    Key = "STATUS",
                    Value = "1"
                },
                new BotSetting()
                {
                    Id = 3,
                    Key = "MIN_AMOUNT",
                    Value = "50000"
                },
                new BotSetting()
                {
                    Id = 4,
                    Key = "MAX_AMOUNT",
                    Value = "500000"
                },
                new BotSetting()
                {
                    Id = 5,
                    Key = "RECEIPT_CHATID",
                    Value = "-1002583876730"
                },
                new BotSetting()
                {
                    Id = 6,
                    Key = "CARD",
                    Value = "0000000000000000"
                },
            ]);

            modelBuilder.Entity<User>().HasData(new User()
            {
                Id = 1,
                FirstName = "Main",
                LastName = "Admin",
                JoinDate = DateTime.UtcNow,
                Step = Step.None,
                StepData = "",
                UserId = 7880935437,
                Username = "MrMorphling"
            });

            modelBuilder.Entity<Admin>().HasData(new Admin()
            {
                Id = 1,
                UserId = 1,
                Step = AdminStep.None
            });

            var botMessages = new List<BotMessage>
            {
                new BotMessage { Id = (int)BotCommand.Start, Command = BotCommand.Start, Message = @"🌐 سلام <NAME> عزیز!
به ربات رسمی وی‌پی‌ان 404 خوش اومدی! 🚀

اینجا قراره با چند کلیک ساده، به یه اینترنت آزاد، پرسرعت و امن دسترسی داشته باشی! 🔓⚡️
ما بهت قول می‌دیم دیگه با محدودیت خداحافظی کنی! 💥

برای شروع، یکی از گزینه‌های زیر رو انتخاب کن و از تجربه بی‌دغدغه اینترنت لذت ببر 😎👇

✨ امنیت بالا، سرعت خفن، قیمت منصفانه
💬 هر سوالی داشتی، <a href='https://t.me/the404vpnSupport'>پشتیبانی</a> همیشه آنلاینه!

🆔 @the404vpnRobot" },
                new BotMessage { Id = (int)BotCommand.MainMenu, Command = BotCommand.MainMenu, Message = @"🌐 سلام <NAME> عزیز!
به ربات رسمی وی‌پی‌ان 404 خوش اومدی! 🚀

اینجا قراره با چند کلیک ساده، به یه اینترنت آزاد، پرسرعت و امن دسترسی داشته باشی! 🔓⚡️
ما بهت قول می‌دیم دیگه با محدودیت خداحافظی کنی! 💥

برای شروع، یکی از گزینه‌های زیر رو انتخاب کن و از تجربه بی‌دغدغه اینترنت لذت ببر 😎👇

✨ امنیت بالا، سرعت خفن، قیمت منصفانه
💬 هر سوالی داشتی، <a href='https://t.me/the404vpnSupport'>پشتیبانی</a> همیشه آنلاینه!

🆔 @the404vpnRobot" },
                new BotMessage { Id = (int)BotCommand.BuyService, Command = BotCommand.BuyService, Message = @"🛍 انتخاب سرویس، قدم اول به سوی آزادی بی‌مرز در اینترنت!

📦 ما برات مجموعه‌ای از سرویس‌های حرفه‌ای آماده کردیم که هم از نظر سرعت و امنیت فوق‌العاده‌ان، هم از نظر قیمت، کاملاً منصفانه! 

✨ فقط کافیه پلنی که مناسب توئه رو انتخاب کنی و در کمتر از چند ثانیه، وصل شی به یه دنیای بدون محدودیت!

💡 نکته مهم: هر سرویس با توجه به نیازت طراحی شده 
چه کاربر روزمره باشی، تریدر, استریمر یا گیمر حرفه‌ای 🎮

👇 از لیست زیر پلن دلخواهتو انتخاب کن و برو برای اتصال بی‌دغدغه!" },
                new BotMessage { Id = (int)BotCommand.RenewService, Command = BotCommand.RenewService, Message = "♻️ یکی از سرویس هایه مورد نظر برای تمدیدت رو انتخاب کن" },
                new BotMessage { Id = (int)BotCommand.MyServices, Command = BotCommand.MyServices, Message = @"📦 سرویس‌های فعال شما

اینجا می‌تونی مشخصات همه‌ی سرویس‌هات رو ببینی، وضعیتشون رو چک کنی و اگه لازم بود تمدید یا ارتقاشون بدی 
👇 لیست سرویس‌های شما:" },
                new BotMessage { Id = (int)BotCommand.ExtraBandwidth, Command = BotCommand.ExtraBandwidth, Message = @"🔋 افزایش حجم سرویس

سرویس انتخاب‌شده: <TITLE> ✅
حالا فقط کافیه مقدار حجمی که نیاز داری رو انتخاب کنی 👇

📦 هر چقدر بیشتر، اتصال طولانی‌تر و بی‌دردسرتر!" },
                new BotMessage { Id = (int)BotCommand.Plans, Command = BotCommand.Plans, Message = "TextMessage-PlansMessage" },
                new BotMessage { Id = (int)BotCommand.Wallet, Command = BotCommand.Wallet, Message = @"💰 کیف پول شما
از موجودیت می‌تونی برای خرید، تمدید یا افزایش حجم استفاده کنی ✨

همچنین برای شارژ حساب روی دکمه زیر کلیک کنید" },
                new BotMessage { Id = (int)BotCommand.Support, Command = BotCommand.Support, Message = @"🛟 نیاز به راهنمایی یا مشکلی پیش اومده؟

نگران نباش، تیم پشتیبانی ما اینجاست تا هر زمان که نیاز داشتی، کنارت باشه 🙌
چه مشکلت مربوط به خرید، اتصال، یا هر مورد دیگه‌ای باشه، فقط کافیه به پشتیبانی پیام بدی تا سریع راهنماییت کنیم 🛠💬

🆔 آیدی پشتیبانی:
@the404vpnSupport

🕒 ساعات پاسخ‌گویی: شبانه روز, حتی روزهای تعطیل!

✨ ما بهت قول می‌دیم تجربه‌ات از 404 نت همیشه راحت، شفاف و بدون دغدغه باشه.
در هر مرحله‌ای که بودی، پشتیبانی یه پیام باهاته 🤝" },
                new BotMessage { Id = (int)BotCommand.Help, Command = BotCommand.Help, Message = "پیام راهنما" },
                new BotMessage { Id = (int)BotCommand.MyServiceDetails, Command = BotCommand.MyServiceDetails, Message = @"🧾 جزئیات سرویس شما

📌 نام نمایشی: <TITLE>
🌐 سرویس: <SERVICE>
📶 وضعیت: <STATUS>
📝 یادداشت: <NOTE>

✨ برای مدیریت سرویس‌ات میتونی از دکمه های زیر استفاده کنی" },
                new BotMessage { Id = (int)BotCommand.BuyBandwidth, Command = BotCommand.BuyBandwidth, Message = @"سرویس انتخاب شده - وارد کردن حجم" },
                new BotMessage { Id = (int)BotCommand.RenewMyService, Command = BotCommand.RenewMyService, Message = "سرویس انتخاب شده جهت تمدید - تعداد ماه" },
                new BotMessage { Id = (int)BotCommand.ChargeWallet, Command = BotCommand.ChargeWallet, Message = @"💳 شارژ کیف پول
📌 با شارژ کیف پول، خرید و تمدید سرویس‌ها سریع‌تر و راحت‌تر انجام می‌شه!

حالا مبلغ موردنظرت رو برای شارژ حساب وارد کن 👇" },
                new BotMessage { Id = (int)BotCommand.PaymentMethod, Command = BotCommand.PaymentMethod, Message = @"💰 مبلغ وارد شده: <AMOUNT>

حالا فقط کافیه روش پرداختت رو انتخاب کنی 👇" },
                new BotMessage { Id = (int)BotCommand.CardToCard, Command = BotCommand.CardToCard, Message = @"روش پرداخت: کارت به کارت🏦 روش پرداخت: کارت به کارت

💳 مبلغ: <code><AMOUNT></code>

🔢 شماره کارت: <code><CARD></code>

لطفاً مبلغ رو به شماره کارت بالا واریز کن و رسید پرداخت رو همینجا برای ما ارسال کن ✅
📌 بعد از تایید، کیف پولت به‌صورت خودکار شارژ می‌شه و شمارو در اطلاع میزاریم

🛟 مشکلی داشتی؟ پشتیبانی همیشه در دسترسته: @the404vpnSupport" },
                new BotMessage { Id = (int)BotCommand.BuyServiceCallback, Command = BotCommand.BuyServiceCallback, Message = @"سرویس انتخاب شده: <NAME>
انتخاب مدت زمان سرویس:" },
                new BotMessage { Id = (int)BotCommand.SelectMonthCallback, Command = BotCommand.SelectMonthCallback, Message = @"سرویس انتخاب شده: <NAME>
مدت زمان سرویس: <MONTH>
انتخاب ترافیک:" },
                new BotMessage { Id = (int)BotCommand.SelectTrafficCallback, Command = BotCommand.SelectTrafficCallback, Message = @"🧾 فاکتور سرویس شما آماده‌ست!

همه‌چیز برای شروع یه تجربه سریع، امن و بدون محدودیت آماده‌ست! 🚀

🔹 سرویس انتخاب‌شده: <NAME>
📅 مدت زمان: <MONTH> ماه
📦 حجم: <TRAFFIC> گیگ

💰 مبلغ نهایی: <PRICE> تومان

✨ با پرداخت این فاکتور، فقط چند ثانیه تا اتصال به یه اینترنت پایدار و پرسرعت فاصله داری!
👇 حالا فقط کافیه روش پرداختتو انتخاب کنی و بریم برای فعال‌سازی" },
                new BotMessage { Id = (int)BotCommand.SubscriptionDetails, Command = BotCommand.SubscriptionDetails, Message = @"✨ جزئیات سرویس اختصاصی شما
📌 نام نمایشی: <code><TITLE></code>
🌐 نوع سرویس: <code><SERVICE></code>
⚙️ وضعیت فعلی: <code><STATUS></code>
📝 یادداشت اختصاصی: <code><NOTE></code>

🕰 تاریخ شروع: <b><CREATE></b>
⏳ تاریخ پایان: <b><EXPIRE></b>
📊 مصرف شما تا این لحظه:
<USED> گیگ از <BANDWIDTH> گیگ
" },

            };

            modelBuilder.Entity<BotMessage>().HasData(botMessages);

            modelBuilder.Entity<UserSubscription>()
            .HasKey(us => us.Id);

            modelBuilder.Entity<UserSubscription>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserSubscriptions)
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserSubscription>()
                .HasOne(us => us.Service)
                .WithMany(s => s.UsersSubscriptions)
                .HasForeignKey(us => us.ServiceId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

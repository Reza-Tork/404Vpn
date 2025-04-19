using System;
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
                new BotMessage { Id = (int)BotCommand.BuyService, Command = BotCommand.BuyService, Message = "پیام خرید سرویس - مرحله انتخاب سرویس" },
                new BotMessage { Id = (int)BotCommand.RenewService, Command = BotCommand.RenewService, Message = "♻️ یکی از سرویس هایه مورد نظر برای تمدیدت رو انتخاب کن" },
                new BotMessage { Id = (int)BotCommand.MyServices, Command = BotCommand.MyServices, Message = @"📦 سرویس‌های فعال شما

اینجا می‌تونی مشخصات همه‌ی سرویس‌هات رو ببینی، وضعیتشون رو چک کنی و اگه لازم بود تمدید یا ارتقاشون بدی 
👇 لیست سرویس‌های شما:" },
                new BotMessage { Id = (int)BotCommand.ExtraBandwidth, Command = BotCommand.ExtraBandwidth, Message = @"🔋 افزایش حجم سرویس

سرویس انتخاب‌شده: <TITLE> ✅
حالا فقط کافیه مقدار حجمی که نیاز داری رو انتخاب کنی 👇

📦 هر چقدر بیشتر، اتصال طولانی‌تر و بی‌دردسرتر!" },
                new BotMessage { Id = (int)BotCommand.Plans, Command = BotCommand.Plans, Message = "TextMessage-PlansMessage" },
                new BotMessage { Id = (int)BotCommand.Wallet, Command = BotCommand.Wallet, Message = "پیام کیف پول - نمایش بالانس" },
                new BotMessage { Id = (int)BotCommand.Support, Command = BotCommand.Support, Message = "پیام پشتیبانی - نمایش ایدی اکانت پشتیبانی" },
                new BotMessage { Id = (int)BotCommand.Help, Command = BotCommand.Help, Message = "پیام راهنما" },
                new BotMessage { Id = (int)BotCommand.MyServiceDetails, Command = BotCommand.MyServiceDetails, Message = @"🧾 جزئیات سرویس شما

📌 نام نمایشی: <TITLE>
🌐 سرویس: <SERVICE>
📶 وضعیت: <STATUS>
📝 یادداشت: <NOTE>

✨ برای مدیریت سرویس‌ات میتونی از دکمه های زیر استفاده کنی" },
                new BotMessage { Id = (int)BotCommand.BuyBandwidth, Command = BotCommand.BuyBandwidth, Message = @"سرویس انتخاب شده - وارد کردن حجم" },
                new BotMessage { Id = (int)BotCommand.RenewMyService, Command = BotCommand.RenewMyService, Message = "سرویس انتخاب شده جهت تمدید - تعداد ماه" },
                new BotMessage { Id = (int)BotCommand.ChargeWallet, Command = BotCommand.ChargeWallet, Message = "شارژ ولت انتخاب شده - وارد کردن مبلغ" },
                new BotMessage { Id = (int)BotCommand.PaymentMethod, Command = BotCommand.PaymentMethod, Message = @"مبلغ وارد شده: <AMOUNT>
روش پرداخت را انتخاب کنید:" },
                new BotMessage { Id = (int)BotCommand.CardToCard, Command = BotCommand.CardToCard, Message = @"روش پرداخت: کارت به کارت
مبلغ: <code><AMOUNT></code>
شماره کارت: <code><CARD></code>
به شماره کارت بالا واریز کنید و رسید بفرستید" },
                new BotMessage { Id = (int)BotCommand.BuyServiceCallback, Command = BotCommand.BuyServiceCallback, Message = @"سرویس انتخاب شده: <NAME>
انتخاب مدت زمان سرویس:" },
                new BotMessage { Id = (int)BotCommand.SelectMonthCallback, Command = BotCommand.SelectMonthCallback, Message = @"سرویس انتخاب شده: <NAME>
مدت زمان سرویس: <MONTH>
انتخاب ترافیک:" },
                new BotMessage { Id = (int)BotCommand.SelectTrafficCallback, Command = BotCommand.SelectTrafficCallback, Message = @"سرویس انتخاب شده: <NAME>
مدت زمان سرویس: <MONTH>
مقدار ترافیک: <TRAFFIC>
مبلغ نهایی: <PRICE>
فاکتور ساخته شد ، انتخاب روش پرداخت: " },
                new BotMessage { Id = (int)BotCommand.SubscriptionDetails, Command = BotCommand.SubscriptionDetails, Message = @"نام نمایشی: <TITLE>
سرویس: <SERVICE>
وضعیت: <STATUS>
یادداشت: <NOTE>
تاریخ شروع سرویس: <CREATE>
تاریخ انقضا: <EXPIRE>
حجم مصرف شده: <USED> گیگ از <BANDWIDTH> گیگ
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

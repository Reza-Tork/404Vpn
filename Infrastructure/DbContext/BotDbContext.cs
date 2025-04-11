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

        public BotDbContext(DbContextOptions<BotDbContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
                new BotMessage { Id = (int)BotCommand.Start, Command = BotCommand.Start, Message = "پیام کامند استارت" },
                new BotMessage { Id = (int)BotCommand.MainMenu, Command = BotCommand.MainMenu, Message = "به منوی اصلی بازگشتید" },
                new BotMessage { Id = (int)BotCommand.BuyService, Command = BotCommand.BuyService, Message = "پیام خرید سرویس - مرحله انتخاب سرویس" },
                new BotMessage { Id = (int)BotCommand.RenewService, Command = BotCommand.RenewService, Message = "پیام تمدید سرویس - مرحله انتخاب سرویس جهت تمدید" },
                new BotMessage { Id = (int)BotCommand.MyServices, Command = BotCommand.MyServices, Message = "پیام سرویس های من - مرحله نمایش سرویس ها" },
                new BotMessage { Id = (int)BotCommand.ExtraBandwidth, Command = BotCommand.ExtraBandwidth, Message = "حجم اضافه - انتخاب سرویس جهت افزودن حجم" },
                new BotMessage { Id = (int)BotCommand.Plans, Command = BotCommand.Plans, Message = "پیام پلن ها - نمایش تمام پلن ها" },
                new BotMessage { Id = (int)BotCommand.Wallet, Command = BotCommand.Wallet, Message = "پیام کیف پول - نمایش بالانس" },
                new BotMessage { Id = (int)BotCommand.Support, Command = BotCommand.Support, Message = "پیام پشتیبانی - نمایش ایدی اکانت پشتیبانی" },
                new BotMessage { Id = (int)BotCommand.Help, Command = BotCommand.Help, Message = "پیام راهنما" },
                new BotMessage { Id = (int)BotCommand.MyServiceDetails, Command = BotCommand.MyServiceDetails, Message = @"نام سرویس: <TITLE>
حجم خریداری شده: <BUYBAND>
حجم باقیمانده: <REMAINBAND>
تاریخ انقضا سرویس: <EXPIRE>" },
                new BotMessage { Id = (int)BotCommand.BuyBandwidth, Command = BotCommand.BuyBandwidth, Message = @"سرویس انتخاب شده - وارد کردن حجم" },
                new BotMessage { Id = (int)BotCommand.RenewMyService, Command = BotCommand.RenewMyService, Message = "سرویس انتخاب شده جهت تمدید - تعداد ماه" },
                new BotMessage { Id = (int)BotCommand.ChargeWallet, Command = BotCommand.ChargeWallet, Message = "شارژ ولت انتخاب شده - وارد کردن مبلغ" },
                new BotMessage { Id = (int)BotCommand.PaymentMethod, Command = BotCommand.PaymentMethod, Message = @"مبلغ وارد شده: <AMOUNT>
روش پرداخت را انتخاب کنید:" },
                new BotMessage { Id = (int)BotCommand.CardToCard, Command = BotCommand.CardToCard, Message = @"روش پرداخت: کارت به کارت
مبلغ: <code><AMOUNT></code>
شماره کارت: <code><CARD></code>
به شماره کارت بالا واریز کنید و رسید بفرستید" },

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

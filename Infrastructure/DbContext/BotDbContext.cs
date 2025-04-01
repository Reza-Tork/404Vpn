using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Bot;
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
                }
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

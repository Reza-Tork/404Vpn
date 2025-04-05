using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Vpn;

namespace Domain.Entities.Bot
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public long UserId { get; set; }
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime JoinDate { get; set; } = DateTime.UtcNow;
        public Step Step { get; set; } = Step.None;
        public string? StepData { get; set; }
        public Wallet Wallet { get; set; }
        public Admin? Admin { get; set; }
        public Discount? Discount { get; set; }
        public ICollection<UserSubscription> UserSubscriptions { get; set; } = [];

        public bool IsAdmin() => Admin != null;

        public bool HasService() => UserSubscriptions != null && UserSubscriptions.Any();
    }

    public enum Step
    {
        None
    }
}

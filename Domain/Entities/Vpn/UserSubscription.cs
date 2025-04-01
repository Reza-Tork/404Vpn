using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Bot;

namespace Domain.Entities.Vpn
{
    public class UserSubscription
    {
        public int Id { get; set; }

        public string Username { get; set; }
        public int Bandwidth { get; set; }
        public DateTime ExpireTime { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.UtcNow;
        public Status Status { get; set; } = Status.Pending;
        
        public int UserId { get; set; }
        public User User { get; set; }
        
        public int ServiceId { get; set; }
        public Service Service { get; set; }

        public bool IsActive() => ExpireTime > DateTime.UtcNow && Status == Status.Active;
    }
    public enum Status
    {
        Active,
        Pending,
        Suspend
    }
}

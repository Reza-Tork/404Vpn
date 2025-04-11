using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Vpn;

namespace Domain.Entities.Bot
{
    public class Factor
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Amount { get; set; }
        public FactorState State { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public User User { get; set; }
    }
    public enum FactorState
    {
        Pending,
        Expired,
        Confirmed,
        Rejected
    }
}

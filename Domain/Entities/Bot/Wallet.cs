using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Bot
{
    public class Wallet
    {
        public int Id { get; set; }
        public int Balance { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<TransactionHistory> TransactionHistories { get; set; }
    }
}

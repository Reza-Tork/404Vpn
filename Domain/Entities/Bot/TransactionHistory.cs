using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Bot
{
    public class TransactionHistory
    {
        public int Id { get; set; }
        public Type Type { get; set; }
        public int Value { get; set; }

        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }
    }

    public enum Type
    {
        AddBalance,
        BuyService
    }
}

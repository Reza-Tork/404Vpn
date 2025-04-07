using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Bot;

namespace Domain.Entities.Vpn
{
    public class Service
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public int Index { get; set; }
        public int Capacity { get; set; } = -1;
        public bool IsActive { get; set; } = true;
        public long Price { get; set; }
        public ICollection<UserSubscription> UsersSubscriptions { get; set; } = [];
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Bot
{
    public class Discount
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public int DiscountPercent { get; set; }
        public int Capacity { get; set; } = 0;
        public DateTime ExpireDate { get; set; }
        public ICollection<User> Users { get; set; }
    }
}

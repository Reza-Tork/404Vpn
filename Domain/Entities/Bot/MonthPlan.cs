using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Bot
{
    public class MonthPlan
    {
        public int Id { get; set; }
        public int Month { get; set; }
        public int PricePerMonth { get; set; }
        public ICollection<TrafficPlan> TrafficPlans { get; set; } = [];
    }
}

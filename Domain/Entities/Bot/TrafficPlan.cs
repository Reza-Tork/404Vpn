using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Bot
{
    public class TrafficPlan
    {
        public int Id { get; set; }
        public int Bandwidth { get; set; }
        public int PricePerGb { get; set; }

        public int MonthPlanId { get; set; }
        public MonthPlan MonthPlan { get; set; }
    }
}

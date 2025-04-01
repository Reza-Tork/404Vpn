using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Vpn
{
    public class ApiInfo
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

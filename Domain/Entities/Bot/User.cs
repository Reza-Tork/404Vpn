using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public DateTime JoinDate { get; set; }
        public Admin? Admin { get; set; }
        public bool IsAdmin()
        {
            return Admin != null;
        }
    }
}

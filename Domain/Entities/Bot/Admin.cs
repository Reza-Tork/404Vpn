using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Bot
{
    public class Admin
    {
        public int Id { get; set; }
        public int UserId { get; set; }
<<<<<<< HEAD
=======
        public AdminStep Step { get; set; }
>>>>>>> Initial Project

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
    public enum AdminStep
    {
        None
    }
}

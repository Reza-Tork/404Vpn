using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Domain.DTOs.Marzban.Requests
{
    public class LoginRequestDTO
    {
        private string grant_type { get; set; } = "password";

        public string username { get; set; }

        public string password { get; set; }
    }
}

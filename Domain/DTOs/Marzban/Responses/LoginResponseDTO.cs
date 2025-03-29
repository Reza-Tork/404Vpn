using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.DTOs.Marzban.Responses
{
    public class LoginResponseDTO
    {
        [JsonPropertyName("access_token")]
        public string Token { get; set; }

        private string token_type { get; set; }
    }

    public class LoginResponseErrorDTO
    {
        [JsonPropertyName("detail")]
        public string Message { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class StringHelpers
    {
        public static string GenerateUsername(int length = 10)
        {
            return "";
        }
        public static string ConvertToFormUrlEncoded(this object obj)
        {
            var properties = obj.GetType().GetProperties();
            var keyValuePairs = new List<string>();

            foreach (var prop in properties)
            {
                var value = prop.GetValue(obj)?.ToString();
                if (!string.IsNullOrEmpty(value))
                {
                    keyValuePairs.Add($"{prop.Name}={Uri.EscapeDataString(value)}");
                }
            }

            return string.Join("&", keyValuePairs);
        }
    }
}

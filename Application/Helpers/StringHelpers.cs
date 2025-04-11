using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class StringHelpers
    {
        public static string GenerateUsername(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            string randomPart = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return $"404Vpn_{randomPart}";
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
        public static string ToPersianDate(this DateTime date)
        {
            TimeZoneInfo iranTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time");
            DateTime iranTime = TimeZoneInfo.ConvertTime(date, iranTimeZone);

            PersianCalendar pc = new PersianCalendar();

            string[] persianDays = { "یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنج‌شنبه", "جمعه", "شنبه" };
            string[] persianMonths = {
            "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
            "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
        };

            string dayOfWeek = persianDays[(int)pc.GetDayOfWeek(iranTime)];
            int day = pc.GetDayOfMonth(iranTime);
            string month = persianMonths[pc.GetMonth(iranTime) - 1];
            int hour = iranTime.Hour;
            int minute = iranTime.Minute;
            return $"{dayOfWeek} {day} {month} ساعت {hour:D2}:{minute:D2}";
        }
    }
}

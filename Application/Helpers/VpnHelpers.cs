using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class VpnHelpers
    {
        public static long GbToByte(int value)
        {
            return (long)value * 1024 * 1024 * 1024;
        }
        public static string ByteToPersianUnit(long bytes)
        {
            const long KB = 1024;
            const long MB = KB * 1024;
            const long GB = MB * 1024;
            const long TB = GB * 1024;

            if (bytes >= TB)
                return $"{(bytes / (double)TB):0.##} ترابایت";
            if (bytes >= GB)
                return $"{(bytes / (double)GB):0.##} گیگابایت";
            if (bytes >= MB)
                return $"{(bytes / (double)MB):0.##} مگابایت";
            if (bytes >= KB)
                return $"{(bytes / (double)KB):0.##} کیلوبایت";

            return $"{bytes} بایت";
        }

        public static long ToTimestamp(this DateTime time)
        {
            return ((DateTimeOffset)time).ToUnixTimeSeconds();
        }
    }
}

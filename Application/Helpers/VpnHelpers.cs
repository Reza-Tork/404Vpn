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

        public static long ToTimestamp(this DateTime time)
        {
            return ((DateTimeOffset)time).ToUnixTimeSeconds();
        }
    }
}

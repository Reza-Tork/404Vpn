using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class VpnHelpers
    {
        public static int GbToByte(int value)
        {
            return value * 1073741824;
        }
        public static long ToTimestamp(this DateTime time)
        {
            return ((DateTimeOffset)time).ToUnixTimeSeconds();
        }
    }
}

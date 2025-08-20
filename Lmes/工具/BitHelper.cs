using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.工具
{
    public static class BitHelper
    {
        public static bool 获取bit位<T>(this T value, int 位) where T : struct, IConvertible
        {
            long longValue = Convert.ToInt64(value);
            return (longValue & (1L << 位)) != 0;
        }
    }
}

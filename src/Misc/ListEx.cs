using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if NO_CONCURRENT
namespace SQLite.Extensions
{
    public static class ListEx
    {
        public static bool TryAdd<TKey, TValue> (this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            try {
                dict.Add (key, value);
                return true;
            }
            catch (ArgumentException) {
                return false;
            }
        }
    }
}
#endif

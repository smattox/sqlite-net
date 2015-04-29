using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns.PropertyCollection
{
    public static class PropertyCollectorFactory
    {
        public static PropertyCollector Create()
        {
#if USE_NEW_REFLECTION_API
            return new NewAPIPropertyCollector();
#else
            return new OldAPIPropertyCollector();
#endif
        }
    }
}

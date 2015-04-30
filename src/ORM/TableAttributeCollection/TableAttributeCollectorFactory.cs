using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.TableAttributeCollection
{
    public static class TableAttributeCollectorFactory
    {
        public static TableAttributeCollector Create()
        {
#if USE_NEW_REFLECTION_API
            return new NewReflectionAPITableAttributeCollector();
#else
            return new OldReflectionAPITableAttributeCollector();
#endif
        }
    }
}

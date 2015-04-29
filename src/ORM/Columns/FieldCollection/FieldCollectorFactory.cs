using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns.FieldCollection
{
    public static class FieldCollectorFactory
    {
        public static FieldCollector Create()
        {
#if USE_NEW_REFLECTION_API
            return new NewAPIFieldCollector();
#else
            //TODO: Write OldAPI; used for WinPhone?
#endif
        }
    }
}

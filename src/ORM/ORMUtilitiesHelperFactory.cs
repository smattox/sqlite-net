using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM
{
    public static class ORMUtilitiesHelperFactory
    {
        public static ORMUtilitiesHelper Create()
        {
#if USE_NEW_REFLECTION_API
            return new NewAPIORMUtilitiesHelper();
#else
            return new OldAPIORMUtilitiesHelper();
#endif
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns.PropertyAttributeChecker
{
    public static class PropertyAttributeCheckerFactory
    {
        public static IPropertyAttributeChecker Create()
        {
#if !USE_NEW_REFLECTION_API
            return new OldAPIPropertyAttributeChecker();
#else
            return new NewAPIPropertyAttributeChecker();
#endif
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns.PropertyAttributeChecker
{
    public class PropertyAttributeCheckerFactory
    {
        public IPropertyAttributeChecker Create()
        {
#if !USE_NEW_REFLECTION_API
            return new OldAPIPropertyAttributeChecker();
#else
            return new NewAPIPropertyAttributeChecker();
#endif
        }
    }
}

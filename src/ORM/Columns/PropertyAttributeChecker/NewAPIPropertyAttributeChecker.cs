using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns.PropertyAttributeChecker
{
    public class NewAPIPropertyAttributeChecker : PropertyAttributeChecker
    {
        public bool PropertyHasAttribute(PropertyInfo property, Type attribute)
        {
#if USE_NEW_REFLECTION_API
            return property.GetCustomAttributes(attribute, true).Count() > 0;
#else
            return false
#endif
        }
    }
}

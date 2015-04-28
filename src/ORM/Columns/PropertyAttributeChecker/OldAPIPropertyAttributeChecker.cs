using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns.PropertyAttributeChecker
{
    public class OldAPIPropertyAttributeChecker : PropertyAttributeChecker
    {
        public bool PropertyHasAttribute(PropertyInfo property, Type attribute)
        {
#if !USE_NEW_REFLECTION_API
            return propertyInfo.GetCustomAttributes(attribute, true).Length > 0;
#else
            return false;
#endif
        }
    }
}

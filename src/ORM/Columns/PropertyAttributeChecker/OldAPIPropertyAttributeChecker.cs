using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

#if !USE_NEW_REFLECTION_API
namespace SQLite.ORM.Columns.PropertyAttributeChecker
{
    public class OldAPIPropertyAttributeChecker : IPropertyAttributeChecker
    {
        public bool PropertyHasAttribute(PropertyInfo property, Type attribute)
        {

            return propertyInfo.GetCustomAttributes(attribute, true).Length > 0;
        }
    }
}
#endif
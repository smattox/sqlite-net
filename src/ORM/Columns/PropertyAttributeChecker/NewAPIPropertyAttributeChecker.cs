using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

#if USE_NEW_REFLECTION_API
namespace SQLite.ORM.Columns.PropertyAttributeChecker
{
    public class NewAPIPropertyAttributeChecker : IPropertyAttributeChecker
    {
        public bool PropertyHasAttribute(PropertyInfo property, Type attribute)
        {

            return property.GetCustomAttributes(attribute, true).Count() > 0;
        }
    }
}
#endif
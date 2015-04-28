using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns.PropertyCollection
{
    public class OldAPIPropertyCollector : PropertyCollector
    {
        public PropertyInfo[] Collect(Type type)
        {
#if !USE_NEW_REFLECTION_API
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
#else
            return new PropertyInfo[0];
#endif
        }
    }
}

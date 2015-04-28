using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns.PropertyCollection
{
    public class NewAPIPropertyCollector : PropertyCollector
    {
        public PropertyInfo[] Collect(Type type)
        {
#if USE_NEW_REFLECTION_API
            var properties = from p in type.GetRuntimeProperties()
                   where ((p.GetMethod != null && p.GetMethod.IsPublic) || (p.SetMethod != null && p.SetMethod.IsPublic) || (p.GetMethod != null && p.GetMethod.IsStatic) || (p.SetMethod != null && p.SetMethod.IsStatic))
                   select p;
            return properties.ToArray();
#else
            return new PropertyInfo[0];
#endif
        }
    }
}

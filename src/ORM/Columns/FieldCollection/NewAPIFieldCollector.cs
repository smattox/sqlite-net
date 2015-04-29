using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns.FieldCollection
{
    public class NewAPIFieldCollector : FieldCollector
    {
        public FieldInfo[] Collect(Type type)
        {
#if USE_NEW_REFLECTION_API
            var fields = new List<FieldInfo>();
            foreach (FieldInfo field in type.GetRuntimeFields())
            {
                if (field.IsPublic)
                {
                    fields.Add(field);
                }
            }
            return fields.ToArray();
#else
            return new FieldInfo[0];
#endif
        }
    }
}

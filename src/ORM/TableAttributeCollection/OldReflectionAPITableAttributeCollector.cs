using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.TableAttributeCollection
{
    public class OldReflectionAPITableAttributeCollector : TableAttributeCollector
    {
        public TableAttribute GetAttributesForType(Type type)
        {
#if !USE_NEW_REFLECTION_API
            return (TableAttribute)type.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault();
#else
            return null;
#endif
        }
    }
}

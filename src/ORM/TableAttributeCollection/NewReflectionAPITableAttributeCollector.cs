using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace SQLite.ORM.TableAttributeCollection
{
    public class NewReflectionAPITableAttributeCollector : TableAttributeCollector
    {
        public TableAttribute GetAttributesForType(Type type)
        {
#if USE_NEW_REFLECTION_API
            return (TableAttribute)System.Reflection.CustomAttributeExtensions
                .GetCustomAttribute(type.GetTypeInfo(), typeof(TableAttribute), true);
#else
            return null;
#endif
        }
    }
}

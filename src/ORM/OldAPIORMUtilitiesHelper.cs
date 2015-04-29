using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if !USE_NEW_REFLECTION_API
namespace SQLite.ORM
{
    class OldAPIORMUtilitiesHelper : ORMUtilitiesHelper
    {
        public T GetAttribute<T>(MemberInfo info) where T : Attribute
        {
            var attrs = info.GetCustomAttributes(typeof(T), true);

            if (attrs.Length > 0)
                return attrs[0] as T;
            
            return null;
        }

        public bool IsEnum(Type type)
        {
            return type.IsEnum;
        }
    }
}
#endif

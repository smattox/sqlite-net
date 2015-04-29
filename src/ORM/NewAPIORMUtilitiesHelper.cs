using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

#if USE_NEW_REFLECTION_API
namespace SQLite.ORM
{
    class NewAPIORMUtilitiesHelper : ORMUtilitiesHelper
    {
        public T GetAttribute<T>(MemberInfo info) where T : Attribute
        {
            var attrs = info.GetCustomAttributes(typeof(T), true);
			if (attrs.Count() > 0)
				return attrs.First() as T;
            return null;
        }

        public bool IsEnum(Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }
    }
}
#endif
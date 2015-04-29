using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM
{
    public interface ORMUtilitiesHelper
    {
        T GetAttribute<T>(MemberInfo info) where T : Attribute;

        bool IsEnum(Type type);
    }
}

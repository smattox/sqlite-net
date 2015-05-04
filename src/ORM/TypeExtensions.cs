using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SQLite.ORM
{
    public static class TypeExtensions
    {
        public static string GetSimpleAssemblyName(this Type type)
        {
            return Regex.Replace(type.AssemblyQualifiedName, ",.+\\z", "");
        }
    }
}

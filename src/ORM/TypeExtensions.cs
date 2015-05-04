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
            string asmName = type.AssemblyQualifiedName;
            var matches = Regex.Matches(asmName, ",.+\\z");
            return Regex.Replace(asmName, ",.+\\z", "");
        }
    }
}

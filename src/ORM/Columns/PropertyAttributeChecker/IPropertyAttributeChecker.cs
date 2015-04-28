using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns.PropertyAttributeChecker
{
    public interface IPropertyAttributeChecker
    {
        bool PropertyHasAttribute(PropertyInfo property, Type attribute);
    }
}

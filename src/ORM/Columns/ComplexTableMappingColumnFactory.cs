using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns
{
    public class ComplexTableMappingColumnFactory : TableMappingColumnFactory
    {
        private BasicTableMappingColumnFactory simpleTypesFactory = new BasicTableMappingColumnFactory();

        public TableMappingColumn[] CreateColumnsOnMember(MemberInfo info, CreateFlags flags)
        {
            Type targetType = null;
            if (info is PropertyInfo) targetType = (info as PropertyInfo).PropertyType;
            if (info is FieldInfo) targetType = (info as FieldInfo).FieldType;
            if (info == null) throw new InvalidOperationException("What is " + info.Name + "?");

            if (ORMUtilities.IsSimpleSQLType(targetType))
            {
                return simpleTypesFactory.CreateColumnsOnMember(info, flags);
            }
            return null;
        }
    }
}

using System;
using System.Collections;
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

        public TableMappingColumn[] CreateColumnsOnMember(MemberInfo info, TableMappingConfiguration configuration, CreateFlags flags, string path)
        {
            Type targetType = null;
            if (info is PropertyInfo) targetType = (info as PropertyInfo).PropertyType;
            if (info is FieldInfo) targetType = (info as FieldInfo).FieldType;
            if (info == null) throw new InvalidOperationException("What is " + info.Name + "?");

            path += path.Length > 0 ? "." : "";
            if (ORMUtilities.IsSimpleSQLType(targetType))
            {
                return simpleTypesFactory.CreateColumnsOnMember(info, configuration, flags, path);
            }

            path += ORMUtilities.GetColumnName(info);

            return ORMUtilities.GetColumnsOnType(targetType, configuration, flags, path);
        }
    }
}

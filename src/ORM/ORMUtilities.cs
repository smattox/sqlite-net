using SQLite.ORM.Columns;
using SQLite.ORM.TableAttributeCollection;
using SQLite.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM
{
    public static class ORMUtilities
    {
        public const int DefaultMaxStringLength = 140;
        public const string ImplicitPkName = "Id";
        public const string ImplicitIndexSuffix = "Id";

        public static TableMappingColumn[] GetColumnsOnType(Type type, TableMappingConfiguration configuration, CreateFlags createFlags, string path)
        {
            var tableColumns = new List<TableMappingColumn>();
            List<MemberInfo> eligibleMembers = new List<MemberInfo>();
            eligibleMembers.AddRange(configuration.PropertyCollector.Collect(type));
            eligibleMembers.AddRange(configuration.FieldCollector.Collect(type));

            foreach (var info in eligibleMembers)
            {
                tableColumns.AddRange(configuration.TableMappingColumnFactory.CreateColumnsOnMember(info, configuration, createFlags, path));
            }

            return tableColumns.ToArray();
        }

        public static string SqlDecl(TableMappingColumn p, bool storeDateTimeAsTicks)
        {
            string decl = "\"" + p.Name + "\" " + SqlType(p, storeDateTimeAsTicks) + " ";

            if (p.IsPK)
            {
                decl += "primary key ";
            }
            if (p.IsAutoInc)
            {
                decl += "autoincrement ";
            }
            if (!p.IsNullable)
            {
                decl += "not null ";
            }
            if (!string.IsNullOrEmpty(p.Collation))
            {
                decl += "collate " + p.Collation + " ";
            }

            return decl;
        }

        public static string SqlType(TableMappingColumn column, bool storeDateTimeAsTicks = false)
        {
            return SqlType(column.TargetType, column.MaxStringLength, storeDateTimeAsTicks);
        }

        public static string SqlType(Type clrType, int? maxLength = DefaultMaxStringLength, bool storeDateTimeAsTicks = false)
        {
            var type = SQLite3.GetSQLiteType(clrType);

            if (type != null)
            {
                return type.GetSQL(maxLength, storeDateTimeAsTicks);
            }
            throw new NotSupportedException("Don't know about " + clrType);
        }

        public static bool IsSimpleSQLType(Type clrType)
        {
            return SQLite3.GetSQLiteType(clrType) != null;
        }

        public static string GetTableName(Type type)
        {
            var tableAttr = TableAttributeCollectorFactory.Create().GetAttributesForType(type);
            return tableAttr != null ? tableAttr.Name : type.Name;
        }

        public static string GetColumnName(MemberInfo info)
        {
            var colAttr = (ColumnAttribute)info.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault();

            return (colAttr == null ? info.Name : colAttr.Name);
        }

        public static bool IsPrimaryKey(MemberInfo info)
        {
            return ORMUtilitiesHelperFactory.Create().GetAttribute<PrimaryKeyAttribute>(info) != null;
        }

        public static string Collation(MemberInfo info)
        {
            var attribute = ORMUtilitiesHelperFactory.Create().GetAttribute<CollationAttribute>(info);
            if (attribute != null)
            {
                return attribute.Value;
            }
            else
            {
                return string.Empty;
            }
        }

        public static bool IsAutoInc(MemberInfo info)
        {
            return ORMUtilitiesHelperFactory.Create().GetAttribute<AutoIncrementAttribute>(info) != null;
        }

        public static IEnumerable<IndexedAttribute> GetIndices(MemberInfo info)
        {
            var attrs = info.GetCustomAttributes(typeof(IndexedAttribute), true);
            return attrs.Cast<IndexedAttribute>();
        }

        public static int? MaxStringLength(MemberInfo info)
        {
            var attribute = ORMUtilitiesHelperFactory.Create().GetAttribute<MaxLengthAttribute>(info);
            if (attribute != null)
            {
                return attribute.Value;
            }
            return null;
        }

        public static bool IsMarkedNotNull(MemberInfo info)
        {
            return ORMUtilitiesHelperFactory.Create().GetAttribute<NotNullAttribute>(info) != null;
        }
    }
}

using SQLite.ORM.Columns;
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

        private static string DATETIME = "datetime";
        private static string INTEGER = "integer";
        private static string BIG_INT = "bigint";
        private static string VAR_CHAR = "varchar";
        private static string FLOAT = "float";
        private static string BLOB = "blob";

        private static SqlMapper[] StandardTypes = {
            new SqlMapper(clrType => clrType == typeof(Boolean) || clrType == typeof(Byte) || clrType == typeof(UInt16) || clrType == typeof(SByte) || clrType == typeof(Int16) || clrType == typeof(Int32) || clrType == typeof(UInt32) || clrType == typeof(Int64), INTEGER),
            new SqlMapper(clrType => clrType == typeof(Single) || clrType == typeof(Double) || clrType == typeof(Decimal), FLOAT),
            new SqlMapper(clrType => clrType == typeof(String), (len, date) => VAR_CHAR + (len.HasValue ? "(" + len.Value + ")" : "")),
            new SqlMapper(clrType => clrType == typeof(TimeSpan), BIG_INT),
            new SqlMapper(clrType => clrType == typeof(DateTime), (len, storeDateTimeAsTicks) => storeDateTimeAsTicks ? BIG_INT : DATETIME),
            new SqlMapper(clrType => clrType == typeof(DateTimeOffset), BIG_INT),
            new SqlMapper(clrType => ORMUtilitiesHelperFactory.Create().IsEnum(clrType), INTEGER),
            new SqlMapper(clrType => clrType == typeof(byte[]), BLOB),
            new SqlMapper(clrType => clrType == typeof(Guid), VAR_CHAR + "(36)")
        };

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
            var type = StandardTypes.FirstOrDefault(map => map.Satisfies(clrType));

            if (type != null)
            {
                return type.GetSQL(maxLength, storeDateTimeAsTicks);
            }
            throw new NotSupportedException("Don't know about " + clrType);
        }

        public static bool IsSimpleSQLType(Type clrType)
        {
            return StandardTypes.FirstOrDefault(map => map.Satisfies(clrType)) != null;
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

        private class SqlMapper
        {
            private Func<Type, bool> Condition;
            private Func<int?, bool, string> SQL;

            public SqlMapper(Func<Type, bool> condition, string sql) :
                this(condition, (len, date) => sql)
            {
            }

            public SqlMapper(Func<Type, bool> condition, Func<int?, bool, string> sql)
            {
                Condition = condition;
                SQL = sql;
            }

            public bool Satisfies(Type type)
            {
                return Condition.Invoke(type);
            }

            public string GetSQL(int? maxLen, bool storeDateTimeAsTicks)
            {
                return SQL.Invoke(maxLen, storeDateTimeAsTicks);
            }
        }
    }
}

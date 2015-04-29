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

        public static string SqlType(TableMappingColumn p, bool storeDateTimeAsTicks)
        {
            var clrType = p.TargetType;
            if (clrType == typeof(Boolean) || clrType == typeof(Byte) || clrType == typeof(UInt16) || clrType == typeof(SByte) || clrType == typeof(Int16) || clrType == typeof(Int32) || clrType == typeof(UInt32) || clrType == typeof(Int64))
            {
                return "integer";
            }
            else if (clrType == typeof(Single) || clrType == typeof(Double) || clrType == typeof(Decimal))
            {
                return "float";
            }
            else if (clrType == typeof(String))
            {
                int? len = p.MaxStringLength;

                if (len.HasValue)
                    return "varchar(" + len.Value + ")";

                return "varchar";
            }
            else if (clrType == typeof(TimeSpan))
            {
                return "bigint";
            }
            else if (clrType == typeof(DateTime))
            {
                return storeDateTimeAsTicks ? "bigint" : "datetime";
            }
            else if (clrType == typeof(DateTimeOffset))
            {
                return "bigint";
            } else if (ORMUtilitiesHelperFactory.Create().IsEnum(clrType)) {
                return "integer";
            }
            else if (clrType == typeof(byte[]))
            {
                return "blob";
            }
            else if (clrType == typeof(Guid))
            {
                return "varchar(36)";
            }
            else
            {
                throw new NotSupportedException("Don't know about " + clrType);
            }
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

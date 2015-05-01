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
            if (clrType == null)
            {
                return null;
            }

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

        public static object InstantiateMember(object obj, string name)
        {
            MemberInfo info = null;
            Type memberType = null;

            var property = obj.GetType().GetRuntimeProperty(name);
            if (property != null)
            {
                info = property;
                memberType = property.PropertyType;
            }

            var field = obj.GetType().GetRuntimeField(name);
            if (field != null)
            {
                info = field;
                memberType = field.FieldType;
            }

            if (memberType != null)
            {
                var newInstance = Activator.CreateInstance(memberType);
                SetMemberValue(obj, info.Name, newInstance);
                return newInstance;
            }
            throw new InvalidOperationException("Don't recognize member " + name + " on " + obj.GetType().FullName);
        }

        public static object GetMemberValue(object obj, string name)
        {
            var property = obj.GetType().GetRuntimeProperty(name);
            if (property != null)
                return property.GetValue(obj);

            var field = obj.GetType().GetRuntimeField(name);
            if (field != null)
                return field.GetValue(obj);

            throw new InvalidOperationException(obj.GetType().FullName + " has no member " + name);
        }

        public static void SetMemberValue(object obj, string name, object value)
        {
            var property = obj.GetType().GetRuntimeProperty(name);
            if (property != null)
            {
                property.SetValue(obj, value);
                return;
            }

            var field = obj.GetType().GetRuntimeField(name);
            if (field != null)
            {
                field.SetValue(obj, value);
                return;
            }

            throw new InvalidOperationException(obj.GetType().FullName + " has no member " + name);
        }

        public static object GetValueFromMember(MemberInfo info, object obj)
        {
            if (info is PropertyInfo)
            {
                return (info as PropertyInfo).GetValue(obj);
            }
            if (info is FieldInfo)
            {
                return (info as FieldInfo).GetValue(obj);
            }
            throw new InvalidOperationException("Confusion; " + info.Name + " is neither property or field.");
        }

        public static long GetPrimaryKey(object target, TableMappingConfiguration configuration)
        {
            var properties = configuration.PropertyCollector.Collect(target.GetType());
            var fields = configuration.FieldCollector.Collect(target.GetType());

            List<MemberInfo> infoList = new List<MemberInfo>();
            infoList.AddRange(properties);
            infoList.AddRange(fields);

            MemberInfo primaryKeyInfo = infoList.FirstOrDefault(info => ORMUtilitiesHelperFactory.Create().GetAttribute<PrimaryKeyAttribute>(info) != null);

            if (primaryKeyInfo == null)
                throw new InvalidOperationException("Parent type of list must have a primary key.");

            var result = ORMUtilities.GetValueFromMember(primaryKeyInfo, target);
            return Convert.ToInt64(result);
        }
    }
}

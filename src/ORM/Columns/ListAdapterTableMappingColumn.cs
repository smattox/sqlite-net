using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns
{
    public class ListAdapterTableMappingColumn : AbstractTableMappingColumn, IndirectTableMappingColumn
    {
        private string listName;
        private Type listType;
        private Type listContainerType;
        private string targetTableContext;
        private string targetTableName;

        private SQLiteConnection connection;
        private string path;

        public ListAdapterTableMappingColumn(MemberInfo info, Type targetType, string path, SQLiteConnection connection, CreateFlags createFlags = CreateFlags.None)
            : base(info, path, createFlags)
        {
            this.connection = connection;
            this.path = path;
            this.listName = info.Name;

            listType = targetType.GenericTypeArguments[0];
            listContainerType = typeof(ListContainer<>).MakeGenericType(listType);
            targetTableContext = info.DeclaringType.GetSimpleAssemblyName();
            targetTableName = targetTableContext + '.' + listContainerType.GetSimpleAssemblyName();

            connection.CreateTable(listContainerType, targetTableContext, createFlags);
        }

        public object LoadValueFromDatabase(object target)
        {
            var baseMethod = this.GetType().GetRuntimeMethods().First(mi => mi.Name.Equals("LoadValue"));
            var method = baseMethod.MakeGenericMethod(listType);
            return method.Invoke(this, new object[] { target });
        }

        private object LoadValue<T>(object target) where T : new()
        {
            long keyValue = GetPrimaryKey(target);
            var query = connection.Table<ListContainer<T>>(targetTableContext);
            return query.Where(container => container.MasterKey == keyValue).ToList();
        }

        public void SaveValueToDatabase(object target)
        {
            var baseMethod = this.GetType().GetRuntimeMethods().First(mi => mi.Name.Equals("SaveValue"));
            var method = baseMethod.MakeGenericMethod(listType);
            method.Invoke(this, new object[] { target });
        }

        private void SaveValue<T>(object target)
        {
            long keyValue = GetPrimaryKey(target);
            target = GetTargetObject(target);
            List<T> list = ORMUtilities.GetMemberValue(target, listName) as List<T>;
            IEnumerable<ListContainer<T>> items = list.Select(item => new ListContainer<T>(keyValue, item));

            // TODO: SetData is inappropriate here, obviously. We want to 'set' only the items
            // with a particular key
            connection.SetData<ListContainer<T>>(items, targetTableContext, true, false);
        }

        private long GetPrimaryKey(object target)
        {
            return ORMUtilities.GetPrimaryKey(target, connection.TableMappingConfiguration);
        }

        private class ListContainer<T>
        {
            public long MasterKey;

            public T Data;

            public ListContainer() { }

            public ListContainer(long key, T data)
            {
                MasterKey = key;
                Data = data;
            }
        }
    }
}

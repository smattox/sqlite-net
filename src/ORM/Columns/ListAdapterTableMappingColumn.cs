using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns
{
    public class ListAdapterTableMappingColumn : AbstractTableMappingColumn
    {
        private Type listType;
        private Type listContainerType;

        private SQLiteConnection connection;
        private string path;

        public ListAdapterTableMappingColumn(MemberInfo info, Type targetType, string path, SQLiteConnection connection, CreateFlags createFlags = CreateFlags.None)
            : base(info, path, createFlags)
        {
            this.connection = connection;
            this.path = path;

            listType = targetType.GenericTypeArguments[0];

            listContainerType = typeof(ListContainer<>).MakeGenericType(listType);

            connection.CreateTable(listContainerType, path.TrimEnd(new char[] { '.' }), createFlags);
        }

        public override bool IsDirectWrite
        {
            get
            {
                return false;
            }
        }

        public override object GetValue(object target)
        {
            var baseMethod = this.GetType().GetRuntimeMethods().First(mi => mi.Name.Equals("GetTargetValue"));
            var method = baseMethod.MakeGenericMethod(listType);
            return method.Invoke(this, new object[] { target });
        }

        private object GetTargetValue<T>(object target) where T : new()
        {
            long keyValue = GetPrimaryKey(target);
            var query = connection.Table<ListContainer<T>>();
            return query.Where(container => container.MasterKey == keyValue);
        }

        public override void SetValue(object target, object value)
        {
            var baseMethod = this.GetType().GetRuntimeMethods().First(mi => mi.Name.Equals("SetTargetValue"));
            var method = baseMethod.MakeGenericMethod(listType);
            method.Invoke(this, new object[] { target, value });
        }

        private void SetTargetValue<T>(object target, object value)
        {
            long keyValue = GetPrimaryKey(target);
            IEnumerable<ListContainer<T>> items = (value as List<T>).Select(item => new ListContainer<T>(keyValue, item));

            connection.UpdateAll(items);
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

using SQLite.ORM.Columns;
using SQLite.ORM.Columns.PropertyAttributeChecker;
using SQLite.ORM.Columns.PropertyCollection;
using SQLite.ORM.TableAttributeCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if NO_CONCURRENT
using ConcurrentStringDictionary = System.Collections.Generic.Dictionary<string, object>;
#else
using ConcurrentStringDictionary = System.Collections.Concurrent.ConcurrentDictionary<string, object>;
using System.Reflection;
#endif

namespace SQLite.ORM
{
    public class StandardTableMapping : AbstractTableMapping
    {
        public StandardTableMapping(Type type, TableMappingConfiguration configuration,
            CreateFlags createFlags = CreateFlags.None)
        {
            MappedType = type;

			var tableAttr = new TableAttributeCollectorFactory().Create().GetAttributesForType(type);
            TableName = tableAttr != null ? tableAttr.Name : MappedType.Name;

            var tableColumns = new List<TableMappingColumn>();
            List<MemberInfo> eligibleMembers = new List<MemberInfo>();
            eligibleMembers.AddRange(configuration.PropertyCollector.Collect(type));
            eligibleMembers.AddRange(configuration.FieldCollector.Collect(type));

            foreach (var info in eligibleMembers )
            {
                tableColumns.AddRange(configuration.TableMappingColumnFactory.CreateColumnsOnMember(info, createFlags));
            }

            Columns = tableColumns.ToArray();

            _autoPk = Columns.FirstOrDefault(column => column.IsAutoInc && column.IsPK);
            PrimaryKey = Columns.FirstOrDefault(column => column.IsPK);

            if (PrimaryKey != null)
            {
                GetByPrimaryKeySql = string.Format("select * from \"{0}\" where \"{1}\" = ?", TableName, PrimaryKey.Name);
            }
            else
            {
                // People should not be calling Get/Find without a PK
                GetByPrimaryKeySql = string.Format("select * from \"{0}\" limit 1", TableName);
            }

            _insertCommandMap = new ConcurrentStringDictionary();
        }
    }
}

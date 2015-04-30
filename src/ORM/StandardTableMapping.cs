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
            string contextName,
            CreateFlags createFlags = CreateFlags.None)
        {
            MappedType = type;

            TableName = contextName + ORMUtilities.GetTableName(MappedType);
            string path = string.IsNullOrEmpty(contextName) ? "" : contextName;

            Columns = ORMUtilities.GetColumnsOnType(type, configuration, createFlags, path);

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

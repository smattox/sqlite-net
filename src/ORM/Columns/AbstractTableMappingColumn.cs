using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns
{
    public abstract class AbstractTableMappingColumn : TableMappingColumn
    {
        public string Name { get; protected set; }

        public Type TargetType { get; protected set; }

        public string TargetName { get; protected set; }

        public bool IsAutoInc { get; protected set; }

        public bool IsPK { get; protected set; }

        public bool IsNullable { get; protected set; }

        public bool IsAutoGuid { get; protected set; }

        public string Collation { get; protected set; }

        public int? MaxStringLength { get; protected set; }

        public IEnumerable<IndexedAttribute> Indices { get; set; }

        protected AbstractTableMappingColumn(MemberInfo info, string path, CreateFlags createFlags = CreateFlags.None)
        {
            Name = path + ORMUtilities.GetColumnName(info);
            Collation = ORMUtilities.Collation(info);

            IsPK = ORMUtilities.IsPrimaryKey(info) ||
                (((createFlags & CreateFlags.ImplicitPK) == CreateFlags.ImplicitPK) &&
                    string.Compare(info.Name, ORMUtilities.ImplicitPkName, StringComparison.OrdinalIgnoreCase) == 0);

            var isAuto = ORMUtilities.IsAutoInc(info) || (IsPK && ((createFlags & CreateFlags.AutoIncPK) == CreateFlags.AutoIncPK));
            IsAutoGuid = isAuto && TargetType == typeof(Guid);
            IsAutoInc = isAuto && !IsAutoGuid;

            Indices = ORMUtilities.GetIndices(info);
            if (!Indices.Any()
                && !IsPK
                && ((createFlags & CreateFlags.ImplicitIndex) == CreateFlags.ImplicitIndex)
                && Name.EndsWith(ORMUtilities.ImplicitIndexSuffix, StringComparison.OrdinalIgnoreCase)
                )
            {
                Indices = new IndexedAttribute[] { new IndexedAttribute() };
            }
            IsNullable = !(IsPK || ORMUtilities.IsMarkedNotNull(info));
            MaxStringLength = ORMUtilities.MaxStringLength(info);
        }

        public abstract void SetValue(object obj, object val);

        public abstract object GetValue(object obj);
    }
}

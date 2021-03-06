﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns
{
    public abstract class AbstractDirectTableMappingColumn : AbstractTableMappingColumn, DirectTableMappingColumn
    {
        public bool IsAutoInc { get; protected set; }

        public bool IsPK { get; protected set; }

        public bool IsNullable { get; protected set; }

        public bool IsAutoGuid { get; protected set; }

        public string Collation { get; protected set; }

        public int? MaxStringLength { get; protected set; }

        protected AbstractDirectTableMappingColumn(MemberInfo info, string path, CreateFlags createFlags = CreateFlags.None)
            : base(info, path, createFlags)
        {
            Collation = ORMUtilities.Collation(info);

            IsPK = ORMUtilities.IsPrimaryKey(info) ||
                (((createFlags & CreateFlags.ImplicitPK) == CreateFlags.ImplicitPK) &&
                    string.Compare(info.Name, ORMUtilities.ImplicitPkName, StringComparison.OrdinalIgnoreCase) == 0);

            var isAuto = ORMUtilities.IsAutoInc(info) || (IsPK && ((createFlags & CreateFlags.AutoIncPK) == CreateFlags.AutoIncPK));
            IsAutoGuid = isAuto && TargetType == typeof(Guid);
            IsAutoInc = isAuto && !IsAutoGuid;

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
            TargetName = info.Name;
        }

        public abstract object GetValue(object obj);

        public abstract void SetValue(object obj, object value);
    }
}
